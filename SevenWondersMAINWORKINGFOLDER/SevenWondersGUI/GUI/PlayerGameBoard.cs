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
    public class PlayerGameBoard
    {
        Grid parent;
        Canvas playBoard;
        GameState game;
        PlayerState player; //= new PlayerState("Player", 0);
        //List<Card> hand = new List<Card>();
        int g_index = 0;//needed to reset ZIndex to original value upon mouse events

        public PlayerGameBoard(Grid gameGrid, PlayerState p, GameState g)
        {
            player = p;
            parent = gameGrid;//only 1 child
            if (parent.Children.Count > 1)//handle the case where there is existing child when we change views
            {
                parent.Children.Remove(parent.Children[1]);
                //System.Console.WriteLine("Child Removed");
            }
            game = g;
            if (game.getAge() == 4)//game is over
            {
                ScoreWindow s = new ScoreWindow(game, parent);
                s.Show(); ;
            }

            playBoard = new Canvas();
            parent.Children.Add(playBoard);//now child[1], two children

            createCardStack("R", 179, 10, playBoard);//0-9
            createCardStack("M", 336, 10, playBoard);//10-19
            createCardStack("S", 493, 10, playBoard);//20-29
            createCardStack("Civ", 650, 10, playBoard);//30-39
            createCardStack("Com", 807, 10, playBoard);//40-49
            createCardStack("G", 964, 6, playBoard);//50-55
            createHandStack();//56-62
            updateAge(game.getAge());//63
            createWonderBoard(player.getBoard().getName());//64
            createPlayerButtons();//65-71
            createPlayerRedCircles();//72-78
            createPlayerBlueCircles();//79-85
            createMoveButtons();//86-89
            updateMoney();//90
            createWonderSpots(player.getBoard().getMaxWonderLevel());//[91-94] variable length
            createRulesButton();//variable base on above wonder
            updateWonderSpots(player.getWonderCards());
            updateHand(player.getHand());
            updateCards(player.getPlayedCards());            
        }

//=================================================================================================================
//Methods for handling mouse events on our board
        private void cardChangeSmall_MouseEnter(object sender, EventArgs e)
        {
            Image img = ((Image)sender);
            DropShadowEffect cardGlow = new DropShadowEffect();
            Color c = new Color();
            c.ScA = (float)0.5;
            c.ScB = 1;
            c.ScG = (float)0.5;
            c.ScR = (float)0.4;

            cardGlow.Color = c;
            cardGlow.Direction = 90;
            img.Effect = cardGlow;
            g_index = Canvas.GetZIndex(img);

            Canvas.SetZIndex(img, 20);//make sure its on top while its in focus
        }

//mouse leave for normal cards
        private void cardChange_MouseLeave(object sender, EventArgs e)
        {
            Image img = ((Image)sender);
            img.Effect = null;
            img.Height = 150;
            img.Width = 100;
            Canvas.SetZIndex(img, g_index);
        }

//mouse click for normal cards
        private void cardChangeBig_MouseLeftButtonDown(object sender, EventArgs r)
        {
            Image img = ((Image)sender);
            img.Effect = null;
            img.Height *= 2.2;
            img.Width *= 2.2;
        }
        
//normal cards left mouse button up
        private void cardReset_MouseLeftButtonUp(object sender, EventArgs e)
        {
            Image img = ((Image)sender);
            img.Effect = null;
            img.Height = 150;
            img.Width = 100;            
        }

//Mouse click for wonder board cards        
        private void boardCardChange_MouseLeftButtonDown(object sender, EventArgs r)
        {
            Canvas playerCan = (Canvas)parent.Children[1];
            Image img = ((Image)sender);
            
            for (int i = 0; i < 7; i++)
            {
                Image card = (Image)playerCan.Children[i + 56];
                if (card.ActualHeight > 150)
                {
                    card.Height = 150;
                    card.Width = 100;
                    hideMoveButtons();
                }
            }

            img.Effect = null;
            
            if (img.ActualHeight > 150)
            {
                img.Height = 150;
                img.Width = 100;
                
                hideMoveButtons();
            }
            else
            {
                img.Height *= 2.1;
                img.Width *= 2.1;
                showMoveButtons();              
            }
        }
        
 //Mouse Leave for wonderBoard Cards
        private void boardCardChange_MouseLeave(object sender, EventArgs e)
        {
            Image img = ((Image)sender);
            Canvas.SetZIndex(img, g_index);
            img.Effect = null;
        }

// Mouse Click on player Buttons 72-78 Red 79-85 blue
        private void playerButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string p1 = "P1";
            string p2 = "P2";
            string p3 = "P3";
            string p4 = "P4";
            string p5 = "P5";
            string p6 = "P6";
            string p7 = "P7";

            makeCirclesBlack();

            if (b.Name.Equals(p1))
            {
                Ellipse el = (Ellipse)playBoard.Children[72];
                el.Fill = new SolidColorBrush(Colors.Red);
                PlayerGameBoard g = new PlayerGameBoard(parent, game.getPlayerNum(0), game);
            }
            else if(b.Name.Equals(p2))
            {
                Ellipse el = (Ellipse)playBoard.Children[73];
                el.Fill = new SolidColorBrush(Colors.Red);
                PlayerGameBoard g = new PlayerGameBoard(parent, game.getPlayerNum(1), game);
            }
            else if (b.Name.Equals(p3))
            {
                Ellipse el = (Ellipse)playBoard.Children[74];
                el.Fill = new SolidColorBrush(Colors.Red);
                PlayerGameBoard g = new PlayerGameBoard(parent, game.getPlayerNum(2), game);
            }
            else if (b.Name.Equals(p4))
            {
                Ellipse el = (Ellipse)playBoard.Children[75];
                el.Fill = new SolidColorBrush(Colors.Red);
                PlayerGameBoard g = new PlayerGameBoard(parent, game.getPlayerNum(3), game);
            }
            else if (b.Name.Equals(p5))
            {
                Ellipse el = (Ellipse)playBoard.Children[76];
                el.Fill = new SolidColorBrush(Colors.Red);
                PlayerGameBoard g = new PlayerGameBoard(parent, game.getPlayerNum(4), game);
            }
            else if (b.Name.Equals(p6))
            {
                Ellipse el = (Ellipse)playBoard.Children[77];
                el.Fill = new SolidColorBrush(Colors.Red);
                PlayerGameBoard g = new PlayerGameBoard(parent, game.getPlayerNum(5), game);
            }
            else if (b.Name.Equals(p7))
            {
                Ellipse el = (Ellipse)playBoard.Children[78];
                el.Fill = new SolidColorBrush(Colors.Red);
                PlayerGameBoard g = new PlayerGameBoard(parent, game.getPlayerNum(6), game);
            }
        }

