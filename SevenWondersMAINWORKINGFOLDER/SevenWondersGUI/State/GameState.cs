using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
     public class GameState{

    	public List<PlayerState> players;

        private int turn;
    	private int	age;
        private List<Card> cards;
        private List<Card> age1Cards = new List<Card>();
        private List<Card> age2Cards = new List<Card>();
        private List<Card> age3Cards = new List<Card>();
        private List<Card> discards  = new List<Card>();
        private List<Board> boards;

    	public GameState(List<Card> lst, List<PlayerState> ps, List<Board> b)
        {
            cards   = lst;
            players = ps;
            boards  = b;
            turn    = 0;
    		age     = 1;

            makeAgeCardLists(cards);
            dealCards(age1Cards);
            assignBoards(boards);
    	}

        public PlayerState getLeftPlayer(PlayerState p)
        {
            if (p == null) { Console.WriteLine("Snap: PlayerState is null"); }
            
            if (p.getSeatNumber() == 0) 
            {
                return this.players[players.Count-1];//last player 
            }
            else
            {
                return this.players[(p.getSeatNumber()-1)];
            }
        }

        public PlayerState getRightPlayer(PlayerState p)
        {
            if (p.getSeatNumber() == (players.Count-1))
            {
                return this.players[0];//first player
            }
            else
            {
                return this.players[(p.getSeatNumber() + 1)];
            }
        }

        public int getAge(){ return age; }

        public void setAge(int n){  age = n;}

        public void setPlayers(List<PlayerState> p) { players = p; }

        public List<PlayerState> getPlayers() {return players; }

        public PlayerState getPlayerNum(int n)
        {
            foreach (PlayerState p in players)
            {
                if (p.getSeatNumber() == n)
                {
                    return p;
                }
            }
            return players[0];//something broke
        }
        
        public int getTurn(){ return turn;}

        public void setTurn(int n){ turn = n;}

        public void incrementTurn()
        {
            if (checkIfAllPlayed())
            {
                playAIPCard(); 
                resetAllPlayed();   //allow players to play next turn                
                passHands();
                turn += 1;
                if (turn == 6)
                {
                    //need a special board check here for case of being able to play extra card if wonderboard "WB4"
                    incrementAge();
                    turn = 0;
                }                
            }
        }

        // make sure all players have played a card
        private bool checkIfAllPlayed()
        {                       
            bool flag = true;
            for(int i =0; i < players.Count;i++)
            {
                if ( !players[i].IsAIPlayer() && players[i].getPlayedACard() == false )
                {
                    flag = false; // return false;   //stop at first false
                }
            }

            return flag;
        }

        private void resetAllPlayed()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].setPlayedACard();
            }
        }

        private void playAIPCard()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].IsAIPlayer())
                {
                    AIPlayer aip = (AIPlayer)players[i];
                    aip.playACard();
                }                    
            }
        }

        public void incrementAge()
        {
            removeLastCard();
            battle(age);//add the military points
            age += 1;
            
            
            if (age == 2)
            {
                dealCards(age2Cards);
                //TESTING
                //DiscardsWindow w = new DiscardsWindow();
               // w.Show();

                return;
            }
            if (age == 3)
            {
                dealCards(age3Cards);
                return;
            }
            else //game is over
            {
               // System.Console.WriteLine("--- >>> GameState:: incrementAge Calculating Scores");
                //printScores();test method delete 
                age = 4;
                return;
            }
        }

        private void printScores()
        {
            for (int i = 0; i < players.Count; i++)
            {
                List<int> scores = players[i].getScores(this);
                System.Console.WriteLine("--- >>> GameState:: {0} {1} {2} {3} {4} {5} {6} {7}"
                                        , players[i].getName()
                                        , scores[Calculator._MILITARY_SCORE]
                                        , scores[Calculator._TREASURY_SCORE]
                                        , scores[Calculator._WONDER_SCORE]
                                        , scores[Calculator._CIVILIAN_SCORE]
                                        , scores[Calculator._SCIENCE_SCORE]
                                        , scores[Calculator._COMMERCE_SCORE]
                                        , scores[Calculator._GUILD_SCORE]
                                        );
            }
        }

        // count how many players we have and deal the appropriate amount of cards
        // from the right stack of cards
        private void dealCards(List<Card> c)
        {
            Random rand = new Random();            
            List<Card> cards = new List<Card>();
            cards = c;

            //System.Console.WriteLine("GameState():: dealCards() {0} {1}", c.Count, cards.Count);
  
            int numCards = cards.Count;
            int r = rand.Next(0, (numCards - 1));
            
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    players[i].setHand(cards[r]);
                    cards.Remove(cards[r]);
                    numCards = cards.Count;
                    if (numCards == 0)
                    {
                        break;
                    }
                    else
                    {
                        r = rand.Next(0, (numCards - 1));
                    }
                }
            }
        }

        private void makeAgeCardLists(List<Card> c)
        {
            foreach (Card card in c)
            {
                if (card.getAct() == 1)
                {
                    age1Cards.Add(card);
                    
                }
                if (card.getAct() == 2)
                {
                    age2Cards.Add(card);
                }
                if(card.getAct() == 3)
                {
                    age3Cards.Add(card);
                }
            }
        }

        private void battle(int a)
        {
            int loss = -1;
            int win  =  1;
            PlayerState you;
            PlayerState left;
            PlayerState right;
            
            if (a == 1) { win = 1; }
            if (a == 2) { win = 3; }
            if (a == 3) { win = 5; }

            for(int i = 0; i < players.Count;i++)
            {
                you   = players[i]; 
                left  = getLeftPlayer(you);
                right = getRightPlayer(you);

                List<Card> yourCards   = you.getPlayedCards();
                List<Card>  leftCards  = left.getPlayedCards();
                List<Card>  rightCards = right.getPlayedCards();
                int yTotMilPower = militaryPower(yourCards);
                int lTotMilPower = militaryPower(leftCards);
                int rTotMilPower = militaryPower(rightCards);

                System.Console.WriteLine("Seat #" + you.getSeatNumber() + " You Are: " + you.getName() + " Your power: " + yTotMilPower + " " +left.getName()+ " LPow: " + lTotMilPower + " " +right.getName()+" RPow: " + rTotMilPower);

                if (yTotMilPower < lTotMilPower)
                {
                    you.setNumLosses();
                    you.setMilitaryPoints(loss);
                }
                if (yTotMilPower < rTotMilPower)
                {
                    you.setNumLosses();
                    you.setMilitaryPoints(loss);
                }
                if (yTotMilPower > lTotMilPower)
                {
                    you.setMilitaryPoints(win);
                }
                if (yTotMilPower > rTotMilPower)
                {
                    you.setMilitaryPoints(win);
                }
                System.Console.WriteLine("Player: " + you.getName() + " current Military points: " + you.getMilitaryPoints() + " wins are " + win
                    + " Losses " + you.getNumLosses());
            }
        }

        private int militaryPower(List<Card> lst)
        {
            int total = 0;

            foreach (Card c in lst)
            {
                if (c.getName()[0].Equals('M'))
                {                    
                    MilitaryCard m = (MilitaryCard)c;
                    System.Console.WriteLine("card value " + m.getValue() + " " + c.getName() );
                    total += m.getValue();
                }
            }
            return total;
        }
