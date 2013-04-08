using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public abstract class Card
    {
        public static readonly int _RESOURCE = 1;
        public static readonly int _MATERIAL = 2;
        public static readonly int _CIVILIAN = 3;
        public static readonly int _MERCHANT = 4;
        public static readonly int _MILITARY = 5;
        public static readonly int _SCIENCE  = 6;
        public static readonly int _GUILD    = 7;

        [Flags]
        public enum Resource { Wood = 0, Stone = 1, Clay = 2, Ore = 3, Glass = 4, Loom = 5, Paper = 6 };
        //public enum Resource { Clay = 0, Ore = 1, Stone = 2, Wood = 3, Glass = 4, Loom = 5, Paper = 6 };

        [Flags]
        public enum Structure { Resource = 1, Material = 2, Civilian = 3, Merchant = 4, Military = 5, Science = 6, Guild = 7 };
        
        private int act;            //restriction on which age the card is played in
        private int type;           //card type, 1 = resource, 2 = mgoods, 3 = civ, 4 = merch, 5 = military, 6 = science, 7 = guild
        private int number;         //cardnumbers 0-49 Age1, 50-99 Age2, 100-149 Age3   
        private int playercount;    //restriction on which cards are in game
        private string cardName;

        private int ccost;          //coin cost
        private int rcost;          //Total Resource Cost

        private int[] cost;         //order is Wood, Stone, Clay, Ore, Glass, Loom, Paper
        private int[] preCard;      //list of card.number that can over ride cost.

        private string name;

        public Card(string cn, int n, int p, int a, int t, int[] c, int cc, string na, int[] pc)
        {
            cardName = cn;
            number = n;
            playercount = p;
            act = a;
            type = t;
            cost = c;
            ccost = cc;
            name = na;
            preCard = pc;           
            setResourceCost();
        }

        public Card() { }

        public string getCardName() { return cardName; }

        public int getNumber()
        {
            return number;
        }

        public string getName()
        {
            return name;
        }

        public int getCoinCost()
        {
            return ccost;
        }

        public int getTotalResourceCost()
        {
            return rcost;
        }

        public int[] getCost() 
        { 
            return cost; 
        }

        public int getPlayerCount() 
        { 
            return playercount; 
        }

        public int getAct()
        { 
            return act; 
        }

        public int getType()
        { 
            return type; 
        }

        public int[] getPreCard() 
        { 
            return preCard; 
        }

        public String id(){ return "" + act + type + number; }

        private void setResourceCost ()
        {
            int i = 0;
            string[] resources = Enum.GetNames(typeof(Resource));
            foreach (var type in resources)
                rcost += cost[i++];
           // System.Console.WriteLine("--->>> Card:: Total Resource Cost for {0} {1:D}", name, rcost);    
        }
        public void toString() { System.Console.WriteLine( cardName + "{" + cost[0] + "," + cost[1] + "," + cost[2] + "," + cost[3] + "," + cost[4] + "," + cost[5] + "," + cost[6] + "}"); }

    }
}
