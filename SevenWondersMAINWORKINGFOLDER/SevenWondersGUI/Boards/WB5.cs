using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB5: Board
    {
        //private int[] resources = new int[7]{0,0,0,0,0,0,1};

        public WB5()
            : base("WB5", 3, new int[3, 7] { { 0, 2, 0, 0, 0, 0, 0 }, { 2, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 2 } }, new int[7] { 0, 0, 0, 0, 0, 0, 1 })
        {}

       // public int[] getResources() { return resources; }

        public override void incrementWonderLevel(PlayerState p)
        { 
            if(notMaxYet()){
                currentWonderLevel++;
                switch (currentWonderLevel)
                {
                    case 1:
                        victoryPoints = 3;
                        return;
                    case 2:
                        p.updateCoins(9);
                        break;
                    case 3:
                        victoryPoints = 10;
                        break;

                }
            }
        }

    }
}
