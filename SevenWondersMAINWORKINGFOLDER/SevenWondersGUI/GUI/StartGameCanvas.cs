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
    public class StartGameCanvas
    {
        Grid parent;

        public StartGameCanvas(Grid gameGrid)
        {
            parent = gameGrid;
            ImageBrush brush = new ImageBrush(new BitmapImage(
            new Uri(@"pack://application:,,,/Images/startBackground.png", UriKind.RelativeOrAbsolute)));
            brush.Stretch = Stretch.Fill;

            Canvas start = new Canvas
            {
                Background = brush,
                Name = "startingCanvas",
            };

            ImageBrush buttbrush = new ImageBrush(new BitmapImage(
            new Uri(@"pack://application:,,,/Images/startBackground.png", UriKind.RelativeOrAbsolute)));
            

            Button play = new Button();
            play.Width = 130;
            play.Height = 35;
            play.FontSize = 22;
            Canvas.SetTop(play, 434);
            Canvas.SetLeft(play, 629);
            buttbrush.Stretch = Stretch.None;
            play.Background = buttbrush;
            play.Content = "Play!";
            play.Click += new RoutedEventHandler(PlayGameButton_Click);
            start.Children.Add(play);

            Button host = new Button();
            host.Width = 130;
            host.Height = 35;
            Canvas.SetTop(host, 368);
            Canvas.SetLeft(host, 629);
            host.Background = buttbrush;
            host.Content = "Host!";
            host.FontSize = 22;
            host.Click += new RoutedEventHandler(HostGameButton_Click);
            start.Children.Add(host);

            Canvas.SetZIndex(start, 1);
            parent.Children.Add(start);
        }
        //"Play!" Button on StartCanvas
        private void PlayGameButton_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(parent.Children[1]);
            PlayGameCanvas play = new PlayGameCanvas(parent);
        }
        //"Host!" Button on StartCanvas
        private void HostGameButton_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(parent.Children[1]);
            HostGameCanvas host = new HostGameCanvas(parent);
        }

    }
}
