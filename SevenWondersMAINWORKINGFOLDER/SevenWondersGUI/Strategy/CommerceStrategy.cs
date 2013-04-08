using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class CommerceStrategy : GameStrategy
    {
        public static readonly int ID = 0;

        private CommonAiFunc ult;

        public CommerceStrategy()
        {
            ult = new CommonAiFunc(4);
        }

        public String Name()
        {
            return "Commerce Strategy Pattern";
        }

        public Card getNextCard(PlayerState p, List<Card> hand)
        {
            Card cur = ult.playPrimary(p, hand);
            if (cur != null)
            {
                return cur;
            }

            if(((p.getBoard().getName().Equals("WB1")) 
                || (p.getBoard().getName().Equals("WB1"))
                && (p.getBoard().getCurrentWonderLevel() == 2))){
                    cur = ult.buildWonder(p, hand);
                    if (cur != null)
                    {
                        return cur;
                    }

            }
                

            cur = ult.playSecondary(p, hand);
            if (cur != null)
            {
                return cur;
            }

            cur = ult.buildWonder(p, hand);
            if (cur != null)
            {
                return cur;
            }

            return ult.playAnyCard(p, hand);
        }

        public Card discardCard(PlayerState p, List<Card> hand)
        {
            Random r = new Random();
            int i = r.Next(0, hand.Count);
            return hand[i];
        }

    }
}
