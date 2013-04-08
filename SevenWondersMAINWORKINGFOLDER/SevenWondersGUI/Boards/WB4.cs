using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB4 : Board
    {
        //private int[] resources = new int[7]{0,0,1,0,0,0,0};
        private bool scienceActivated = false;
        private bool doubleCard = false;

        public WB4()
            : base("WB4", 3, new int[3, 7] { { 0, 0, 1, 0, 0, 1, 0 }, { 2, 0, 0, 0, 1, 0, 0 }, { 0, 0, 3, 0, 0, 0, 1 } }, new int[7] { 0, 0, 1, 0, 0, 0, 0 })
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
                        return;
                    case 2:
                        doubleCard = true;
                        break;
                    case 3:
                        scienceActivated = true;
                        break;

                }
            }
        }

    }
}
