using Microsoft.Phone.Controls;
using System;
using System.Windows;

namespace WP8TwosUnlimited
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/" + typeof(GamePage).Name + ".xaml", UriKind.Relative));
        }

        private void instructionsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Just swipe and combine!", "It's pretty simple \u2026", MessageBoxButton.OK);
        }
    }
}