//Mouse Click for "Play Card" button
        private void playerMoveButton_Click(object sender, EventArgs e)
        {
            if (player.getPlayedACard() == false)
            {               
                int numCards = player.getHand().Count;

                for (int i = 0; i < player.getHand().Count; i++)
                {
                    Card c = player.getHand()[i];
                    Image img = (Image)playBoard.Children[(56 + i)];

                    if (img.ActualHeight > 150)//this is the selected card
                    {
                        if (img.Source != new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute)))
                        {
                            ResourceManager resourceManager = ResourceManager.GetInstance(game);

                            if (resourceManager.ValidateCard(player, c))//already has enough resources
                            {
                                player.addPlayedCards(c);//add to the playerState
                                changeCard(c);//adds to the board cards                    
                                player.setPlayedACard();//set true
                                player.getHand().Remove(c);//remove from cards in hand of player
                                game.incrementTurn();
                                img.Height = 150;
                                img.Width = 100;
                                img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute));
                                PlayerGameBoard play = new PlayerGameBoard(parent, player, game);
                            }
                            else
                            {
                                ResourceManager manager = ResourceManager.GetInstance(game);
                                manager.resetResources(player);

                                ResourceBuying window = new ResourceBuying(ResourceManager.GetInstance().GetCombinedResources(player), c, this, game);
                                window.Show();
                            }
                        }
                    }                    
                }
            }
        }

