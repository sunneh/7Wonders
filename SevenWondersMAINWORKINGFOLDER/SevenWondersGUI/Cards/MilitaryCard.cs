using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class MilitaryCard : Card
    {
        private int value;

        public MilitaryCard(string cn, int n, int p, int a, int t, int[] c, int coi, string na, int[] pc, int v) :
            base(cn, n, p, a, t, c, coi, na, pc){
            value = v;
        }

        public int getValue(){ return value; }

    }       
}
