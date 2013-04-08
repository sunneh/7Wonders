using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI.Engine
{
    class WonderCostCheck : resourceChecks
    {
        public WonderCostCheck(GameState g, PlayerState p): base (g,p) {
            cost = p.getBoard().getBuildCost(p.getBoard().getCurrentWonderLevel());
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
    }
}
