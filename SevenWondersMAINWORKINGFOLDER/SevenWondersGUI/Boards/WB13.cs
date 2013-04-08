﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class WB13 : Board
    {
        //private int[] resources = new int[7]{0,1,0,0,0,0,0};

        public WB13()
            : base("WB13", 3, new int[3, 7] { { 0, 2, 0, 0, 0, 0, 0 }, { 3, 0, 0, 0, 0, 0, 0 }, { 0, 4, 0, 0, 0, 0, 0 } }, new int[7] { 0, 1, 0, 0, 0, 0, 0 })
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
                        victoryPoints = 8;
                        break;
                    case 3:
                        victoryPoints = 15;
                        break;

                }
            }
        }

    }
}
