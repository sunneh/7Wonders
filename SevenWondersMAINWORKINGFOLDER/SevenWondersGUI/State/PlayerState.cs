using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class PlayerState
    {
        private string  name;

        protected Board gameBoard;

        protected List<Card> hand;
        protected List<Card> playedCards;
        protected List<Card> wonderBuildCards;

        private List<int> scores;   //8 ints representing 7 scores and total
        private int[] resources;    //the values for each of seven resources
        
        private int coins;
        private int militaryPower;
        private int militaryPoints;
        private int seatNumber;
        private int numLosses = 0; //How many times lost in battle
        
        private bool playedACard;

        public PlayerState(string n, int s) {
            name = n;
            seatNumber = s;
            playedCards = new List<Card>();
            hand = new List<Card>();
            wonderBuildCards = new List<Card>();
            gameBoard = null;
            coins = 3;
            militaryPoints = 0;
            militaryPower = 0;
            playedACard = false;
        }

        public virtual bool IsAIPlayer() { return false; }

        public string getName() { return name; }

        public int getSeatNumber() { return seatNumber; }

        public void setSeatNumber(int n) { seatNumber = n; }

        public int getNumLosses() { return numLosses; }

        public void setNumLosses() { numLosses += 1; }

        public List<Card> getPlayedCards() { 
            return playedCards; 
        }

        public void addPlayedCards(Card c ) {
            playedCards.Add(c);
        }

        public void setMilitaryPoints(int i) {militaryPoints = (militaryPoints + i) ;}
        
        public int[] getPlayedCardNumbers() {
            int[] temp = new int[playedCards.Count];
            for(int i = 0; i< playedCards.Count; i++){
                temp[i] = playedCards[i].getNumber();
            }
            return temp; // I think this pops, might need to pass in a reference.
        }

        public void setResources(int[] r)
        {
            resources = r;
        }

        public int[] getResources()
        {
            return resources;
        }

        public List<Card> getWonderCards() { 
            return wonderBuildCards;
        }

        public void setWonderCards(Card c) { 
             wonderBuildCards.Add(c);
        }

        public bool getPlayedACard() { return playedACard; }

        public void setPlayedACard()
        {
            if (playedACard == false)
            {
                playedACard = true;
            }
            else
            {
                playedACard = false;
            }
        }

        public List<Card> getHand() { 
            return hand; 
        }

        public void setHand(Card c){ hand.Add(c);}

        public void switchHands(List<Card> lst)
        {
            hand = lst;
        }

        public Board getBoard() { 
            return gameBoard; 
        }

        public void setBoard(Board b) { 
            gameBoard = b;
        }

        public int getCoins(){ return coins; }

        public void updateCoins(int n) { coins = coins + n; }

        public int getMilitaryPoints() { return militaryPoints; }

        public void updateMilitaryPoints(int n) 
        {
            militaryPoints = militaryPoints + n;
        }

        public int getMilitaryPower() { return militaryPower; }

        public void updateMilitaryPower(int n) {
            militaryPower = militaryPower + n; 
        }

        public List<int> getScores(GameState g)
        {
            scores = Calculator.getInstance().getScores(this, g); 
            return this.scores;
        }

        public int countCardType(int cType)
        {
            int count = 0;
            foreach (Card cCard in this.getPlayedCards())
            {
                if (cCard.getType() == cType)
                {
                    count++;
                }
            }
            return count;
        }

        public String id() { return name +"("+(seatNumber+1)+")"; }
    }
}