//Mouse Click for "Sell Card" Button
        private void sellCardButton_Click(object sender, EventArgs e)
        {
            if (player.getPlayedACard() == false)
            {
                for (int i = 0; i < player.getHand().Count; i++)
                {
                    Card c = player.getHand()[i];
                    Image img = (Image)playBoard.Children[(56 + i)];//

                    if (img.ActualHeight > 150)
                    {
                        if (img.Source != new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute)))
                        {
                            player.getHand().Remove(c);//remove from cards in hand of player                            
                            player.updateCoins(3);
                            player.setPlayedACard();
                            game.incrementTurn();
                            game.setDiscards(c);//add to discard pile
                            img.Height = 150;
                            img.Width = 100;
                            img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute));
                            PlayerGameBoard play = new PlayerGameBoard(parent, player, game);
                        }
                        //img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute));
                        
                       // return;
                    }

                }
            }
        }

//Mouse Click for "Add to Wonder" Button
        private void wonderCardButton_Click(object sender, EventArgs e) 
        {
            if (player.getPlayedACard() == false && (player.getBoard().getMaxWonderLevel() >= player.getWonderCards().Count))
            {
                for (int i = 0; i < player.getHand().Count; i++)
                {
                    Card c = player.getHand()[i];
                    Image img = (Image)playBoard.Children[(56 + i)];

                    if (img.ActualHeight > 150)
                    {
                        if (img.Source != new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute)))
                        {
                            ResourceManager resourceManager = ResourceManager.GetInstance(game);

                            if (resourceManager.ValidateWonder(player))
                            {
                                player.getHand().Remove(c);//remove from cards in hand of player
                                player.setPlayedACard();//set true
                                game.incrementTurn();
                                player.setWonderCards(c);
                                player.getBoard().incrementWonderLevel(player);
                                img.Height = 150;
                                img.Width = 100;
                                img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute));

                                Image imgl = (Image)playBoard.Children[91];
                                PlayerGameBoard play = new PlayerGameBoard(parent, player, game);
                            }
                            else
                            {
                                ResourceManager manager = ResourceManager.GetInstance(game);
                                manager.resetResources(player);

                                WonderBuyingWindow window = new WonderBuyingWindow(ResourceManager.GetInstance().GetCombinedResources(player), c, this, game);
                                window.Show();
                            }
                        }
                    }
                }
            }            
        }

        private void rules_Click(object sender, RoutedEventArgs e)
        {
            RulesWindow r = new RulesWindow();
            r.Show();
        }

     /*  //the number of wonders can be between 2-4 so children 90-93
        private void changeWonderImage(int age, int loopSize)
        {
            for (int i = 0; i < loopSize; i++)
            {
                Image img = (Image)playBoard.Children[90 + i];                
               // System.Console.WriteLine("Changed Age Image in loop");                

                if (img.Source == new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCardWonderBoard.png", UriKind.RelativeOrAbsolute)))
                {
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/Age" + age + ".png", UriKind.RelativeOrAbsolute));
                   // System.Console.WriteLine("Changed Age Image");
                }
            }
        }
        */
