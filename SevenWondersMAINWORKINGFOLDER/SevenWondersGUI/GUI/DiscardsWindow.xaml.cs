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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SevenWondersGUI
{
    /// <summary>
    /// Interaction logic for DiscardsWindow.xaml
    /// </summary>
    public partial class DiscardsWindow : Window
    {
        
        List<Card> discards;
        int index = 0;
        ResourceManager rm = ResourceManager.GetInstance();

        public DiscardsWindow()
        {
            discards = rm.getGameState().getDiscards();
           
            InitializeComponent();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (index < discards.Count)
            {
                Card c = discards.ElementAt(index);

                image.BeginInit();
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/" + c.getName() + ".jpg", UriKind.RelativeOrAbsolute));
                image.EndInit();

                index++;
            }
            else {
                index = 0;
                Card c = discards.ElementAt(index);

                image.BeginInit();
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/" + c.getName() + ".jpg", UriKind.RelativeOrAbsolute));
                image.EndInit();

                index++;                  
            }           
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            if (index >= 0 && index < discards.Count )
            {
                Card c = discards.ElementAt(index);

                image.BeginInit();
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/" + c.getName() + ".jpg", UriKind.RelativeOrAbsolute));
                image.EndInit();

                index--;
            }
            else {             
                index = (discards.Count - 1);
                Card c = discards.ElementAt(index);

                image.BeginInit();
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/" + c.getName() + ".jpg", UriKind.RelativeOrAbsolute));
                image.EndInit();

                index--;
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
