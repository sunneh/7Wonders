using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB8: Board
    {
        //private int[] resources = new int[7]{0,0,0,1,0,0,0};

        public WB8()
            : base("WB8", 2, new int[2, 7] { { 0, 3, 0, 0, 0, 0, 0 }, { 0, 0, 0, 4, 0, 0, 0 } }, new int[7] { 0, 0, 0, 1, 0, 0, 0 })
        {}

        //public int[] getResources() { return resources; }

        public override void incrementWonderLevel(PlayerState p)
        { 
            if(notMaxYet()){
                currentWonderLevel++;
                switch (currentWonderLevel)
                {
                    case 1:
                        p.updateMilitaryPower(1);
                        victoryPoints = 3;
                        p.updateCoins(3);
                        return;
                    case 2:
                        p.updateMilitaryPower(1);
                        victoryPoints = 7;
                        p.updateCoins(4);
                        break;
                }
            }
        }

    }
}
