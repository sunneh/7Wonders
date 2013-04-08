using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class Resources
    {
        private int[] left;
        private int[] right;
        private int[] center;
        private string playername;

        public Resources(int[] l, int[] r, int[] c, string n)
        {
            left = l;
            right = r;
            center = c;
            playername = n;
        }

        public string getPlayerName() { return playername; }

        public int[] getLeft()
        {
            return left;
        }

        public int[] getRight()
        {
            return right;
        }

        public int[] getCenter()
        {
            return center;
        }
    }
}
