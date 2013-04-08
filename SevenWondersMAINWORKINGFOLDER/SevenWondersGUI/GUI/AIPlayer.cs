using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class AIPlayer : PlayerState, PlayerStrategy
    {
        GameStrategy strategy;

        public AIPlayer(string n, int s) : base (n,s) 
        { 
        }
        
        public void setStrategy(GameStrategy strategy)
        {
            this.strategy = strategy;
        }        
    }

}
