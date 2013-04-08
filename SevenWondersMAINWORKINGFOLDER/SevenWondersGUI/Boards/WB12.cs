using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB12 : Board
    {
        //private int[] resources = new int[7]{0,0,0,0,0,1,0};
        private int freeBuildCounter = 0;

        public WB12()
            : base("WB12", 3, new int[3, 7] { { 0, 0, 0, 2, 0, 0, 0 }, { 0, 0, 3, 0, 0, 0, 0 }, { 0, 0, 0, 0, 1, 1, 1 } }, new int[7] { 0, 0, 0, 0, 0, 1, 0 })
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
                        freeBuildCounter++;
                        break;
                    case 2:
                        victoryPoints = 3;
                        freeBuildCounter++;
                        break;
                    case 3:
                        freeBuildCounter++;
                        break;

                }
            }
        }

    }
}