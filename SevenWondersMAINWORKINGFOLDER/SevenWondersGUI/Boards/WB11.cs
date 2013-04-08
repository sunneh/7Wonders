using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB11 : Board
    {
        //private int[] resources = new int[7]{0,0,0,0,0,1,0};
        private bool freeBuild = false;
        private bool freeBuildThisAge = false;

        public WB11()
            : base("WB11", 3, new int[3, 7] { { 0, 0, 2, 0, 0, 0, 0 }, { 0, 0, 0, 3, 0, 0, 0 }, { 0, 0, 0, 0, 0, 2, 0 } }, new int[7] { 0, 0, 0, 0, 0, 1, 0 })
        {}

        //public int[] getResources() { return resources; }

        public override void incrementWonderLevel(PlayerState p)
        { 
            if(notMaxYet()){
                currentWonderLevel++;
                switch (currentWonderLevel)
                {
                    case 1:
                        victoryPoints = 3;
                        break;
                    case 2:
                        freeBuild = true;
                        break;
                    case 3:
                        victoryPoints = 10;
                        break;

                }
            }
        }

    }
}
