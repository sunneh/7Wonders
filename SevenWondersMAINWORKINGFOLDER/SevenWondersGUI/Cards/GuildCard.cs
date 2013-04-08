using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class GuildCard : Card
    {
        public GuildCard(string cn, int n, int p, int a, int t, int[] c, int coi, string na, int[] pc) :
            base(cn, n, p, a, t, c, coi, na, pc){                          
        }       
    }
}
