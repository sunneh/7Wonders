using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SevenWondersGUI
{
    /// <summary>
    /// Interaction logic for RulesWindow.xaml
    /// </summary>
    public partial class RulesWindow : Window
    {
        int pageNumber = 1;

        public RulesWindow()
        {
            InitializeComponent();
            image.BeginInit();
            image.Source = new BitmapImage(
                new Uri(@"pack://application:,,,/Images/7wonders-rules-1.png", UriKind.RelativeOrAbsolute));
            image.EndInit();

        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber < 12 && pageNumber >= 1)
            {
                pageNumber += 1;
                image.BeginInit();
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/7wonders-rules-" + pageNumber + ".png", UriKind.RelativeOrAbsolute));
                image.EndInit();                
            }
            else
            {
                pageNumber = 1;

                image.BeginInit();
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/7wonders-rules-" + pageNumber + ".png", UriKind.RelativeOrAbsolute));
                image.EndInit();
            }
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber > 1 && pageNumber <= 12)
            {
                pageNumber -= 1;
                image.BeginInit();
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/7wonders-rules-" + pageNumber + ".png", UriKind.RelativeOrAbsolute));
                image.EndInit();
            }
            else
            {
                pageNumber = 12;

                image.BeginInit();
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/7wonders-rules-" + pageNumber + ".png", UriKind.RelativeOrAbsolute));
                image.EndInit();
            }
        }
    }
}
