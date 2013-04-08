using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB9: Board
    {
        //private int[] resources = new int[7]{1,0,0,0,0,0,0};
        private bool freeBuild = false;
        private bool freeBuildThisAge = false;

        public WB9()
            : base("WB9", 3, new int[3, 7] { { 2, 0, 0, 0, 0, 0, 0 }, { 0, 2, 0, 0, 0, 0, 0 }, { 0, 0, 0, 2, 0, 0, 0 } }, new int[7] { 1, 0, 0, 0, 0, 0, 0 })
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
                        freeBuildThisAge = true;
                        break;
                    case 3:
                        victoryPoints = 10;
                        break;

                }
            }
        }

    }
}
