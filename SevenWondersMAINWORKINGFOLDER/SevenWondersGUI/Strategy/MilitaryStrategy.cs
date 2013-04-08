using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class MilitaryStrategy : GameStrategy
    {
        public static readonly int ID = 0;

        private CommonAiFunc ult;

        public MilitaryStrategy()
        {
            ult = new CommonAiFunc(5);
        }

        public String Name()
        {
            return "Military Strategy Pattern";
        }

        public Card getNextCard(PlayerState p, List<Card> hand)
        {
            PlayerState left = ResourceManager.GetInstance().getGameState().getLeftPlayer(p);
            PlayerState right = ResourceManager.GetInstance().getGameState().getRightPlayer(p);
            int age = ResourceManager.GetInstance().getGameState().getAge();

            Card cur;

            //Checks to see if AI has enough MilitaryPower to win the
            //the battles at the end of the Age
            if ((left.getMilitaryPower() >= p.getMilitaryPower() - age) ||
                (right.getMilitaryPower() >= p.getMilitaryPower() - age))
            {
                cur = ult.playPrimary(p, hand);
                if (cur != null)
                {
                    return cur;
                }
            }

            //attempts to play wonder to increase military
            if ((p.getBoard().getName().Equals("WB7"))
                && (p.getBoard().getCurrentWonderLevel() < 2)
                && (ResourceManager.GetInstance().ValidateWonder(p)))
            {
                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i].getType() != 6)
                    {
                        p.setWonderCards(hand[i]);
                        p.getBoard().incrementWonderLevel(p);
                        return hand[i];
                    }
                }
            }

            if ((p.getBoard().getName().Equals("WB8"))
                && (p.getBoard().getCurrentWonderLevel() < 2)
                && (ResourceManager.GetInstance().ValidateWonder(p)))
            {
                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i].getType() != 6)
                    {
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
