using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class CommerceCard : Card {

        //Resource Related
        //[x,y]
        //x: 0 = all, 1 = left, 2 = right
        //y: 0 = none, 1 = resources, 2 = manufactured goods 
        private int[]   resourceTradable; 
        private int     resource;  //0 = none, 1 = resource card, 2 = manufactured good
        private int     coins;

        //Neighbor Card Related
        //[x,y] 
        //x: 0 = all, 1 = self
        //y: 0 = nothing, 1 = gold, 2 = victory point
        private int[]  collect;

        // Wonder Related
        // #t/#f, always 3 coins, 1 vp per level of wonder
        private int perWonder;

        public CommerceCard(string cn, int n, int p, int a, int t, int[] c
                           , int ccoi, string na, int[] pc, int[] rt
                           , int r, int coi, int[] col, int w)
            : base(cn, n, p, a, t, c, ccoi, na, pc)
        {
            resourceTradable = rt;
            resource = r;
            coins = coi;
            collect = col;
            perWonder = w;
        }

        public int getCoins()
        {
            return coins;
        }

        public int getResources() 
        { 
            return resource; 
        }

        public int getPerWonder()
        {
            return perWonder;
        }

        public bool hasTradableResources()
        {
            return (resourceTradable.Count() > 0); 
        }

        public int[] getCollect()
        { 
            return collect; 
        }

        public int[] getResourceTradable()
        {
            return resourceTradable;
        }

    }
}
