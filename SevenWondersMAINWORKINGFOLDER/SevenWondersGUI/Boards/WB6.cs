using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB6: Board
    {
        //private int[] resources = new int[7]{0,0,0,0,0,0,1};

        public WB6()
            : base("WB6", 3, new int[3, 7] { { 0, 2, 0, 0, 0, 0, 0 }, { 2, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 1, 1, 1 } }, new int[7] { 0, 0, 0, 0, 0, 0, 1 })
        {}

        //public int[] getResources() { return resources; }

        public override void incrementWonderLevel(PlayerState p)
        { 
            if(notMaxYet()){
                currentWonderLevel++;
                switch (currentWonderLevel)
                {
                    case 1:
                        victoryPoints = 2;
                        p.updateCoins(4);
                        return;
                    case 2:
                        victoryPoints = 5;
                        p.updateCoins(4);
                        break;
                    case 3:
                        victoryPoints = 10;
                        p.updateCoins(4);
                        break;

                }
            }
        }

    }
}
