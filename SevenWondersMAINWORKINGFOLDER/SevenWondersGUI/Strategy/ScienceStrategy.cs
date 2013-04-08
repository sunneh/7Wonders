using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class ScienceStrategy : GameStrategy
    {
        public static readonly int ID = 0;

        private CommonAiFunc ult;

        public ScienceStrategy()
        {
            ult = new CommonAiFunc(6);
        }

        public String Name()
        {
            return "Science Strategy Pattern";
        }

        public Card getNextCard(PlayerState p, List<Card> hand)
        {
            Card cur = ult.playPrimary(p, hand);
            if (cur != null)
            {
                return cur;
            }

            //attempts to play wonder to increase science
            if((p.getBoard().getName().Equals("WB3"))
                && (p.getBoard().getCurrentWonderLevel() < 2)
                && (ResourceManager.GetInstance().ValidateWonder(p))){
                for (int i = 0; i < hand.Count; i++){
                    if(hand[i].getType() != 6){
                        p.setWonderCards(hand[i]);
                        p.getBoard().incrementWonderLevel(p);
                        return hand[i];
                    }
                }
            }

            if((p.getBoard().getName().Equals("WB4"))
                && (p.getBoard().getCurrentWonderLevel() < 3)
                && (ResourceManager.GetInstance().ValidateWonder(p))){
                for (int i = 0; i < hand.Count; i++){
                    if(hand[i].getType() != 6){
                        p.setWonderCards(hand[i]);
                        p.getBoard().incrementWonderLevel(p);
                        return hand[i];
                    }
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