//=================================================================================================================
// This creates the places for all the cards on the playerCanvas
        private void createCardStack(String name, int l_left, int l_loop, Canvas c)
        {
            int left = l_left;
            int top = 316;
            int l_index = 11;//Zindex

            for (int i = 0; i < l_loop; i++)
            {
                top -= 30;
                Image card = new Image
                {
                    Width = 100,
                    Height = 150,
                };

                if (i == 0)
                {
                    card.Source = new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    card.Source = new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/darkPlaceholder.png", UriKind.RelativeOrAbsolute));
                }
                card.Name = name + i;
                //  this.RegisterName(card.Name, card);
                card.MouseLeftButtonUp += new MouseButtonEventHandler(cardReset_MouseLeftButtonUp);
                card.MouseLeftButtonDown += new MouseButtonEventHandler(cardChangeBig_MouseLeftButtonDown);
                card.MouseEnter += new MouseEventHandler(cardChangeSmall_MouseEnter);
                card.MouseLeave += new MouseEventHandler(cardChange_MouseLeave);
                Canvas.SetZIndex(card, l_index--);
                Canvas.SetTop(card, top);
                Canvas.SetLeft(card, left);
                c.Children.Add(card);//add it to the canvas            
            }
        }

//=================================================================================================================
// This creates the seven hand places on the wonder board
        private void createHandStack()
        {
            Canvas player = (Canvas)parent.Children[1];
            
            int left = 60;
            int top = 440;
            int l_index = 12;
            // Canvas boardCanvas = (Canvas)Player1Canvas.Children[0];
            for (int i = 0; i < 7; i++)
            {
                left += 130;
                
                Image card = new Image
                {
                    Width = 100,
                    Height = 150,
                };
                
                card.Source = new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute));
                card.Name = "H" + i;
                
                card.MouseLeftButtonDown += new MouseButtonEventHandler(boardCardChange_MouseLeftButtonDown);
                card.MouseEnter += new MouseEventHandler(cardChangeSmall_MouseEnter);
                card.MouseLeave += new MouseEventHandler(boardCardChange_MouseLeave);
                Canvas.SetZIndex(card, l_index);
                Canvas.SetTop(card, top);
                Canvas.SetLeft(card, left);
                
                player.Children.Add(card);
            }
        }

//=================================================================================================================
// Method to add the wonderboard to the player game canvas.
        private void createWonderBoard(String n)
        {
            Canvas g = (Canvas)parent.Children[1];           

            Canvas wonderB = new Canvas 
            {
                Height = 361,
                Width  = 1000,
                Background = new ImageBrush(new BitmapImage(
                            new Uri(@"pack://application:,,,/Images/" + n + ".png", UriKind.RelativeOrAbsolute))),                
            };

            Canvas.SetZIndex(wonderB, 11);
            Canvas.SetTop(wonderB, 361);
            Canvas.SetLeft(wonderB, 129);

            g.Children.Add(wonderB);
        }

//=================================================================================================================
// Method to add the age icon to canvas, takes in the current age and selects correct Icon to place    
        private void updateAge(int age)
        {

            if (age < 4)
            {
                Canvas g = (Canvas)parent.Children[1];

                Image ageIcon = new Image
                {
                    Width = 100,
                    Height = 100,
                    Source = new BitmapImage(
                            new Uri(@"pack://application:,,,/Images/ph" + age + ".png", UriKind.RelativeOrAbsolute))
                };
                ageIcon.Name = "Age" + age;
                Canvas.SetLeft(ageIcon, 10);
                Canvas.SetTop(ageIcon, 10);
                g.Children.Add(ageIcon);

            }
            else
            {
                Canvas g = (Canvas)parent.Children[1];

                Image ageIcon = new Image
                {
                    Width = 100,
                    Height = 100,
                    Source = new BitmapImage(
                            new Uri(@"pack://application:,,,/Images/gameover.png", UriKind.RelativeOrAbsolute))
                };
                ageIcon.Name = "Age" + age;
                Canvas.SetLeft(ageIcon, 10);
                Canvas.SetTop(ageIcon, 10);
                g.Children.Add(ageIcon);
            }
        }

//=================================================================================================================
// Method to modify the hand given a list of cards
        private void updateHand(List<Card> lst)
        {
            for (int i = 0; i < 7; i++)
            {
                if (i >= lst.Count)
                {
                    Image img = (Image)playBoard.Children[(56 + i)];
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCard.png", UriKind.RelativeOrAbsolute));
                    continue;
                }
                else
                {
                    Image img = (Image)playBoard.Children[(56 + i)];
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/" + lst[i].getName() + ".jpg", UriKind.RelativeOrAbsolute));
                }
            }            
        }
