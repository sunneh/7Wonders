using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class ScienceCard : Card
    {
        private int sciType;

        public ScienceCard(string cn, int n, int p, int a, int t, int[] c, int coi, string na, int[] pc, int v) :
            base(cn, n, p, a, t, c, coi, na, pc){
            sciType = v;
        }

        public int getSciType(){ return sciType; }
    }
}
