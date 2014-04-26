using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WP8TwosUnlimited
{
    public partial class GamePage : PhoneApplicationPage
    {
        public static readonly int SwipeDelta = 10;

        public GamePage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (!gameControl.GameOver)
            {
                double x = e.TotalManipulation.Translation.X;
                double y = e.TotalManipulation.Translation.Y;

                if (Math.Abs(y) > Math.Abs(x) && Math.Abs(y) > SwipeDelta)
                {
                    gameControl.DoMove(y < 0.0 ? GameMove.Up : GameMove.Down);
                }
                else if (Math.Abs(x) > SwipeDelta)
                {
                    gameControl.DoMove(x < 0.0 ? GameMove.Left : GameMove.Right);
                }
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, CancelEventArgs e)
        {
            if (!gameControl.GameOver)
            {
                if (MessageBox.Show("You'll forfeit this game if you choose ok.\n\nIf you just want to switch to another app without quitting, choose cancel and press the Start button.",
                    "Quit?", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}