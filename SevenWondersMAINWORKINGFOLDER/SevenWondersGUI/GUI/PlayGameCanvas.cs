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
    public class PlayGameCanvas
    {
        Grid parent;

        public PlayGameCanvas(Grid gameGrid)
        {
            parent = gameGrid;

            ImageBrush brush = new ImageBrush(new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/backGroundTwo.jpg", UriKind.RelativeOrAbsolute)));
            brush.Stretch = Stretch.Fill;
            brush.TileMode = TileMode.FlipXY;
            Canvas play = new Canvas
            {
                Background = brush,
                Name = "playCanvas",
            };
            Label enter = new Label();
            enter.Content = "To Start Please Enter The IP Address of The Host";
            enter.FontSize = 28;
            Canvas.SetLeft(enter, 330);
            Canvas.SetTop(enter, 190);
            enter.Foreground = Brushes.White;
            play.Children.Add(enter);

            Label add = new Label();
            add.Content = "Address";
            add.FontSize = 22;
            Canvas.SetTop(add, 310);
            Canvas.SetLeft(add, 330);
            add.Foreground = Brushes.White;
            play.Children.Add(add);

            Button b1 = new Button();
            b1.Content = "Play!";
            b1.FontSize = 22;
            Canvas.SetLeft(b1, 800);
            Canvas.SetTop(b1, 310);
            b1.Width = 110;
            b1.Click += new RoutedEventHandler(playGame_Click);
            play.Children.Add(b1);


            Button b2 = new Button();
            b2.Content = "Go Back";
            b2.FontSize = 22;
            Canvas.SetLeft(b2, 940);
            Canvas.SetTop(b2, 310);
            b2.Width = 110;
            b2.Click += new RoutedEventHandler(goBack_Click);
            play.Children.Add(b2);

            TextBox tb = new TextBox();
            tb.Width = 300;
            tb.FontSize = 22;
            tb.MaxLines = 1;
            tb.MaxLength = 15;
            Canvas.SetLeft(tb, 450);
            Canvas.SetTop(tb, 310);
            play.Children.Add(tb);

            //play.
            Canvas.SetZIndex(play, 100);
            parent.Children.Add(play);
           
        }

        //"play" button on play canvas "start hosting" on host canvas
        private void playGame_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(parent.Children[1]);

            /*var connecting = new WaitingOnConnectionWindow();
            connecting.Show();
            WondersServer server = new WondersServer(3004);
            ThreadStart ts = new ThreadStart(server.serverStart);
            Thread thread = new Thread(ts );
            thread.Start();*/

            WaitingToStartCanvasPlayer wait = new WaitingToStartCanvasPlayer(parent);
        }
        //GoBack button for play and host canvas
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(parent.Children[1]);
            StartGameCanvas start = new StartGameCanvas(parent);
        }

        private void CenterScreen()
        {
            double frm_width = this.parent.Width;
            double frm_height = this.parent.Height;

            System.Windows.Forms.Screen src = System.Windows.Forms.Screen.PrimaryScreen;
            int src_height = src.Bounds.Height;
            int src_width = src.Bounds.Width;

            //this.parent.Left = (src_width - frm_width) / 2;
            //this.parent.Top = (src_height - frm_height) / 2;
        }


    }
}
