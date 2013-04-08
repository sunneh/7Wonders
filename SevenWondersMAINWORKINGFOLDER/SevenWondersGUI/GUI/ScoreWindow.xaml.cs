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
    /// Interaction logic for ScoreWindow.xaml
    /// </summary>
    public partial class ScoreWindow : Window
    {
        GameState game;
        Grid parent;

        public ScoreWindow(GameState g, Grid gg)
        {
            game = g;
            parent = gg;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ScoreWindow_Loaded);
            addPlayers(scoreGrid);//scoreGrid is named in XAML
            addButton(scoreGrid);
        }

        void ScoreWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        private void addButton(Grid g)
        {
            Button b = new Button();
            b.Content = "Start Over";
            b.Width = 76;
            b.Height = 48;
            b.Opacity = 0.5;
            b.Click += new RoutedEventHandler(startOver_Click);

            Grid.SetRow(b,1 );
            Grid.SetColumn(b, 1);
            g.Children.Add(b);
        }

        private void startOver_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.GetInstance().Reset();
            parent.Children.Remove(parent.Children[1]);
            this.Close();
            StartGameCanvas start = new StartGameCanvas(parent);
        }

        private void addPlayers(Grid g)
        {
            for (int i = 0; i < game.getPlayers().Count; i++)
            {
                Label l = new Label();
                PlayerState player = game.getPlayers()[i];
                List<int> scores = player.getScores(game);

                l.FontSize = 24;
                l.Content = player.getName();//"P" + (i+1);//TODO change so name shows AI geoffry

                Grid.SetColumn(l,(i+2));
                Grid.SetRow(l, 1);
                g.Children.Add(l);

                int sum = 0;
                for (int j = 0; j < 7; j++)
                {
                    sum += scores[j];
                    Label v = new Label();

                    v.FontSize = 24;
                    v.Content = scores[j];
                   
                    Grid.SetRow(v, (j+2));
                    Grid.SetColumn(v, (i + 2));
                    g.Children.Add(v);
                }

                Label s = new Label();

                s.FontSize = 24;
                s.Content = sum;

                Grid.SetRow(s, (9));
                Grid.SetColumn(s, (i + 2));
                g.Children.Add(s);

            }
        }
    }
}
