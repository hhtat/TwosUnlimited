using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.Windows;

namespace WP8TwosUnlimited
{
    public partial class GamePage : PhoneApplicationPage
    {
        public GamePage()
        {
            InitializeComponent();
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