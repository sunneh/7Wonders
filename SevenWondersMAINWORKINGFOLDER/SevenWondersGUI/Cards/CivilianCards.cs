using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI{

    public class CivilianCard : Card{

        private int victorypoints;

        public CivilianCard(string cn, int n, int p, int a, int t, int[] c, int coi, string na, int[] pc, int v) :
            base(cn, n, p, a, t, c, coi, na, pc){

            victorypoints = v;
        }

        public int getVictoryPoints(){
            return victorypoints;
        }
    }
}
