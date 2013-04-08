using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public abstract class resourceChecks
    {
        protected int[] cost;
        protected int[] baseResources;
        protected int[] lResources;
        protected int[] rResources;
        protected int[] lTotal;
        protected int[] rTotal;
        protected PlayerState left;
        protected PlayerState right;
        protected List<ResourceCard> SRCardLst;
        protected List<CommerceCard> SCCardLst;
        protected List<ResourceCard> lSRCardLst;
        protected List<ResourceCard> rSRCardLst;
        protected int[] coinTransactions;

        //Base Constructor, Needs GameState and PlayerState
        protected resourceChecks(GameState g, PlayerState p) {
            //coinTransactions = new int[3]{0,0,0};
            
            //Getting left and right player of player P
            left = g.getLeftPlayer(p);
            right = g.getRightPlayer(p);

            //Initialize base resources
            baseResources = ResourceInitializer(p);
            lResources = ResourceInitializer(left);
            rResources = ResourceInitializer(right);
            
            //List of important resource cards
            SRCardLst = SRCardInitializer(p);
            lSRCardLst = SRCardInitializer(left);
            rSRCardLst = SRCardInitializer(right);
        }

        //Getting Resources for each player
        public int[] getBaseResources() { return baseResources; }
        public int[] getlTotal() { return lTotal; }
        public int[] getrTotal() { return rTotal; }
        public int[] getCost()   { return cost; }
        public int[] getspecialResources() { return availableSpecialResources(SRCardLst); }

        //initial baseResources build up, SRCard initializer
        protected int[] ResourceInitializer(PlayerState p) {
            int[] rTotals = p.getBoard().getResources();
            int[] tempResource = new int[7];
            //Fill rTotals first, Wonder then Cards
            
            //Cards
            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {
                //resource/manufactured good cards only
                if ((p.getPlayedCards()[i].getType() == 1) || (p.getPlayedCards()[i].getType() == 2))
                {
                    //remove optional resource cards from total 
                    if ((p.getPlayedCards()[i].getNumber() < 7) || (p.getPlayedCards()[i].getNumber() > 14))
                    {
                        ResourceCard tempCard = (ResourceCard)p.getPlayedCards()[i];
                        
                        System.Console.WriteLine("tempCard: " + tempCard.getResources());
                        
                        addResources(rTotals,tempCard.getResources());
                    }
                }
            }
            return rTotals;
        }

        protected List<ResourceCard> SRCardInitializer(PlayerState p)
        {
            List<ResourceCard> tempList = new List<ResourceCard>();
            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {
                if ((p.getPlayedCards()[i].getNumber() > 7) && (p.getPlayedCards()[i].getNumber() < 14)) 
                { tempList.Add((ResourceCard)p.getPlayedCards()[i]); }
            }
            return tempList;
        }

        //Compares the value of cost and baseResources, returns true if there are enough resources
        protected bool compareResources()
        {
            for (int i = 0; i < 7; i++) { if (cost[i] > baseResources[i]) { return false; } }
            return true;
        }

        //Adds resources together!
        protected int[] addResources(int[] initial, int[] toAdd)
        {
            int temp = 0;
            for (int i = 0; i < 7; i++)
            {
                temp = initial[i] + toAdd[i];
                System.Console.WriteLine("value of temp: " + temp + " initial length is: " + initial.Length+ " toAdd length is: " + toAdd.Length);
                initial[i] = temp;
            }
            return initial;
        }


        protected void UsingSpecialResourceCards(int resourceNeeded){
            //Can't allocate resource values higher then type 7
            if (resourceNeeded > 6) { return; }

            //Cycle through list of special resource cards and look for cards with said resources
            for (int i = 0; i < SRCardLst.Count; i++)
            {
                if (SRCardLst[i].getResources()[resourceNeeded] > 0)
                {
                    SRCardLst.RemoveAt(i);
                    baseResources[resourceNeeded]++;
                }
            }
        }

        protected int[] availableSpecialResources(List<ResourceCard> lst) {
            int[] specialResources = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < lst.Count; i++)
            {
                addResources(specialResources, lst[i].getResources());
            }
            return specialResources;
        }

        //Updates the total to be displayed on the gui
        protected void updateTradeTotals(){
            System.Console.WriteLine("Inside of updateTradeTotals");
            lTotal = addResources(availableSpecialResources(lSRCardLst), lResources);
            rTotal = addResources(availableSpecialResources(rSRCardLst), rResources);
        }

        protected void tradeResources(int player, int resourceNeeded)
        {
            switch (player)
            {
                case 0://left
                    tradeResourcesResolution(lSRCardLst, lResources, resourceNeeded);
                    break;
                case 1://right
                    tradeResourcesResolution(rSRCardLst, rResources, resourceNeeded);
                    break;
            }

        }

        protected int[] tradeResourcesResolution(List<ResourceCard> lst, int[] pResources, int resourceNeeded) {
            //
            if (pResources[resourceNeeded] > 0)
            {
                pResources[resourceNeeded]--;
                //Forgot to adjust here for coins
                baseResources[resourceNeeded]++;
            }
            else
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].getResources()[resourceNeeded] > 0)
                    {
                        lst.RemoveAt(i);
                        baseResources[resourceNeeded]++;
                    }
                }
            }
            return pResources;
        }
    }
}
