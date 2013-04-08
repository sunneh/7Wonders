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
        private  Resources   rc   = null;
        private  PlayerState you  = null;
        private  Card        card = null;
        private  PlayerGameBoard board = null;

        private static ResourceBuying _instance = null;
        private static ResourceManager rm = ResourceManager.GetInstance();     
        
        public ResourceBuying(Resources res, Card c, PlayerGameBoard p)
        {
            rc = res;
            card = c;
            board = p;
            you = rm.getGameState().getPlayerNum(rc.getPlayerName()[1]);
            
            InitializeComponent();
            createButtons();
            createValueFields();
        }

        //when i = 0:you, i=1:left, i=2:right
        //The buttons appear for you when you have special resources. If this is zero then you don't need buttons and none appear.
        //The buttons for the left and right on the other hand will appear when any COMBINED(special and base) resource is > 0.
        //This is so you can choose which resource to add from yourself and which resource to buy from your neighbor.
        private void createButtons()
        {
            int skip = 0;
            int counter = 0;
            int[] currentTotals = new int[]{ 0, 0, 0, 0, 0, 0, 0 };

            for(int i = 0; i < 3; i++)
            {
                currentTotals = rm.SpecialResourceArray(rc.getPlayerName());//this is YOU

                if (i == 0)
                {
                    Console.WriteLine("{" + currentTotals[0] + "," + currentTotals[1] + "," + currentTotals[2] + "," + currentTotals[3] + "," + currentTotals[4] +
                        "," + currentTotals[5] + "," + currentTotals[6] + "}");
                }

                if (i == 1)//This is LEFT
                {
                    currentTotals = rm.tradableArray(you, 0);
                }
                if (i == 2)//This is RIGHT 
                {
                    currentTotals = rm.tradableArray(you, 1);
                }
               
                for (int j = 0; j < 7; j++)
                {
                    counter++;
                    //Console.WriteLine("Value of Counter: " + counter);
                    Button b = new Button();
                    b.Content = "+";
                    b.Name = "B" + (string)counter.ToString();//You: 1-7, Left:8-14, Right:15-21
                    b.Width   = 25;
                    b.Height = 27;
                    b.Click += new RoutedEventHandler(buyButton_Click);
                    Grid.SetColumn(b, j);
                    Grid.SetRow(b, (i+1+skip));
                   // b.RegisterName(b.Name, b);

                    //Console.WriteLine("Named: "+ b.Name);

                    if (currentTotals[j] > 0)
                    {
                        Console.WriteLine("Player #: " + i + " In index: " + j + " value is: "+ currentTotals[j]);
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

        //when i = 0:you, i=1:left, i=2:right
        // The value field for YOU is just your base resources and excludes special resources.
        // The value field for LEFT and RIGHT are the combination of special and base resources.
        private void createValueFields()
        {
            int skip = 0;
            int[] currentTotals;

            for (int i = 0; i < 3; i++)
            {             
                currentTotals = getCurrentTotals(i);    //This is you
                
                if (i == 1)//This is LEFT
                {
                    currentTotals = rm.tradableArray(you, 0);
                }
                if (i == 2)//This is RIGHT 
                {
                    currentTotals = rm.tradableArray(you, 1);
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
                return rc.getCenter();
            }
            else
            if (i == 1)
            {
                return rc.getLeft();
            }
            else
            {
                return rc.getRight();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        //This button click handler finds which button you pressed and then modifies the appropriate 
        //Player resources as well calling methods for removing and adding money
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

            Console.WriteLine("Button Number: " + index);
            
            if(index > 0 && index < 8)
            {

                this.Hide();

                rm.usingSpecialResource(you, index-1);                
                
                //rm.resetSpecialResources(you);

                rc = rm.GetCombinedResources(you);

                ResourceBuying window = ResourceBuying.GetInstance(rc, card, board);                
                
                if (rm.ValidateCard(you, card))//ok check if you can play the card now.
                {                    
                    //window.Hide();
                    you.addPlayedCards(card);//add to the playerState
                    board.changeCard(card);//adds to the board cards                    
                    you.setPlayedACard();//set true
                    you.getHand().Remove(card);//remove from cards in hand of player
                    rm.getGameState().incrementTurn();

                    //refresh the view
                    PlayerGameBoard play = new PlayerGameBoard(board.getGrid(), you, rm.getGameState());
                }
                else
                {                    
                    window.Show();
                }
                Console.WriteLine("Player: " + you.getName() + " Calling Special: index currently: " + (index-1));
            }
        }


        public static ResourceBuying GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ResourceBuying(null, null, null);
            }
            return _instance;
        }

        public static ResourceBuying GetInstance(Resources r, Card c, PlayerGameBoard p)
        {
            if (_instance == null)
            {
                _instance = new ResourceBuying(r, c, p);
            }
            else
            {
                _instance.Close();
                _instance = new ResourceBuying(r, c, p);
            }
            return _instance;
        }

        public void reset()
        {
            _instance = null;
        }
        
    }
}
