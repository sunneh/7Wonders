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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SevenWondersGUI
{
    class WaitingToStartCanvasPlayer
    {
        Grid parent;

        public WaitingToStartCanvasPlayer(Grid gameGrid)
        {
            parent = gameGrid;
            
            Canvas waitingCanvas = new Canvas();
            ImageBrush brush = new ImageBrush(new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/backGroundTwo.jpg", UriKind.RelativeOrAbsolute)));

            Canvas.SetTop(waitingCanvas, 0);
            Canvas.SetLeft(waitingCanvas, 0);
            waitingCanvas.Background = brush;
            gameGrid.Children.Add(waitingCanvas);

            Button b2 = new Button();
            b2.Content = "Cancel";
            b2.Width = 75;
            b2.Height = 30;
            Canvas.SetLeft(b2, 700);
            Canvas.SetTop(b2, 600);
            b2.Click += new RoutedEventHandler(cancelGameButton_Click);
            b2.HorizontalAlignment = HorizontalAlignment.Stretch;
            b2.VerticalAlignment = VerticalAlignment.Stretch;
            waitingCanvas.Children.Add(b2);
        }

        private void cancelGameButton_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(parent.Children[1]);
            StartGameCanvas start = new StartGameCanvas(parent);
        }


    }

}
