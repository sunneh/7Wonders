using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class AdaptiveStrategy : GameStrategy
    {
        public static readonly int ID = 0;
        private int noOfPlayers;

        private GameStrategy strategy = null;
        private CommonAiFunc utl;

        public AdaptiveStrategy()
        {            
            utl = new CommonAiFunc(1);
        }

        public String Name()
        {
            return "Adaptive Strategy Pattern";
        }

        public Card getNextCard(PlayerState p, List<Card> hand)
        {
            noOfPlayers = ResourceManager.GetInstance().getGameState().getPlayers().Count(); 
            Card c = SetStrategy(p, hand);
            Console.WriteLine("[{0}] [{1}]", "Adaptive Strategy", strategy.Name());
            return c;
        }

        private Card SetStrategy(PlayerState p, List<Card> hand)
        {
            Card bestCard = null;
            int currentAge = ResourceManager.GetInstance().getGameState().getAge();

            if (p.getPlayedCards().Count() == 0)
                bestCard = BuildWonder(p, hand);

            if (bestCard == null)
                if (noOfPlayers == 3 || noOfPlayers == 7)
                    if (p.getBoard().getCurrentWonderLevel() != currentAge)
                        bestCard = BuildWonder(p, hand);

            if (bestCard == null)
                SetMilitaryStrategy(p, hand);

            if (bestCard == null)
                bestCard = SetScienceStrategy(p, hand);

            if (bestCard == null)
                bestCard = SetCommerceStrategy(p, hand);

            if (bestCard == null)
                bestCard = SetCivilianStrategy(p, hand);

            if (bestCard == null)
            {
                strategy = new RandomStrategy();
                bestCard = strategy.getNextCard(p, hand);
            }

            return bestCard;
        }

        private Card SetScienceStrategy(PlayerState p, List<Card> hand)
        {
            if (noOfPlayers == 5 || noOfPlayers == 7)
                if (hand.Count() > 3)
                {
                    strategy = new ScienceStrategy();
                    return strategy.getNextCard(p, hand);
                }
            return null;
        }

        private Card SetCivilianStrategy(PlayerState p, List<Card> hand)
        {

            for (int i = 0; i < hand.Count(); i++)
            {
                if (noOfPlayers == 3 || noOfPlayers == 7)
                    if (hand[i].getName() == "C22"
                    ||  hand[i].getName() == "C23"
                    ||  hand[i].getName() == "C63"
                    ||  hand[i].getName() == "C64")
                        if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                        {
                            strategy = new CivilianStrategy();
                            p.addPlayedCards(hand[i]);
                            return hand[i];
                        }

                if (noOfPlayers == 3 || noOfPlayers == 6)
                    if (hand[i].getName() == "C24"
                    ||  hand[i].getName() == "C25"
                    ||  hand[i].getName() == "C108"
                    ||  hand[i].getName() == "C109")
                        if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                        {
                            strategy = new CivilianStrategy();
                            p.addPlayedCards(hand[i]);
                            return hand[i];
                        }
            }

            int vc = 0, index = 0;
            for (int i = 0; i < hand.Count(); i++)
            {
                if (hand[i] is CivilianCard)
                {
                    CivilianCard c = (CivilianCard)hand[i];
                    if (c.getVictoryPoints() > vc)
                    {
                        vc = c.getVictoryPoints();
                        index = i;
                    }
                }
            }

            if (index != 0)
                if (ResourceManager.GetInstance().ValidateCard(p, hand[index]))
                {
                    strategy = new CivilianStrategy();
                    p.addPlayedCards(hand[index]);
                    return hand[index];
                }

            return null;
        }

        private Card SetCommerceStrategy(PlayerState p, List<Card> hand)
        {
            for (int i = 0; i < hand.Count(); i++)
            {
                if (hand[i].getName() == "C0123"
                ||  hand[i].getName() == "C0124")
                    if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                    {
                        strategy = new CommerceStrategy();
                        p.addPlayedCards(hand[i]);
                        return hand[i];
                    }

                if (noOfPlayers == 3)
                {
                    if (hand[i].getName() == "C031"
                    || hand[i].getName() == "CO33"
                    || hand[i].getName() == "CO71"
                    || hand[i].getName() == "CO74"
                    || hand[i].getName() == "CO77"
                    || hand[i].getName() == "CO121"
                    || hand[i].getName() == "CO125")
                        if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                        {
                            strategy = new CommerceStrategy();
                            p.addPlayedCards(hand[i]);
                            return hand[i];
                        }
                }

                if (noOfPlayers == 4)
                {
                    if (hand[i].getName() == "C028"
                    || hand[i].getName() == "CO79"
                    || hand[i].getName() == "C0120")
                        if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                        {
                            strategy = new CommerceStrategy();
                            p.addPlayedCards(hand[i]);
                            return hand[i];
                        }
                }

                if (noOfPlayers == 5)
                {
                    if (hand[i].getName() == "C029"
                     || hand[i].getName() == "CO75"
                     || hand[i].getName() == "C0126")
                        if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                        {
                            strategy = new CommerceStrategy();
                            p.addPlayedCards(hand[i]);
                            return hand[i];
                        }
                }

                if (noOfPlayers == 6)
                {
                    if (hand[i].getName() == "C036"
                     || hand[i].getName() == "CO72"
                     || hand[i].getName() == "C076"
                     || hand[i].getName() == "CO78"
                     || hand[i].getName() == "C0122"
                     || hand[i].getName() == "C0124")
                        if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                        {
                            strategy = new CommerceStrategy();
                            p.addPlayedCards(hand[i]);
                            return hand[i];
                        }
                }

                if (noOfPlayers == 7)
                {
                    if (hand[i].getName() == "C030"
                    || hand[i].getName() == "C032"
                    || hand[i].getName() == "C034"
                    || hand[i].getName() == "C032"
                    || hand[i].getName() == "C073"
                    || hand[i].getName() == "C080"
                    || hand[i].getName() == "C0127")
                        if (ResourceManager.GetInstance().ValidateCard(p, hand[i]))
                        {
                            strategy = new CommerceStrategy();
                            p.addPlayedCards(hand[i]);
                            return hand[i];
                        }
                }

                if (TradeCard(p, hand[i]))
                    return hand[i];

            }
            return null;
        }

        private Card SetMilitaryStrategy(PlayerState p, List<Card> hand)
        {
            int currentAge = ResourceManager.GetInstance().getGameState().getAge();
            PlayerState left = ResourceManager.GetInstance().getGameState().getLeftPlayer(p);
            PlayerState right = ResourceManager.GetInstance().getGameState().getRightPlayer(p);

            if ((left.getMilitaryPower() >= p.getMilitaryPower() - currentAge)
            || (right.getMilitaryPower() >= p.getMilitaryPower() - currentAge))
            {
                strategy = new MilitaryStrategy();
                Card c = strategy.getNextCard(p, hand);
                return c;
            }
            return null;
        }

        private Card BuildWonder(PlayerState p, List<Card> hand)
        {
            if (p.getBoard().notMaxYet())
            {
                for (int i = 0; i < hand.Count(); i++)
                {
                    if (hand[i].getType() == 1 || hand[i].getType() == 2)
                    {
                        if (ResourceManager.GetInstance().ValidateWonder(p))
                        {
                            strategy = new CommerceStrategy();
                            p.setWonderCards(hand[i]);
                            p.getBoard().incrementWonderLevel(p);
                            return hand[i];
                        }
                    }
                }
            }
            return null;
        }

        private bool TradeCard(PlayerState p, Card c)
        {
            int trade = ResourceManager.GetInstance().validateTrade(p, c, 0);

            if ((trade > 0) && (p.getCoins() >= trade))
            {
                p.updateCoins(-trade);
                ResourceManager.GetInstance().getGameState().getRightPlayer(p).updateCoins(trade);
                p.addPlayedCards(c);
                return true;
            }
            trade = ResourceManager.GetInstance().validateTrade(p, c, 1);

            if ((trade > 0) && (p.getCoins() >= trade))
            {
                p.updateCoins(-trade);
                ResourceManager.GetInstance().getGameState().getLeftPlayer(p).updateCoins(trade);
                p.addPlayedCards(c);
                return true;
            }
            return false;
        }

        public Card discardCard(PlayerState p, List<Card> hand)
        {
            bool jackEmUp = false;
            int currentAge = ResourceManager.GetInstance().getGameState().getAge();
            List<PlayerState> players = ResourceManager.GetInstance().getGameState().getPlayers();

            for (int i = 0; i < players.Count(); i++)
            {
                if ((players[i].getBoard().getName() == "WB3")
                || (players[i].getBoard().getName() == "WB7")
                || (players[i].getBoard().getName() == "WB8")
                || (players[i].getBoard().getName() == "WB13")
                || (players[i].getBoard().getName() == "WB14"))
                    jackEmUp = true;
            }

            if (jackEmUp && currentAge == 1)
                for (int i = 0; i < hand.Count(); i++)
                {
                    if (hand[i].getName() == "R6" || hand[i].getName() == "R10")
                    {
                        return hand[i];
                    }
                }

            if (jackEmUp && currentAge == 2)
                for (int i = 0; i < hand.Count(); i++)
                {
                    if (hand[i].getName() == "R55")
                    {
                        return hand[i];
                    }
                }

            Random _r = new Random();

            var r = new Random();
            int index = r.Next(0, hand.Count());
            return hand[index];

        }

    }

}
