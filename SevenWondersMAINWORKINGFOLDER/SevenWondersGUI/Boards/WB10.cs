using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB10 : Board
    {
        //private int[] resources = new int[7]{1,0,0,0,0,0,0};
        private bool copyGuildCard = false;
        private bool resourceTrade = false;

        public WB10()
            : base("WB10", 3, new int[3, 7] { { 2, 0, 0, 0, 0, 0, 0 }, { 0, 2, 0, 0, 0, 0, 0 }, { 0, 0, 0, 2, 0, 1, 0 } }, new int[7] { 1, 0, 0, 0, 0, 0, 0 })
        {}

        //public int[] getResources() { return resources; }

        public override void incrementWonderLevel(PlayerState p)
        { 
            if(notMaxYet()){
                currentWonderLevel++;
                switch (currentWonderLevel)
                {
                    case 1:
                        resourceTrade = true;
                        break;
                    case 2:
                        victoryPoints = 5;
                        break;
                    case 3:
                        copyGuildCard = true;
                        break;

                }
            }
        }

    }
}
