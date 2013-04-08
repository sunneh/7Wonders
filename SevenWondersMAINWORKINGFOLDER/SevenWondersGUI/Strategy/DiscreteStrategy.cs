using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class DiscreteStrategy : GameStrategy
    {
        public static readonly int ID = 0;
        
        private GameStrategy strategy = null;
        public DiscreteStrategy()
        {
        }

        public String Name()
        {
            return "Discrete Strategy Pattern";
        }

        public Card getNextCard(PlayerState p, List<Card> hand)
        {
            strategy = new CivilianStrategy();

            Card c = strategy.getNextCard(p, hand);
            if (c == null)
            {
                strategy = new CommerceStrategy();
                c = strategy.getNextCard(p,hand);
            }

            if (c == null)
            {
                strategy = new ScienceStrategy();
                c = strategy.getNextCard(p, hand);            
            }

            return c;
        }

        public Card discardCard(PlayerState p, List<Card> hand)
        {
            for (int i = 0; i < hand.Count(); i++)
            {
                if (hand[i].getType() == 5)
                    return hand[i];
            }

            for (int i = 0; i < hand.Count(); i++)
            {
                if (hand[i].getType() == 6)
                    return hand[i];
            }

            for (int i = 0; i < hand.Count(); i++)
            {
                if ( hand[i].getType() == 3)
                    return hand[i];
            }

            Random r = new Random();
            int index = r.Next(0, hand.Count);
            return hand[index];
        }
    
    }

}
