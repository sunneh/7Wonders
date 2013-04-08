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
    class WaitingToStartCanvasHost
    {
        //int numPlayers = 0;

        Grid parent;
        String input;
        TextBox tb;
        List<TextBox> textboxes = new List<TextBox>();
        Canvas waitingCanvas;
        ComboBox cb;
        //menu

        public WaitingToStartCanvasHost(Grid gameGrid)
        {
            parent = gameGrid;
            waitingCanvas = new Canvas();
            ImageBrush brush = new ImageBrush(new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/backGroundTwo.jpg", UriKind.RelativeOrAbsolute)));            
            Canvas.SetTop(waitingCanvas, 0);
            Canvas.SetLeft(waitingCanvas, 0);
            waitingCanvas.Background = brush;
            parent.Children.Add(waitingCanvas);

            Button b1 = new Button();
            b1.Content = "Start";
            b1.Width = 75;
            b1.Height = 30;
            Canvas.SetLeft(b1, 590);
            Canvas.SetTop(b1, 600);
            b1.Click += new RoutedEventHandler(startGameButton_Click);
            b1.HorizontalAlignment = HorizontalAlignment.Stretch;
            b1.VerticalAlignment = VerticalAlignment.Stretch;
            waitingCanvas.Children.Add(b1);

            Button b2 = new Button();
            b2.Content = "Cancel";
            b2.Width = 75;
            b2.Height = 30;
            Canvas.SetLeft(b2, 670);
            Canvas.SetTop(b2, 600);
            b2.Click += new RoutedEventHandler(cancelGameButton_Click);
            b2.HorizontalAlignment = HorizontalAlignment.Stretch;
            b2.VerticalAlignment = VerticalAlignment.Stretch;
            waitingCanvas.Children.Add(b2);            

            Label enter = new Label();
            enter.Content = "Enter the Number of Players, 3-7 in the player 7 box";
            enter.FontSize = 28;
            Canvas.SetLeft(enter, 330);
            Canvas.SetTop(enter, 190);
            enter.Foreground = Brushes.White;
            waitingCanvas.Children.Add(enter);

            Label addAI = new Label();
            addAI.Content = "Add AI and Type";
            addAI.FontSize = 20;
            Canvas.SetLeft(addAI, 780);
            Canvas.SetTop(addAI, 230);
            addAI.Foreground = Brushes.White;
            waitingCanvas.Children.Add(addAI);

            createButton();
            createLabels();
            createTextBoxes();
            //System.Console.WriteLine("WaitingToStartCanvasHost()");

            NameScope.SetNameScope(waitingCanvas, new NameScope());


        }

        private void createTextBoxes()
        {
            int left = 450;
            int top = 270;
            
            for (int i = 0; i < 7; i++)
            {
                tb = new TextBox();
                tb.Name = "TB00" + i.ToString();
                tb.Width = 300;
                tb.FontSize = 22;
                tb.MaxLines = 1;
                tb.MaxLength = 2;                
               
                Canvas.SetLeft(tb, left);
                Canvas.SetTop(tb, top);
                waitingCanvas.Children.Add(tb);
                textboxes.Add(tb);
                top += 40;

                if (i == 0)
                {
                    tb.Text = "P1";
                }else
                if (i > 0)
                {
                    tb.Text = "A" + (i + 1);//for testing only remove for live game
                }
               // waitingCanvas.RegisterName(tb.Name, tb);
            }
        }

        private void createLabels()
        {
            int left = 330;
            int top = 270;

            for (int i = 0; i < 7; i++)
            {
                Label add = new Label();
                add.Content = "Player " + (i+1) + " : ";
                add.FontSize = 22;
                Canvas.SetTop(add, top);
                Canvas.SetLeft(add, left);
                add.Foreground = Brushes.White;
                waitingCanvas.Children.Add(add);
                top += 40;
            }
        }

        private void createButton()
        {
            int left = 780;
            int top = 270;

            for (int i = 0; i < 7; i++)
            {                               
                cb = new ComboBox();
                cb.Name = "CB00" + i.ToString();
                Console.WriteLine("Named: " + cb.Name);
                cb.DropDownClosed += new EventHandler(ComboBox1_DropDownClosed);

                ComboBoxItem stg00 = new ComboBoxItem();
                stg00.Content = "";
                stg00.Background = Brushes.Gray;
                cb.Items.Add(stg00);

                ComboBoxItem stg01 = new ComboBoxItem();
                stg01.Content = "Human Player";
                stg01.Background = Brushes.Gray;
                cb.Items.Add(stg01);

                ComboBoxItem stg02 = new ComboBoxItem();
                stg02.Content = "Random Strategy";
                stg02.Background = Brushes.LightBlue;
                cb.Items.Add(stg02);

                ComboBoxItem stg03 = new ComboBoxItem();
                stg03.Content = "Aggressive Strategy";
                stg03.Background = Brushes.LightBlue;
                cb.Items.Add(stg03);

                ComboBoxItem stg04 = new ComboBoxItem();
                stg04.Content = "Civilian Strategy";
                cb.Items.Add(stg04);

                ComboBoxItem stg05 = new ComboBoxItem();
                stg05.Content = "Commercial Strategy";
                stg05.Background = Brushes.LightBlue;
                cb.Items.Add(stg05);

                ComboBoxItem stg06 = new ComboBoxItem();
                stg06.Content = "Discrete Strategy";
                cb.Items.Add(stg06);

                ComboBoxItem stg07 = new ComboBoxItem();
                stg07.Content = "Military Strategy";
                stg07.Background = Brushes.LightBlue;
                cb.Items.Add(stg07);

                ComboBoxItem stg08 = new ComboBoxItem();
                stg08.Content = "Scientific Strategy";
                cb.Items.Add(stg08);                

                ComboBoxItem stg09 = new ComboBoxItem();
                stg09.Content = "Adaptive Strategy";
                cb.Items.Add(stg09);

                cb.SelectedIndex = 0;
                cb.Width = 150;
                cb.Height = 30;
                Canvas.SetLeft(cb, left);
                Canvas.SetTop(cb, top);
                
                cb.HorizontalAlignment = HorizontalAlignment.Stretch;
                cb.VerticalAlignment = VerticalAlignment.Stretch;

                if (i == 0)
                    cb.IsEnabled = false;//assume player 1 is a human
                    cb.SelectedIndex = 1;
                if (i > 0)
                    cb.SelectedIndex = 2;//for testing, remove for live game

                //if (i == 2)
                   // cb.SelectedIndex = 2;

                waitingCanvas.Children.Add(cb);
                top += 40;

               // waitingCanvas.RegisterName(cb.Name, cb);
            }
        }
        
        private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(parent.Children[1]);     //hostCanvas

            List<TextBox>  tbList = new List<TextBox>(7);
            List<ComboBox> cbList = new List<ComboBox>(7);
            List<Player>  players = new List<Player>();

            input = this.tb.Text;            
            UIElementCollection c = waitingCanvas.Children;
            //System.Console.WriteLine("\n\n\n");
            
            for (int i = 0; i < c.Count; i++)
            {                                               
                UIElement el = c[i];

                if (c[i].GetType() == typeof(System.Windows.Controls.ComboBox))
                    cbList.Add((ComboBox)c[i]);

                if (c[i].GetType() == typeof(System.Windows.Controls.TextBox))
                    tbList.Add((TextBox)c[i]);
            }

            for (int i = 0; i < tbList.Count; i++)
            {
                ComboBox cb = null;
                TextBox tb = tbList[i];

                if (tb.Text.Length != 0 && tb.Text != " ")
                {
                    cb = this.getComboBox(cbList, tb.Name.Substring(4, 1)); 
                    players.Add(new Player(i, cb.SelectedIndex));
                }
            }

            if (players.Count < 3)
            {
                System.Windows.MessageBox.Show("C'mon you need at least three players!");
                WaitingToStartCanvasHost wait = new WaitingToStartCanvasHost(parent);
            }
            else
            {                
                StartGame start = new StartGame(players);
                //TODO NETWORKING   need a text field input of how many players are connected
                //Just read the text fields?...Accumulate
                GameState game = start.getGame();
                List<PlayerState> ps = game.getPlayers();

                PlayerGameBoard play = new PlayerGameBoard(parent, ps[0], game);//this creates the canvas that shows.                
                //TODO need to make child 0 player and child 1 current view.
            }
        }

        private ComboBox getComboBox(List<ComboBox> l, string index)
        {
            string cbName = "CB00" + index;
            for (int i = 0; i < l.Count; i++)
                if (l[i].Name == cbName)
                    return l[i];
            return null;
        }

        private void cancelGameButton_Click(object sender, RoutedEventArgs e)
        {
            //System.Console.WriteLine("WaitingToStartCanvasHost():: cancelGameButton_Click()"); 
            parent.Children.Remove(parent.Children[1]);
            StartGameCanvas start = new StartGameCanvas(parent);
        }

//This method updates the appropriate text fields with an AI rather than player id 
        private void ComboBox1_DropDownClosed(Object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            string name = c.Name;
            char st = name[4];
            TextBox cur = new TextBox();

            Console.WriteLine("st is: " + st);

            foreach (TextBox t in textboxes)
            {
                if (t.Name.Equals("TB00" + st))//we have the same textbox as combobox
                {
                    Console.WriteLine("tb name is: " + t.Name + " Selected Index = " + c.SelectedIndex);
                    
                    if (c.SelectedIndex == 0)
                    {
                        t.Text = " ";
                        return;
                    }
                    if (c.SelectedIndex == 1)
                    {
                        t.Text = "P" + st;
                        return;
                    }
                    else
                    {
                        t.Text = "A" + st;
                        return;
                    }
                }
            }
        }
    }    
}
