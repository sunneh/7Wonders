using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class CardCostCheck : resourceChecks
    {
        public CardCostCheck(GameState g, PlayerState p, Card c): base (g,p) {
            coinTransactions = new int[3] { 0, 0, 0 };

            //Getting left and right player of player P
            left = g.getLeftPlayer(p);
            right = g.getRightPlayer(p);

            //Initialize base resources
            baseResources = ResourceInitializer(p);
            lResources = ResourceInitializer(left);
            rResources = ResourceInitializer(right);

            //List of important resource cards
            SRCardLst = SRCardInitializer(p);
            lSRCardLst = SRCardInitializer(left);
            rSRCardLst = SRCardInitializer(right);   
        }

        //preCardCheck checks if the player p has the prerequisite card for free play
        protected bool preCardCheck(Card c, PlayerState p)
        {
            //Get a list of preCards
            int[] preCard = c.getPreCard();
            //Go through list of preCards
            console.writeline(preCard[0] + " " + preCard[1] + " " + preCard[2] + " " + preCard[3]);
            for (int i = 0; i < preCard.Length; i++)
            {
                //Check if preCard Value is valid
                if (preCard[i] > 150)
                {
                    //cycle through player's playedcards
                    for (int j = 0; j < p.getPlayedCards().Count; j++)
                    {//if the cards match then return true
                        if (p.getPlayedCards()[j].getNumber() == preCard[i]) { return true; }
                    }
                }
            }
            //else return false
            return false;
        }


    }
}
