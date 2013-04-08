using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class CommonAiFunc
    {
        private int strategy;

        public CommonAiFunc(int type)
        {
            strategy = type;
        }


        //please the primary card for the type baes AIs
        public Card playPrimary(PlayerState p, List<Card> hand)
        {
            int trade;
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].getType() == strategy)
                {
                    if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                    {
                        //System.Console.WriteLine("ScienceStrategy():: getNextCard({0})", hand[i]);
                        p.addPlayedCards(hand[i]);
                        return hand[i];
                    }

                    trade = ResourceManager.GetInstance().validateTrade(p, hand[i], 0);

                    if ((trade > 0) && (p.getCoins() >= trade))
                    {
                        //.WriteLine(p.getName() + " trading for " + trade);
                        p.updateCoins(-trade);
                        ResourceManager.GetInstance().getGameState().getRightPlayer(p).updateCoins(trade);
                        p.addPlayedCards(hand[i]);
                        return hand[i];
                    }
                    trade = ResourceManager.GetInstance().validateTrade(p, hand[i], 1);

                    if ((trade > 0) && (p.getCoins() >= trade))
                    {
                        //Console.WriteLine(p.getName() + " trading for " + trade);
                        p.updateCoins(-trade);
                        ResourceManager.GetInstance().getGameState().getLeftPlayer(p).updateCoins(trade);
                        p.addPlayedCards(hand[i]);
                        return hand[i];
                    }
                }
            }
            return null;
        }

        //plays a resource or commodity
        public Card playSecondary(PlayerState p, List<Card> hand)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if ((hand[i].getType() == 1) && (ResourceManager.GetInstance().ValidateCard(p, hand[i])))
                {
                    //System.Console.WriteLine("ScienceStrategy():: getNextCard({0})", hand[i]);
                    p.addPlayedCards(hand[i]);
                    return hand[i];
                }
            }

            for (int i = 0; i < hand.Count; i++)
            {
                if ((hand[i].getType() == 2) && (ResourceManager.GetInstance().ValidateCard(p, hand[i])))
                {
                    //System.Console.WriteLine("ScienceStrategy():: getNextCard({0})", hand[i]);
                    p.addPlayedCards(hand[i]);
                    return hand[i];
                }
            }
            return null;
        }

        //attempts to build a wonder
        public Card buildWonder(PlayerState p, List<Card> hand)
        {
            if (ResourceManager.GetInstance().ValidateWonder(p))
            {
                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i].getType() == strategy)
                    {
                        p.setWonderCards(hand[i]);
                        p.getBoard().incrementWonderLevel(p);
                        return hand[i];
                    }
                }
                return hand[0];
            }
            return null;
        }

        public Card playAnyCard(PlayerState p, List<Card> hand)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                {
                    //System.Console.WriteLine("ScienceStrategy():: getNextCard({0})", hand[i]);
                    p.addPlayedCards(hand[i]);
                    return hand[i];
                }
            }
            return null;
        }
    }
}
