using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class AggressiveStrategy : GameStrategy
    {
        public static readonly int ID = 0;

        private GameStrategy strategy = null;
        public AggressiveStrategy()
        {
            strategy = new MilitaryStrategy();
        }

        public String Name()
        {
            return "Aggressive Strategy Pattern";
        }

        public Card getNextCard(PlayerState p, List<Card> hand)
        {
            strategy = new MilitaryStrategy();

            Card c = strategy.getNextCard(p, hand);
            if (c == null)
            {
                strategy = new ScienceStrategy();
                c = strategy.getNextCard(p,hand);
            }

            if (c == null)
            {
                strategy = new CommerceStrategy();
                c = strategy.getNextCard(p, hand);            
            }

            return c;
        }

        public Card discardCard(PlayerState p, List<Card> hand)
        {
            for (int i = 0; i < hand.Count(); i++)
            {
                if (hand[i].getType() == 3)
                    return hand[i];
            }

            for (int i = 0; i < hand.Count(); i++)
            {
                if (hand[i].getType() == 4)
                    return hand[i];
            }

            for (int i = 0; i < hand.Count(); i++)
            {
                if ( hand[i].getType() == 1
                ||   hand[i].getType() == 2)
                    return hand[i];
            }

            Random r = new Random();
            int index = r.Next(0, hand.Count);
            return hand[index];
        }

    }
}
