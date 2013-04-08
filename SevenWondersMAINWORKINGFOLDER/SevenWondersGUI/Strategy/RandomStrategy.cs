using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class RandomStrategy : GameStrategy
    {
        public static readonly int ID = 0;

        public RandomStrategy()
        {
            Console.WriteLine("created random");
        }

        public String Name()
        {
            return "Random Strategy Pattern";
        }

        public Card getNextCard(PlayerState p, List<Card> hand)
        {

            Random random = new Random();
            int randomNumber;
         
            Card c = null;
            if (ResourceManager.GetInstance().ValidateWonder(p))
            {
                randomNumber = random.Next(0, 2);
                Console.WriteLine("attempting to play wonder " + randomNumber);
                if (randomNumber == 1)
                {
                    Console.WriteLine("playing wonder " + randomNumber);
                    randomNumber = random.Next(0, hand.Count - 1);
                    c = hand[randomNumber];
                    p.setWonderCards(c);
                    p.getBoard().incrementWonderLevel(p);
                    
                }
            }

            List<int> availCards = new List<int>();
            for (int i = 0; i < hand.Count; i++)
            {
                availCards.Add(i);
            }

            int trade;
            //selects a card to play
            for (int i = 0; i < hand.Count; i++)
            {
                randomNumber = random.Next(0, availCards.Count - 1);
                c = hand[availCards[randomNumber]];
                if (ResourceManager.GetInstance().ValidateCard(p, c))
                {
                    p.addPlayedCards(c);
                    return c;
                    //Console.WriteLine("===================== CHECKED CARD ===========================");
                }
                trade = ResourceManager.GetInstance().validateTrade(p, c, 0);

                if ((trade > 0) && (p.getCoins() >= trade))
                {
                    //.WriteLine(p.getName() + " trading for " + trade);
                    p.updateCoins(-trade);
                    ResourceManager.GetInstance().getGameState().getRightPlayer(p).updateCoins(trade);
                    p.addPlayedCards(c);
                    return c;
                }
                trade = ResourceManager.GetInstance().validateTrade(p, c, 1);

                if ((trade > 0) && (p.getCoins() >= trade))
                {
                    //Console.WriteLine(p.getName() + " trading for " + trade);
                    p.updateCoins(-trade);
                    p.addPlayedCards(c);
                    ResourceManager.GetInstance().getGameState().getLeftPlayer(p).updateCoins(trade);
                    return c;
                }
                availCards.Remove(randomNumber);
                //Console.Write("RANDOM STRATEGY:: AGE :: ");
                //Console.WriteLine(ResourceManager.GetInstance().getGameState().getAge());

            }
            return null;
        }

        public Card discardCard(PlayerState p, List<Card> hand)
        {
            Random r = new Random();
            int i = r.Next(0, hand.Count);
            return hand[i];
        }

    }
}
