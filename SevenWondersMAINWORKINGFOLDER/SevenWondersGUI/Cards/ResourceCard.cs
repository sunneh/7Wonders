using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class ResourceCard : Card
    {
        private int[] resources; //Same layout as cost

        public ResourceCard(string cn, int num, int player, int age, int type, int[] cost, int coin, string name, int[] pc, int[] r)
            : base(cn, num, player, age, type, cost, coin, name, pc){
            resources = r;
        }

        public int[] getResources() { return resources; }

        public bool hasTradableResources()
        {
            return (resources.Count() > 0);
        }

    }
}