//=================================================================================================================
// This method randomly assigns a board to a player while also removing both the A and B sides of a board that has been dealt
        private void assignBoards(List<Board> b)
        {
            Random rand = new Random();
            int numB = boards.Count;
            int r = rand.Next(0, (numB-1));

            for(int i =0; i < players.Count;i++)
            {
                players[i].setBoard(boards[r]);

                if (r == (boards.Count - 1))
                {
                    boards.Remove(boards[r]);
                    boards.Remove(boards[r - 1]);
                    numB = boards.Count;
                    if(numB != 0){
                    r = rand.Next(0, (numB - 1));
                    }
                    continue;
                }
                if (r % 2 == 1)
                {
                    boards.Remove(boards[r]);
                    boards.Remove(boards[r - 1]);
                    continue;
                }
                else
                {
                    boards.Remove(boards[r + 1]);
                    boards.Remove(boards[r]);                   
                }
                
                numB = boards.Count;
                if(numB != 0){
                    r = rand.Next(0, (numB - 1));
                    }
            }
        }

        public List<Card> getDiscards()
        {
            return discards;
        }

        public void setDiscards(Card c)
        {
            discards.Add(c);
        }
//This method handles the last card when all players have finished their turn and one card is left
        private void removeLastCard()
        {
            foreach (PlayerState p in players)
            {
                Card c = p.getHand()[0];
                p.getHand().Remove(c);
                setDiscards(c);
            }
        }
        
//this method passes the hands at the end of each turn. It will switch directions for the second age
//Finally we can use a LinkedList Somewhere :)
        private void passHands()
        {
            LinkedList<List<Card>> allHands = new LinkedList<List<Card>>();

            foreach (PlayerState p in players)
            {
                allHands.AddLast(p.getHand());
            }

            if (age == 2)//rotate CCW
            {
                List<Card> first = allHands.First();
                allHands.RemoveFirst();
                allHands.AddLast(first);

                foreach (PlayerState p in players)
                {
                    p.switchHands(allHands.First());
                    allHands.RemoveFirst();
                }
            }
            else//rotate CW
            {
                List<Card> last = allHands.Last();
                allHands.RemoveLast();
                allHands.AddFirst(last);

                foreach (PlayerState p in players)
                {
                    p.switchHands(allHands.First());
                    allHands.RemoveFirst();
                }
            }
        }


    }
}
