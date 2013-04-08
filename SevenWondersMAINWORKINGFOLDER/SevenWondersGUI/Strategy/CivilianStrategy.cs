using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class CivilianStrategy : GameStrategy
    {
        public static readonly int ID = 0;

        private CommonAiFunc ult;

        public CivilianStrategy()
        {
            ult = new CommonAiFunc(4);
        }

        public String Name()
        {
            return "Civilian Strategy Pattern";
        }

        public Card getNextCard(PlayerState p, List<Card> hand)
        {
            List<CivilianCard> civCards = new List<CivilianCard>();
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].getType() == 3)
                {
                    System.Console.WriteLine("CivilianStrategy():: getNextCard({0})", hand[i]);
                    civCards.Add((CivilianCard)hand[i]);
                }
            }

            // && (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
            int trade;
            while (civCards.Count != 0)
            {
                CivilianCard bestCard = civCards[0];

                for (int i = 1; i < civCards.Count; i++)
                {
                    if (bestCard.getVictoryPoints() < civCards[i].getVictoryPoints())
                    {
                        bestCard = civCards[i];
                    }
                }
                if (ResourceManager.GetInstance().ValidateCard(p, bestCard))
                {
                    p.addPlayedCards(bestCard);
                    return bestCard;
                }

                trade = ResourceManager.GetInstance().validateTrade(p, bestCard, 0);

                if ((trade > 0) && (p.getCoins() >= trade))
                {
                    //.WriteLine(p.getName() + " trading for " + trade);
                    p.updateCoins(-trade);
                    ResourceManager.GetInstance().getGameState().getRightPlayer(p).updateCoins(trade);
                    p.addPlayedCards(bestCard);
                    return bestCard;
                }

                trade = ResourceManager.GetInstance().validateTrade(p, bestCard, 1);

                if ((trade > 0) && (p.getCoins() >= trade))
                {
                    //Console.WriteLine(p.getName() + " trading for " + trade);
                    p.updateCoins(-trade);
                    p.addPlayedCards(bestCard);
                    ResourceManager.GetInstance().getGameState().getLeftPlayer(p).updateCoins(trade);
                    return bestCard;
                }
                civCards.Remove(bestCard);
            }

            Card cur = ult.playSecondary(p, hand);
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
