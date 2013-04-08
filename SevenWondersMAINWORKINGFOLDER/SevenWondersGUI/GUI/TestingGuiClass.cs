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
using System.Windows.Threading;
using System.Threading;
using System.Net.Sockets;
using System.Net;


namespace SevenWondersGUI
{
    class TestingGuiClass
    {
        Grid parent;
        List<Board> boards;
        List<Card> cards;
        List<PlayerState> players;
        GameState game;
        PlayerGameBoard player;
        

        public TestingGuiClass(Grid g)
        {
            parent = g;
           // player = (PlayerGameBoard)parent.Children[1];

            cardlistCreator cardMaker = new cardlistCreator();
            cards = cardMaker.getCardList();
        }

//=================================================================================================================
// Test button in the lower right that should be cycling through a list of boards
        private void boardTest_Click(object sender, RoutedEventArgs e)
        {
            List<String> boards = createBoards();
            Random rand = new Random();
            int r = rand.Next(0, 13);
            //changeBoard(boards[r]);
        }
//=================================================================================================================
//Testing method to see if cards are working. 
        private void cardTest_Click(object sender, RoutedEventArgs e)
        {
             

            Random rand = new Random();
            int r = rand.Next(0, 55);
            //player.changeCard(cards[r]);
        }
//=================================================================================================================
        private void createCardTestButton()
        {
            Canvas g = (Canvas)parent.Children[1];
            
            Button test1 = new Button();
            test1.Content = "Change Card";
            test1.Height = 25;
            test1.Width = 85;
            //test1.Click += new RoutedEventHandler();
            Canvas.SetLeft(test1, 1160);
            Canvas.SetTop(test1, 580);
            g.Children.Add(test1);
        }
//=================================================================================================================
        private void createBoardTestButton()
        {
            Canvas g = (Canvas)parent.Children[1];

            Button test2 = new Button();
            test2.Content = "Change Board";
            test2.Height = 25;
            test2.Width = 85;
            //test2.Click += new RoutedEventHandler();
            Canvas.SetLeft(test2, 1160);
            Canvas.SetTop(test2, 640);
            g.Children.Add(test2);
        }

//=================================================================================================================
 //Testing method helper
        private List<String> createBoards()
        {
            List<String> boards = new List<String>();
            boards.Add("WB1"); boards.Add("WB2"); boards.Add("WB3");
            boards.Add("WB4"); boards.Add("WB5"); boards.Add("WB6");
            boards.Add("WB7"); boards.Add("WB8"); boards.Add("WB9");
            boards.Add("WB10"); boards.Add("WB11"); boards.Add("WB12");
            boards.Add("WB13"); boards.Add("WB14");
            return boards;
        }


    }
}