//
        private void updateWonderSpots(List<Card> cards)
        {
            for (int i = 0; i < player.getBoard().getMaxWonderLevel(); i++)
            {
                // System.Console.WriteLine("playboard Children count "+playBoard.Children.Count + " Max wonder board level: " + player.getBoard().getMaxWonderLevel()+ " i = " + i + " Child # " + (90+i) + " num cards sent in " + cards.Count);
                Image img = (Image)playBoard.Children[(91 + i)];

                if (i < cards.Count)
                {
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/Age" + cards[i].getAct() + "Wonder.png", UriKind.RelativeOrAbsolute));
                    Canvas.SetZIndex(img, 1);
                    continue;
                }
                else
                {
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCardWonderBoard.png", UriKind.RelativeOrAbsolute));
                }
            }
        }
//=================================================================================================================    
// This is a filtering method to determine where in the board the card should be added
        public void changeCard(Card c)
        {
            if (c.getName()[0].Equals('R')) { changeCardHelper(c, "R",  0,  10); }//3-12
            if (c.getName()[0].Equals('M')) { changeCardHelper(c, "M",  10, 10); }//13-22
            if (c.getName()[0].Equals('S')) { changeCardHelper(c, "S",  20, 10); }//23-32
            if (c.getName()[1].Equals('O')) { changeCardHelper(c, "CO", 30, 10); return; }//return to not continue
            if (c.getName()[0].Equals('C')) { changeCardHelper(c, "C",  40, 10); }//33-42            
            if (c.getName()[0].Equals('G')) { changeCardHelper(c, "G",  50, 6); }//53-57
        }

//=================================================================================================================
// Method to take the input card and place it in the next slot on the board
        private void changeCardHelper(Card c, String s, int startIndex, int lengthIndex)
        {
            Canvas g = (Canvas)parent.Children[1];

            for (int i = 0; i < lengthIndex; i++)
            {
                Image img = (Image)g.Children[startIndex++];
                String name = ((BitmapImage)img.Source).UriSource.ToString();//parse this to see if slot is empty
                String subs = name.Split(',').Last();
                String last = subs.Split('/').Last();
                
                //handle both cases of placeholder cards
                String empty = "EmptyCard.png";
                String dark = "darkPlaceholder.png";
                
                if (last.Equals(empty, StringComparison.Ordinal) || last.Equals(dark, StringComparison.Ordinal))
                {
                    img.BeginInit();
                    img.Source = new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/" + c.getName() + ".jpg", UriKind.RelativeOrAbsolute));
                    img.EndInit();
                    break;
                }
            }
        }

//=================================================================================================================
// Created buttons to select different player views during the game
        private void createPlayerButtons()
        {
            Canvas g = (Canvas)parent.Children[1];
            List<PlayerState> ps = game.getPlayers();

            int startLeft = 1200;
            int startTop = 20;
            int numP = ps.Count;

            for (int i = 0; i < 7; i++)
            {
                Button P1 = new Button();
                if (i < ps.Count)
                {
                    P1.Content = ps[i].getName();
                }
                else
                {
                    P1.Content = "P" + (i + 1);
                }
                P1.Name = "P" + (i + 1);
                P1.Width = 50;
                P1.Height = 30;
                P1.Click += new RoutedEventHandler(playerButton_Click);
                Canvas.SetTop(P1, startTop);
                Canvas.SetLeft(P1, startLeft);
                startTop += 30;
                if (i >= numP)
                {
                    P1.Visibility = Visibility.Hidden;
                }
                g.Children.Add(P1);
            }
        }
