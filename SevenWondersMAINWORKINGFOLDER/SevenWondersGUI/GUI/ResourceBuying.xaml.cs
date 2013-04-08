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
    /// Interaction logic for ResourceBuying.xaml
    /// </summary>
    public partial class ResourceBuying : Window
    {
        private  Resources   resources   = null;
        private GameState game;
        private  PlayerState you  = null;
        private  Card        card = null;
        private  PlayerGameBoard board = null;

        private static ResourceBuying _instance = null;
        private static ResourceManager manager = ResourceManager.GetInstance();     
        
        public ResourceBuying(Resources res, Card c, PlayerGameBoard p, GameState g)
        {
            resources = res;

            //Console.WriteLine(resources.ToString());
            
            card    = c;
            board   = p;
            game    = g;
            manager = ResourceManager.GetInstance();


            you = manager.getGameState().getPlayerNum(resources.getPlayerName()[1]);
            
            InitializeComponent();
            createButtons();
            createValueFields();
        }

        // when i = 0:you, i=1:left, i=2:right
        // The buttons appear for you when you have special resources. If this is 
        // zero then you don't need buttons and none appear.
        // The buttons for the left and right on the other hand will appear when any 
        // COMBINED(special and base) resource is > 0.
        // This is so you can choose which resource to add from yourself and which 
        // resource to buy from your neighbor.

        private void createButtons()
        {
            int skip = 0;
            int counter = 0;
            int[] currentTotals = new int[]{ 0, 0, 0, 0, 0, 0, 0 };

            for(int i = 0; i < 3; i++)
            {                
                // This is you
                if (i == 0)
                {
                    //currentTotals = rm.SpecialResourceArray(rc.getPlayerName());                    
                    currentTotals = manager.SpecialResourceArray(you.getName());
                    
                    Console.WriteLine ( "YOU Special Resources: [{0}] [{1}] [{2}] [{3}] [{4}] [{5}] [{6}]"
                                      ,  currentTotals[0] 
                                      ,  currentTotals[1] 
                                      ,  currentTotals[2] 
                                      ,  currentTotals[3] 
                                      ,  currentTotals[4]
                                      ,  currentTotals[5]
                                      ,  currentTotals[6]
                                      );
                }

                // This is Left
                if (i == 1)
                {
                    currentTotals = manager.tradableArray(you, 1);
                }

                // This is Right
                if (i == 2) 
                {
                    currentTotals = manager.tradableArray(you, 2);
                }
               
                for (int j = 0; j < 7; j++)
                {
                    counter++;

                    Button b = new Button();
                    b.Content = "+";
                    b.Name = "B" + (string)counter.ToString();//You: 1-7, Left:8-14, Right:15-21
                    b.Width   = 25;
                    b.Height = 27;
                    b.Click += new RoutedEventHandler(buyButton_Click);
                    Grid.SetColumn(b, j);
                    Grid.SetRow(b, (i+1+skip));

                    if (currentTotals[j] > 0)
                    {
                        //Console.WriteLine("Player #: " + i + " In index: " + j + " value is: "+ currentTotals[j]);
                        b.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        b.Visibility = Visibility.Hidden;
                    }
                    mainGrid.Children.Add(b);
                }
                skip += 1;
            }
        }
//
//when i = 0:you, i=1:left, i=2:right
// The value field for YOU is just your base resources and excludes special resources.
// The value field for LEFT and RIGHT are the combination of special and base resources.
        private void createValueFields()
        {
            int skip = 0;
            int[] currentTotals;

            for (int i = 0; i < 3; i++)
            {
                currentTotals = manager.trArray(you); //getCurrentTotals(i);//This is YOU
                
                if (i == 1)//This is LEFT
                {
                    currentTotals = manager.tradableArray(you, 1);
                }
                if (i == 2)//This is RIGHT 
                {
                    currentTotals = manager.tradableArray(you, 2);
                }

                for (int j = 0; j < 7; j++)
                {
                    Label l = new Label();

                    l.Content = currentTotals[j];
                    l.Width = 25;
                    l.Height = 27;
                    Grid.SetColumn(l, j);
                    Grid.SetRow(l, (i + 2 + skip));
                    mainGrid.Children.Add(l);
                }
                skip += 1;
            }
        }

        //when i = 0:you, i=1:left, i=2:right
        private int[] getCurrentTotals(int i)
        {
            if (i == 0)
            {
                return resources.getCenter();
            }
            else
            if (i == 1)
            {
                return resources.getLeft();
            }
            else
            {
                return resources.getRight();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = (Canvas)board.getGrid().Children[1];
            Label l = (Label)c.Children[90];
            l.Content = " " + you.getCoins();
            this.Close();
        }

// This button click handler finds which button you pressed and then modifies the appropriate 
// Player resources as well calling methods for removing and adding money
// B[1,7] = YOU, B[8,14] = LEFT, B[15-21] = RIGHT
        private void buyButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            string name = b.Name;//have button name will travel
            
            int index;

            if (name.Length > 2)
            {
                index = ((int)Char.GetNumericValue(name[1])*10) + (int)Char.GetNumericValue(name[2]);
            }
            else
            {
                index = (int)Char.GetNumericValue(name[1]);//know which number
            }
            
            if(index > 0 && index < 8)//Player clicked on YOU
            {
                
                manager.usingSpecialResource(you, (index-1));
                resources = manager.GetCombinedResources(you);                
             
                if (manager.ValidateCard(you, card))//ok check if you can play the card now.
                {                    
                    you.addPlayedCards(card);//add to the playerState
                    board.changeCard(card);//adds to the board cards                    
                    you.setPlayedACard();//set true
                    you.getHand().Remove(card);//remove from cards in hand of player
                    manager.getGameState().incrementTurn();
                    this.Close();
                    PlayerGameBoard play = new PlayerGameBoard(board.getGrid(), you, manager.getGameState());
                }
                else
                {           
                    this.Close();

                    ResourceBuying window = new ResourceBuying(resources, card, board, game);
                    window.Show();
                }
                
            }

            if (index > 7 && index < 15)//Player clicked on LEFT
            {
                int i = (index-8);

                if (manager.canAfford(you))
                {
                    Canvas c = (Canvas)board.getGrid().Children[1];
                    Label l = (Label)c.Children[90];
                    l.Content = " " + manager.getCoinTransaction();

                    Console.WriteLine("YOU current money: " + you.getCoins()); 
                    manager.tradeTo(you, i, 0);
                    resources = manager.GetCombinedResources(you); 

                    if (manager.ValidateCard(you, card))//ok check if you can play the card now.
                    {
                        you.addPlayedCards(card);//add to the playerState
                        board.changeCard(card);//adds to the board cards                    
                        you.setPlayedACard();//set true
                        you.getHand().Remove(card);//remove from cards in hand of player
                        manager.getGameState().incrementTurn();
                        this.Close();
                        PlayerGameBoard play = new PlayerGameBoard(board.getGrid(), you, manager.getGameState());
                    }
                    else
                    {
                        this.Close();
                        ResourceBuying window = new ResourceBuying(resources, card, board, game);
                        window.Show();
                    }
                }
            }

            if (index > 14 && index < 22)//Player clicked on RIGHT
            {
                int i = (index - 15);//0-6 the resource traded
                
                if (manager.canAfford(you))
                {
                    Canvas c = (Canvas)board.getGrid().Children[1];
                    Label l = (Label)c.Children[90];
                    l.Content = " " + manager.getCoinTransaction();

                    manager.tradeTo(you, i, 1);
                    resources = manager.GetCombinedResources(you); 

                    if (manager.ValidateCard(you, card))//ok check if you can play the card now.
                    {
                        you.addPlayedCards(card);//add to the playerState
                        board.changeCard(card);//adds to the board cards                    
                        you.setPlayedACard();//set true
                        you.getHand().Remove(card);//remove from cards in hand of player
                        manager.getGameState().incrementTurn();
                        this.Close();
                        PlayerGameBoard playt = new PlayerGameBoard(board.getGrid(), you, manager.getGameState());
                    }
                    else
                    {
                        this.Close();
                        ResourceBuying window = new ResourceBuying(resources, card, board, game);
                        window.Show();
                    }
                }
            }
        }


        public static ResourceBuying GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ResourceBuying(null, null, null, null);
            }
            return _instance;
        }

        public static ResourceBuying GetInstance(Resources r, Card c, PlayerGameBoard p, GameState g)
        {
            if (_instance == null)
            {
                _instance = new ResourceBuying(r, c, p, g);
            }
            else
            {
                _instance.Close();
                _instance.reset();
                _instance = new ResourceBuying(r, c, p, g);
            }
            return _instance;
        }

        public void reset()
        {
            _instance = null;
        }
        
    }
}
