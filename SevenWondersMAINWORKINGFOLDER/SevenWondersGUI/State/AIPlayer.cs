using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class AIPlayer : PlayerState
    {
        GameStrategy gameStrategy;

        public AIPlayer(string n, int s) : base (n,s) 
        {
        }

        public void setStrategy(int strategy)
        {           
            this.gameStrategy = StrategyFactory.getInstance().getGameStrategy(strategy);            
        }

        public override bool IsAIPlayer() { return true; }

        public Card playACard()
        {            
            //System.Console.WriteLine(gameStrategy.Name());                       

            Card c = gameStrategy.getNextCard(this,hand);
            if (c == null)
            {
                c = gameStrategy.discardCard(this, hand);
                updateCoins(3);
            }
            getHand().Remove(c);
            setPlayedACard();
            return c;
        }
    }

}
