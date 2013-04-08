using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class KensUtilityfunctions
    {
        //Calculates the coins increased for a commerce card played
        //and returns the total coin increased
        public class kensUtilityFunction {}

        public int calcCommerceIncome(Card playedCard, GameState gs, int playerId)
        {
            PlayerState thePlayer = gs.getPlayerNum(playerId);
            //System.Console.WriteLine(playedCard.getNumber());
            if (!playedCard.getName().StartsWith("CO"))
            {
                return 0;
            }
            if(playedCard.GetType() != typeof(CommerceCard))
            {
                return 0;
            }
            CommerceCard commCard = (CommerceCard)playedCard;
            if (commCard.getCoins() != 0)
            {
                return commCard.getCoins();
            }
            if ((commCard.getNumber().Equals(77)) ||
                (commCard.getNumber().Equals(78)))
            {
                return countCardType(1, thePlayer) 
                    + countCardType(1, gs.getLeftPlayer(thePlayer))
                    + countCardType(1, gs.getRightPlayer(thePlayer));
            }
            if ((commCard.getNumber().Equals(80)) ||
                (commCard.getNumber().Equals(79)))
            {
                return countCardType(2, thePlayer)
                    + countCardType(2, gs.getLeftPlayer(thePlayer))
                    + countCardType(2, gs.getRightPlayer(thePlayer));
            }
            //Haven
            if ((commCard.getNumber().Equals(119)) ||
                (commCard.getNumber().Equals(120)))
            {
                return countCardType(1, thePlayer);
            }
            //LightHouse
            if ((commCard.getNumber().Equals(121)) ||
                (commCard.getNumber().Equals(122)))
            {
                return countCardType(4, thePlayer);
            }
            //Chamber of Commerce
            if ((commCard.getNumber().Equals(123)) ||
                (commCard.getNumber().Equals(124)))
            {
                return 2 * countCardType(2, thePlayer);
            }
            //Arena
            if ((commCard.getNumber().Equals(125)) ||
                (commCard.getNumber().Equals(126)) ||
                (commCard.getNumber().Equals(127)))
            {
                return 3 * thePlayer.getBoard().getCurrentWonderLevel();
            }
            return 0;
        }

        public int countCardType(int cType, PlayerState pState)
        {
            int count = 0;
            foreach (Card cCard in pState.getPlayedCards())
            {
                if (cCard.getType() == cType)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