//creator of the rules button in lower right, shows the game rules
        private void createRulesButton()
        {
            Button b  = new Button();
            b.Content = "Rules";
            b.Width   = 100;
            b.Height  = 30;
            b.Background = null;
            //b.MouseEnter += null;
           // b.MouseLeave += null;
            b.Foreground = new SolidColorBrush(Colors.LightBlue);
            //b = null;
            b.Click+=new RoutedEventHandler(rules_Click);
            Canvas.SetLeft(b, 1145);
            Canvas.SetTop(b, 670);
            playBoard.Children.Add(b);
        }

//=================================================================================================================
// Create indicator circles next to player buttons RED = Currently Selected
        private void createPlayerRedCircles()
        {            
            Canvas g = (Canvas)parent.Children[1];

            int startLeft = 1175;
            int startTop = 25;
            int numP = game.getPlayers().Count;

            for (int i = 0; i < 7; i++)
            {
                Ellipse e = new Ellipse();
                e.Width  = 20;
                e.Height = 20;
                if (player.getSeatNumber() == i)
                {
                    e.Fill = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    e.Fill = new SolidColorBrush(Colors.Black);
                }
                Canvas.SetTop(e, startTop);
                Canvas.SetLeft(e, startLeft);

                if (i >= numP)
                {
                    e.Visibility = Visibility.Hidden;
                }

                g.Children.Add(e);
                startTop += 30;
            }
        }

//=================================================================================================================
// Create indicator circles next to player buttons Blue = This Player
        private void createPlayerBlueCircles()
        {
            Canvas g = (Canvas)parent.Children[1];

            int startLeft = 1145;
            int startTop = 25;
            int numP = game.getPlayers().Count;

            for (int i = 0; i < 7; i++)
            {
                Ellipse e = new Ellipse();
                e.Width = 20;
                e.Height = 20;
                e.Fill = new SolidColorBrush(Colors.RoyalBlue);
                Canvas.SetTop(e, startTop);
                Canvas.SetLeft(e, startLeft);

                if (i <= numP)
                {
                    e.Visibility = Visibility.Hidden;
                }

                g.Children.Add(e);

                startTop += 30;
            }
        }

//=================================================================================================================
        private void makeCirclesBlack()
        {
            for (int i = 0; i < 7; i++)
            {
                Ellipse e = (Ellipse)playBoard.Children[72 + i];
                e.Fill = new SolidColorBrush(Colors.Black);
            }
        }

//=================================================================================================================
//Create buttons for when player submits a move
        private void createMoveButtons()
        {
            int startLeft = 15;
            int startTop = 460;

            Button b1 = new Button();
            b1.Content = "Play Card";
            b1.Width = 100;
            b1.Height = 30;
            b1.Click += new RoutedEventHandler(playerMoveButton_Click);
            Canvas.SetTop(b1, startTop);
            Canvas.SetLeft(b1, startLeft);
            startTop += 35;
            b1.Visibility = Visibility.Hidden;
            playBoard.Children.Add(b1);

            Button b2 = new Button();
            b2.Content = "Sell Card";
            b2.Width = 100;
            b2.Height = 30;
            b2.Click += new RoutedEventHandler(sellCardButton_Click);
            Canvas.SetTop(b2, startTop);
            Canvas.SetLeft(b2, startLeft);
            startTop += 35;
            b2.Visibility = Visibility.Hidden;
            playBoard.Children.Add(b2);

            Button b3 = new Button();
            b3.Content = "Add To Wonder";
            b3.Width = 100;
            b3.Height = 30;
            b3.Click += new RoutedEventHandler(wonderCardButton_Click);
            Canvas.SetTop(b3, startTop);
            Canvas.SetLeft(b3, startLeft);
            startTop += 35;
            b3.Visibility = Visibility.Hidden;
            playBoard.Children.Add(b3);

            Button b4 = new Button();
            b4.Content = "Special Play";
            b4.Width = 100;
            b4.Height = 30;
            b4.Click += new RoutedEventHandler(wonderCardButton_Click);
            Canvas.SetTop(b4, startTop);
            Canvas.SetLeft(b4, startLeft);
            startTop += 35;
            b4.Visibility = Visibility.Hidden;
            playBoard.Children.Add(b4);            
            if (player.IsAIPlayer())
            {
                b1.IsEnabled = false;
                b2.IsEnabled = false;
                b3.IsEnabled = false;
            }            
        }
