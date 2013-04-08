using System;
using System.Net;
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
using System.Threading;


namespace SevenWondersGUI
{
    public class HostGameCanvas
    {
        Grid parent;

        public HostGameCanvas(Grid gameGrid)
        {
            parent = gameGrid;

            ImageBrush brush = new ImageBrush(new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/backGroundTwo.jpg", UriKind.RelativeOrAbsolute)));
            brush.Stretch = Stretch.Fill;
            brush.TileMode = TileMode.FlipXY;
            
            Canvas host = new Canvas
            {
                Background = brush,
                Name = "hostCanvas",
            };
            
            Label enter = new Label();
            enter.Content = "To Start Hosting Please Enter The IP Address of The Host";
            enter.FontSize = 28;
            Canvas.SetLeft(enter, 330);
            Canvas.SetTop(enter, 190);
            enter.Foreground = Brushes.White;
            host.Children.Add(enter);

            Label add = new Label();
            add.Content = "Host IP";
            add.FontSize = 22;
            Canvas.SetTop(add, 310);
            Canvas.SetLeft(add, 330);
            add.Foreground = Brushes.White;
            host.Children.Add(add);

            Button b1 = new Button();
            b1.Content = "Start Hosting";
            b1.FontSize = 22;
            Canvas.SetLeft(b1, 780);
            Canvas.SetTop(b1, 310);
            b1.Width = 150;
            b1.Click += new RoutedEventHandler(startHostingGame_Click);
            host.Children.Add(b1);

            Button b2 = new Button();
            b2.Content = "Go Back";
            b2.FontSize = 22;
            Canvas.SetLeft(b2, 960);
            Canvas.SetTop(b2, 310);
            b2.Width = 150;
            b2.Click += new RoutedEventHandler(goBack_Click);
            host.Children.Add(b2);

            TextBox tb = new TextBox();
            tb.Width = 300;
            tb.Text = addIPAddress();
            tb.IsReadOnly = true;
            tb.FontSize = 22;
            tb.MaxLines = 1;
            tb.MaxLength = 0;
            Canvas.SetLeft(tb, 450);
            Canvas.SetTop(tb, 310);
            host.Children.Add(tb);

            Canvas.SetZIndex(host, 100);
            parent.Children.Add(host);
        }

        //"play" button on play canvas "start hosting" on host canvas
        private void startHostingGame_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(parent.Children[1]);

            /*var connecting = new WaitingOnConnectionWindow();
            connecting.Show(); */

            WondersServer server = new WondersServer(3004);
            ThreadStart ts = new ThreadStart(server.serverStart);
            Thread serverThread = new Thread(ts);
            serverThread.Start();

            ClientDblBuff commBuffer = new ClientDblBuff();
            ClientComm client = new ClientComm("127.0.0.1", commBuffer);
            ThreadStart cs = new ThreadStart(client.ClientRun);
            Thread clientThread = new Thread(cs);
            clientThread.Start();

            WaitingToStartCanvasHost wait = new WaitingToStartCanvasHost(parent);
        }

        private string addIPAddress()
        {
            IPHostEntry host;
            
            string localIP = "?";
            
            host = Dns.GetHostEntry(Dns.GetHostName());
            
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }

            return localIP;
        }

        //GoBack button for the host canvas
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(parent.Children[1]);
            StartGameCanvas start = new StartGameCanvas(parent);
        }
    }
}