//Method to show/hide special play button if player has a board that supports it and has high enough wonder level
        private void showSpecialMoveButton()
        {
            Button b = (Button)playBoard.Children[89];
            b.Visibility = Visibility.Visible;
        }
        private void hideSpecialMoveButton()
        {
            Button b = (Button)playBoard.Children[89];
            b.Visibility = Visibility.Hidden;
        }
// Show the move buttons 86-89
        private void showMoveButtons()
        {
            for(int i =0; i < 3; i ++){
                Button b = (Button)playBoard.Children[86 + i];
                b.Visibility = Visibility.Visible;
            }
            //ok lets see if player even has a board with special features
            if (player.getBoard().getName().Equals("WB3") ||
               player.getBoard().getName().Equals("WB4" ) ||
               player.getBoard().getName().Equals("WB9" ) ||
               player.getBoard().getName().Equals("WB10") ||
               player.getBoard().getName().Equals("WB11") ||
               player.getBoard().getName().Equals("WB12"))
            {
                showSpecialMoveButton();
            }
        }

 // hide the move buttons 86-89
        private void hideMoveButtons()
        {
            for (int i = 0; i < 3; i++)
            {
                Button b = (Button)playBoard.Children[86 + i];
                b.Visibility = Visibility.Hidden;
            }
            hideSpecialMoveButton();
        }

// Method to dynamically create wonderboard slots based upon how many are in the board
        private void createWonderSpots(int num)
        {
            int left = 200;
            int top = 711;

            if (num == 2)
            {
                left += 300;
                for (int i = 0; i < num; i++)
                {                    
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCardWonderBoard.png", UriKind.RelativeOrAbsolute));
                    img.Width = 245;
                    img.Height = 50;

                    Canvas.SetLeft(img, left);
                    Canvas.SetTop(img, top);
                    Canvas.SetZIndex(img, 10);
                    playBoard.Children.Add(img);
                    left += 300;
                }
            }else
            if (num == 3)
            {
                for (int i = 0; i < num; i++)
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCardWonderBoard.png", UriKind.RelativeOrAbsolute));
                    img.Width = 245;
                    img.Height = 50;

                    Canvas.SetLeft(img, left);
                    Canvas.SetTop(img, top);
                    Canvas.SetZIndex(img, 10);
                    playBoard.Children.Add(img);
                    left += 300;
                }
            }
            else
            if (num == 4)
            {
                left -= 65;
                for (int i = 0; i < num; i++)
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/EmptyCardWonderBoard.png", UriKind.RelativeOrAbsolute));
                    img.Width = 245;
                    img.Height = 50;

                    Canvas.SetLeft(img, left);
                    Canvas.SetTop(img, top);
                    Canvas.SetZIndex(img, 10);
                    playBoard.Children.Add(img);
                    left += 250;
                }
            }

        }
// get the playboard
        public Canvas getPlayBoard()
        {
            return playBoard;
        }

        private void updateCards(List<Card> c)
        {
            foreach (Card card in c)
            {
                changeCard(card);
            }
        }

//the amount of money each player has
        private void updateMoney()
        {
            int money = player.getCoins();

            Label label = new Label();

            label.Content = " " + money;
            label.Height = 50;
            label.Width  = 50;
            label.FontSize = 30;
            label.Foreground = new SolidColorBrush(Colors.White);
            label.Background = new ImageBrush(new BitmapImage(
                            new Uri(@"pack://application:,,,/Images/geld3.png", UriKind.RelativeOrAbsolute)));
            Canvas.SetLeft(label, 35);
            Canvas.SetTop(label, 250);
            playBoard.Children.Add(label);

        }

        public Grid getGrid()
        {
            return parent;
        }

    }
}
