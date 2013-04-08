using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class ResourceManager : Card
    {               
        private Dictionary<string, List<int>> baseResources = null;//Map of Base Resources
        private Dictionary<string, List<ResourceCard>> SResources = null;//Map of Special Resource Cards
        private Dictionary<string, List<CommerceCard>> ComCards = null;//Map of Commerce Cards used for this function
        private Dictionary<string, List<int>> TempResources = null; //Map of Temporary Resources

        private static GameState gameState = null; 
        private static ResourceManager _instance = null;
        private int[] coinTransactions = new int[2]{0,0};
        //Resources resources;
        //private Logger _logger;

        //Constructor
        public ResourceManager(GameState g)
        {   
            gameState = g;

            //resources = GetCombinedResources();

            baseResources = new Dictionary<string,List<int>>();
            SResources    = new Dictionary<string, List<ResourceCard>>();
            TempResources = new Dictionary<string, List<int>>();
            ComCards = new Dictionary<string, List<CommerceCard>>();
            //_logger = Logger.GetInstance(this, g);
            //_logger.SetLogging(true);
            //_logger.LogPlayer(1);
            
            List<PlayerState> players = gameState.getPlayers();
            
            for (int i = 0; i < players.Count; i++)
            {               
                List<int> boardResources = new List<int>();
                List<int> empty = new List<int>();
                List<ResourceCard> rclist = new List<ResourceCard>();
                List<CommerceCard> cclist = new List<CommerceCard>();
                
                for (int j = 0; j < 7; j++)
                {
                    boardResources.Add(players[i].getBoard().getResources()[j]);
                    empty.Add(0);
                }

                baseResources.Add(players[i].getName(), boardResources);
                SResources.Add(players[i].getName(), rclist);
                ComCards.Add(players[i].getName(), cclist);
                TempResources.Add(players[i].getName(), empty);
            }
        }

        public int getCoinTransaction() { return coinTransactions[1] + coinTransactions[0]; }

        //Instance/GameState Stuff
        public static ResourceManager GetInstance()
        {
            if (_instance == null)
                _instance = new ResourceManager(gameState);

            //Console.WriteLine("================= YEA WERE COOL ===================");

            return _instance;
        }

        public static ResourceManager GetInstance(GameState g)
        {
            if (_instance == null)
                _instance = new ResourceManager(g);
            gameState = g;

            //Console.WriteLine("================= SET GAME STATE ===================");
            return _instance;
        }
        //As of now no need to call this method. Add notes here if you want to call this and why.
        public void Reset() { _instance = null; }

        public void SetGameState(GameState g) { gameState = g; }

        public GameState getGameState() { return gameState; }

        /*
        * Input: A Player and a Card
        *
        * 
        * Output: True or False -> Can the player play this card for free? If they have correct pre-card -> True
        */
        private bool preCardCheck(PlayerState p, Card c)
        {
            //Get a list of preCards
            int[] preCard = c.getPreCard();
            //Go through list of preCards
            for (int i = 0; i < preCard.Length; i++)
            {
                //Check if preCard Value is valid
                if (preCard[i] < 150)
                {
                    //cycle through player's playedcards
                    for (int j = 0; j < p.getPlayedCards().Count; j++)
                    {//if the cards match then return true
                        if (p.getPlayedCards()[j].getNumber() == preCard[i]) { return true; }
                    }
                }
            }
            return false;
        }

        /*
        * Input: A Player and a Card
        *
        * 
        * Output: True or False -> Does the player have enough resources to play given card already? 
        */
        public bool ValidateCard (PlayerState p, Card c)
        {
            //_logger.ValidatingCard(p, c);
            //_logger.CheckDictionary(p,hashtable);
            //c.toString();
            //Check for duplicity
            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {
                if (c.getCardName() == p.getPlayedCards()[i].getCardName())
                {
                    if (p.getName().Equals("P0"))
                    {
                        System.Console.WriteLine("!Can't play card with the same name!");
                    }
                    return false; 
                }
            }

            //Check for precards
            if (preCardCheck(p, c)) { return true; }

            // First check what the coin costs and total resource cost are            
            if (c.getCoinCost() == 0 && c.getTotalResourceCost() == 0)
            {
               //_logger.print(p,"Card dosn't cost anything");
                UpdateResources(p, c);
                return true;
            }

            // If the coin cost is not equal to 0 then does the player have enoguh coins ?
            if (c.getCoinCost() != 0 && c.getTotalResourceCost() == 0)
            {
               //_logger.CheckingCoins(p, c);                
                if (c.getCoinCost() <= p.getCoins())
                {
                    UpdateResources(p, c);
                    p.updateCoins(-c.getCoinCost());
                 //  _logger.CheckingPlayersCoins(p);                    
                    return true;
                }
                return false;
            }

            if (c.getTotalResourceCost() != 0)
            {
               //_logger.print(p,"Now we are working on the players Resources");
                return CheckResourceCost(p,c.getCost());
            }

            return false;
        }

        /*
        * Input: A Player, a Card and a direction(0 = right, 1 = left)
        *
        * 
        * Output: number of coins required to trade for resources for card with the player 
        * at the given direction. negitive number if impossible
        */
        public int validateTrade(PlayerState p, Card c, int direction)
        {
            int recCost = 2;
            int comCost = 2;

            //checkes to see if a player can play card with thier own resources
            if (ValidateCard(p, c))
            {
                return 0;
            }

            PlayerState trader;
            if ((p.getBoard().getName().Equals("WB10")
                            && (p.getBoard().getCurrentWonderLevel() >= 1)))
            {
                recCost = 1;
            }

            if (direction == 0)
            {
                trader = gameState.getRightPlayer(p);
                for (int i = 0; i < p.getPlayedCards().Count; i++)
                {
                    if ((p.getPlayedCards()[i].getNumber() == 31) ||
                        (p.getPlayedCards()[i].getNumber() == 32))
                    {
                        recCost = 1;
                        break;
                    }
                }
            }
            else
            {
                trader = gameState.getLeftPlayer(p);
                for (int i = 0; i < p.getPlayedCards().Count; i++)
                {
                    if ((p.getPlayedCards()[i].getNumber() == 33) ||
                        (p.getPlayedCards()[i].getNumber() == 34))
                    {
                        recCost = 1;
                        break;
                    }
                }
            }
            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {
                if ((p.getPlayedCards()[i].getNumber() == 35) ||
                    (p.getPlayedCards()[i].getNumber() == 36))
                {
                    comCost = 1;
                }
            }


            if (!baseResources.ContainsKey(trader.getName()))
            {
                return -1;
            }

            int cost = 0;
            List<int> traderResources = baseResources[trader.getName()];
            int[] requiredResources = new int[c.getCost().Length];
            for (int i = 0; i < c.getCost().Length; i++)
            {
                requiredResources[i] = c.getCost()[i];
                requiredResources[i] -= baseResources[p.getName()][i];//p.getResources()[i];
                if (requiredResources[i] < 0)
                {
                    requiredResources[i] = 0;
                }
                if (traderResources[i] < requiredResources[i])
                {
                    return -1;
                }
                if (i < 4)
                {
                    cost += requiredResources[i] * recCost;
                }
                else
                {
                    cost += requiredResources[i] * comCost;
                }
                //Console.WriteLine("cost " + cost + " cur rec " + requiredResources[i]);
            }
            return cost;
        }

        /*
        * Input: A Player
        *
        * 
        * Output: True or False -> Does the player have enough resources to add to their Wonder Board? 
        */
        public bool ValidateWonder(PlayerState p)
        {
            //System.Console.WriteLine(p.getName() + ":ValidateWonder");
            if (p.getBoard().getCurrentWonderLevel() <p.getBoard().getMaxWonderLevel() )
                return CheckResourceCost(p, p.getBoard().getBuildCost());
            return false;
        }

        //returns the base resources
        /*public Resources GetResources(PlayerState p)
        {
            List<int> l = baseResources[gameState.getLeftPlayer(p).getName()];
            List<int> r = baseResources[gameState.getRightPlayer(p).getName()];
            List<int> c = baseResources[p.getName()];

            // Get the wonder boards default resources
            //_logger.CheckWonderBoard(p);
            
            return new Resources(l.ToArray(), r.ToArray(), c.ToArray(), p.getName());       
        }*/

        /*
        * Input: A Player
        *
        * 
        * Output: Resources Object -> Given the player object, what are the total available resources
        * adding both chosen special resources and base resources? 
        */
        public Resources GetCombinedResources(PlayerState p)
        {
            List<int> l = new List<int>();
            List<int> r = new List<int>();
            List<int> c = new List<int>();

            for (int i = 0; i < 7; i++) 
            {
                l.Add(baseResources[gameState.getLeftPlayer(p).getName()][i] 
                                   + TempResources[gameState.getLeftPlayer(p).getName()][i]);
                r.Add(baseResources[gameState.getRightPlayer(p).getName()][i] 
                                   + TempResources[gameState.getRightPlayer(p).getName()][i]);
                c.Add(baseResources[p.getName()][i] + TempResources[p.getName()][i]);
            }

            // Get the wonder boards default resources
            //_logger.CheckWonderBoard(p);
            //int[] br = p.getBoard().getResources();
            //for (int i = 0; i < br.Count(); i++)
            //    c[i] = (br[i] + c[i]);

            // Now lets get any speical resources the player may have
            //List<ResourceCard> sr = SResources[p.getName()];
            //for (int i = 0; i < sr.Count(); i++)
            //{
            //    ResourceCard rc = sr[i];
            //    int[] resources = rc.getResources();
            //    for (int index = 0; index < resources.Count(); index++)
            //        c[index] = (c[index] + resources[index]);
            //}

            return new Resources(l.ToArray(), r.ToArray(), c.ToArray(), p.getName());
        }

        /*
        * Input: A Player
        * 
        * Output: void -> Given a player, and called upon some action in the players cards, update
        * The special resources to the new state. Example, a player chooses one type of resource from
        * a special resource card. That card is now removed and the available special resources
        * must be updated
        */
        public void resetResources(PlayerState p)
        {
            //Commerce Cards
            resetCommerceCards(p);
            //Special Resources
            resetSpecialResourceSingular(p);
            resetSpecialResourceSingular(gameState.getLeftPlayer(p));
            resetSpecialResourceSingular(gameState.getRightPlayer(p));
            //Temp Resources
            resetTempResources(p);
            resetTempResources(gameState.getLeftPlayer(p));
            resetTempResources(gameState.getRightPlayer(p));
            coinTransactions = new int[2] { 0, 0 };
            System.Console.WriteLine("RESET RESOURCES");
        }

        //Missing Code in this Function
        public void resetCommerceCards(PlayerState p) {
            List<CommerceCard> tempList = new List<CommerceCard>();

            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {
                if (p.getPlayedCards()[i].getType() == 4)
                {
                    CommerceCard c = (CommerceCard)p.getPlayedCards()[i];

                    if (((c.getNumber() > 30) && (c.getNumber() < 37)) || ((c.getNumber() > 70) && (c.getNumber() < 77)))
                    {
                        tempList.Add(c);
                    }
                }
            }
            ComCards[p.getName()] = tempList;
        }

        /*
        * Input: A Player
        * 
        * Output: void -> Given a player, This is the helper function to resetResources.
        * This clears the temp resource of a player.
        */
        public void resetTempResources(PlayerState p) { 
            List<int> temp = new List<int>();
            for(int i = 0; i<7 ; i++) {temp.Add(0);}
            TempResources[p.getName()] = temp;
        }

        /*
        * Input: A Player
        * 
        * Output: void -> Given a player, This is the helper function to resetResources.
        * This looks through the list of played cards and adds special card values to a temporary
        * list of special resources.
        */
        public void resetSpecialResourceSingular(PlayerState p)
        {
            List<ResourceCard> tempList = new List<ResourceCard>();
           
            for (int i = 0; i < p.getPlayedCards().Count; i++)
            {               
                if (p.getPlayedCards()[i].getType() < 3) 
                {                     
                    ResourceCard c = (ResourceCard)p.getPlayedCards()[i]; 
                    
                    if (((c.getNumber() > 7) && (c.getNumber() < 14))) 
                    { 
                        tempList.Add(c); 
                    }
                }
            }
            SResources[p.getName()] = tempList;
        }

        /*
        * Input: A Player, index of resource used
        * 
        * Output: void -> Given a player, 
        */
        public void usingSpecialResource(PlayerState p, int resourceIndex) {
            int count = SResources[p.getName()].Count;

            //Can't allocate resource values higher then type 7
            if (resourceIndex > 6 || resourceIndex < 0) { return; }

            if (ComCards[p.getName()].Count > 0)
            {
                for (int i = 0; i < ComCards[p.getName()].Count; i++)
                {
                    //Resource
                    if ((resourceIndex < 4) && (ComCards[p.getName()][i].getResources() == 1 )) 
                    {
                        TempResources[p.getName()][resourceIndex]++;
                        
                        //System.Console.WriteLine("REMOVING :" + ComCards[p.getName()][i].getName() + " FROM " + p.getName());
                        int[] resources = trArray(p);
                        //System.Console.WriteLine("ResourceArray for " + p.getName() + " {" + resources[0] + "," + resources[1] + "," + resources[2] + "," + resources[3] + "," + resources[4] + "," + resources[5] + "," + resources[6] + "}");
                    
                        ComCards[p.getName()].RemoveAt(i);
                        return;
                    }
                    //Man. goods
                    else if ((resourceIndex > 3) && (ComCards[p.getName()][i].getResources() == 2))
                    {
                        TempResources[p.getName()][resourceIndex]++;
                        
                        //System.Console.WriteLine("REMOVING :" + ComCards[p.getName()][i].getName() + " FROM " + p.getName());
                        int[] resources = trArray(p);
                        //System.Console.WriteLine("ResourceArray for " + p.getName() + " {" + resources[0] + "," + resources[1] + "," + resources[2] + "," + resources[3] + "," + resources[4] + "," + resources[5] + "," + resources[6] + "}");
                    
                        ComCards[p.getName()].RemoveAt(i);
                        return;
                    }
                }
            }


            //Cycle through list of special resource cards and look for cards with said resources
            for (int i = 0; i < count ; i++)
            {                
                if (SResources[p.getName()][i].getResources()[resourceIndex] > 0)
                {
                    TempResources[p.getName()][resourceIndex]++;

                    System.Console.WriteLine("REMOVING :" + SResources[p.getName()][i].getName() + " FROM " + p.getName());
                    int[] resources = trArray(p);
                    System.Console.WriteLine("ResourceArray for " + p.getName() + " {" + resources[0] + "," + resources[1] + "," + resources[2] + "," + resources[3] + "," + resources[4] + "," + resources[5] + "," + resources[6] + "}");
                    

                    SResources[p.getName()].RemoveAt(i);
                    return;
                }

            }
        }

        private void UpdateResources(PlayerState p, Card c)
        {
           //_logger.UpdatingPlayersResources(p,c);
           //_logger.DisplayResourceCost(p,c);

            if (c is CommerceCard)
            {
                //Coin Transaction from playing a commerce Card
                KensUtilityfunctions k = new KensUtilityfunctions();
                int income = k.calcCommerceIncome(c, gameState, p.getSeatNumber());
                p.updateCoins(income);

                //Adding commerce card if it affects trade/resources
                if (((c.getNumber() > 30) && (c.getNumber() < 37)) || ((c.getNumber() > 70) && (c.getNumber() < 77))) {
                    ComCards[p.getName()].Add((CommerceCard)c);
                }
                return;
            }

            if (c is ResourceCard)
            {
               //_logger.CheckResourceCard(p,c);
               //_logger.DisplayPlayersResources(p, hashtable);
                
                List<int> current = baseResources[p.getName()];

                if (((ResourceCard)c).hasTradableResources())
                {
                    if ((((ResourceCard)c).getNumber() > 7) && (((ResourceCard)c).getNumber() < 14))
                    {
                        //populate Special Resource Hashmap if the Card is a special resource
                        List<ResourceCard> srlst = SResources[p.getName()];
                        srlst.Add((ResourceCard)c);
                        SResources[p.getName()] = srlst;
                        //System.Console.WriteLine(p.getName() + " is adding Special Resource Card: {" + ((ResourceCard)c).getResources()[0] + "," + ((ResourceCard)c).getResources()[1] + "," + ((ResourceCard)c).getResources()[2] + "," + ((ResourceCard)c).getResources()[3] + "," + ((ResourceCard)c).getResources()[4] + "," + ((ResourceCard)c).getResources()[5] + "," + ((ResourceCard)c).getResources()[6] + "}");
                    }
                    else
                    {
                        int[] tradableResources = ((ResourceCard)c).getResources();
                        for (int index = 0; index < tradableResources.Count(); index++)
                        {
                            switch (index)
                            {
                                case (int)Resource.Wood:
                                    current[(int)Resource.Wood] += tradableResources[index];
                                    break;
                                case (int)Resource.Stone:
                                    current[(int)Resource.Stone] += tradableResources[index];
                                    break;
                                case (int)Resource.Clay:
                                    current[(int)Resource.Clay] += tradableResources[index];
                                    break;
                                case (int)Resource.Ore:
                                    current[(int)Resource.Ore] += tradableResources[index];
                                    break;
                                case (int)Resource.Glass:
                                    current[(int)Resource.Glass] += tradableResources[index];
                                    break;
                                case (int)Resource.Loom:
                                    current[(int)Resource.Loom] += tradableResources[index];
                                    break;
                                case (int)Resource.Paper:
                                    current[(int)Resource.Paper] += tradableResources[index];
                                    break;
                            }
                        }
                        //System.Console.WriteLine(p.getName() +" is adding: {" + tradableResources[0] + "," + tradableResources[1] + "," + tradableResources[2] + "," + tradableResources[3] + "," + tradableResources[4] + "," + tradableResources[5] + "," + tradableResources[6] + "}");
                        baseResources[p.getName()] = current;
                        
                        //_logger.DisplayPlayersResources(p, hashtable);
                    }
                }
                return;
            }
           //_logger.CheckDictionary(p,hashtable);
           //_logger.DisplayPlayersResources(p, hashtable);
        }

        //trade card to player
        public void tradeTo(PlayerState p, int resourceIndex, int player){
            int coinAmount = 2;
            int[] tradingPostCardNumber = new int[2]{0,0};
            if (resourceIndex > 6)
            {
                System.Console.WriteLine("resourceIndex over 6");
                return;
            }

            PlayerState TradeTo = null;

            //Set TradetoPlayer : player = 0, left, else right
            if (player == 0) 
            { 
                TradeTo = gameState.getLeftPlayer(p);
                tradingPostCardNumber[0] = 33;
                tradingPostCardNumber[1] = 34;
            }
            else if (player == 1)
            { 
                TradeTo = gameState.getRightPlayer(p);
                tradingPostCardNumber[0] = 31;
                tradingPostCardNumber[1] = 32;
            }

            //Commerce Trading Exceptions
            for (int i = 0; i < ComCards[p.getName()].Count; i++)
            {
                //Market Card
                if ((resourceIndex > 3) && ((ComCards[p.getName()][i].getNumber() == 35) || (ComCards[p.getName()][i].getNumber() == 36))) { coinAmount = 1; }
                //Trading Post 
                if ((resourceIndex < 4) && ((ComCards[p.getName()][i].getNumber() == tradingPostCardNumber[0]) || (ComCards[p.getName()][i].getNumber() == tradingPostCardNumber[1]))) { coinAmount = 1; }
            }

            //1. Check for resource in base
            if (baseResources[TradeTo.getName()][resourceIndex] > 0) {
                coinTransactions[player] += coinAmount;
                TempResources[p.getName()][resourceIndex]++;
                TempResources[TradeTo.getName()][resourceIndex]--;
                System.Console.WriteLine("Traded from Base Resource.");
            }
            //2. Then check resource in SR
            else if(SpecialResourceArray(TradeTo.getName())[resourceIndex] > 0){
                coinTransactions[player] += coinAmount;
                TempResources[p.getName()][resourceIndex]++;
                usingSpecialResource(TradeTo, resourceIndex);
                System.Console.WriteLine("Traded from Special Resource.");
            }
            //3. Profit
            else { System.Console.WriteLine("Error, no such resource available."); }
        }

        //CoinTransaction
        public void coinExchange(PlayerState p) {

            gameState.getLeftPlayer(p).updateCoins(coinTransactions[0]);
            gameState.getRightPlayer(p).updateCoins(coinTransactions[1]);
            gameState.getPlayers()[p.getSeatNumber()].updateCoins(0 - coinTransactions[0] - coinTransactions[1]);
            System.Console.WriteLine("Coins Exchanged! Left: " + coinTransactions[0] + " Right: " + coinTransactions[1] + " You: " + (0 - coinTransactions[0] - coinTransactions[1]));
        }

        //CoinTransaction Check
        public bool canAfford(PlayerState p)
        {
            if (p.getCoins() < (coinTransactions[0] + coinTransactions[1]))
            {
                return false;
            }
            return true;
        }

        //Creates an int[7] of special resources from player name
        public int[] SpecialResourceArray(string s) {
            int[] specialResourceTable = new int[7]{0,0,0,0,0,0,0};
            
            //Special Resource Cards
            for (int i = 0; i < SResources[s].Count; i++) 
            {
                for (int j = 0; j < 7; j++)  {  specialResourceTable[j] += SResources[s][i].getResources()[j]; }
            }

            //Special Commerce Cards
            for (int i = 0; i < ComCards[s].Count; i++)
            {
                if (ComCards[s][i].getResources() == 1)
                {                
                    for (int j = 0; j < 7; j++) { if(j < 4 ) { specialResourceTable[j]++; } }
                }
                if (ComCards[s][i].getResources() == 2)
                {                
                    for (int j = 0; j < 7; j++) { if (j > 3) { specialResourceTable[j]++; } }
                }

            }
            return specialResourceTable;
        }

        //Returns true if the player can afford the cost
        public bool CheckResourceCost(PlayerState p, int[] cost)
        {
            int[] totalResources = { 0, 0, 0, 0, 0, 0, 0 };
            if (!canAfford(p)) { return false; }
            List<int> playersResources = baseResources[p.getName()];
            List<int> tempR = TempResources[p.getName()];
            
            for (int i = 0; i < playersResources.Count(); i++)
            {
                totalResources[i] = playersResources[i] + tempR[i];
            }
            if(p.getName().Equals("P0")){
            System.Console.WriteLine(p.getName() + ":CheckResourceCost:COST:{" + cost[0] + "," + cost[1] + "," + cost[2] + "," + cost[3] + "," + cost[4] + "," + cost[5] + "," + cost[6] + "}");
            System.Console.WriteLine(p.getName() + ":CheckResourceCost:AVAI:{" + totalResources[0] + "," + totalResources[1] + "," + totalResources[2] + "," + totalResources[3] + "," + totalResources[4] + "," + totalResources[5] + "," + totalResources[6] + "}");
            }
            for (int i = 0; i < playersResources.Count(); i++)
            {
                if (totalResources[i] < cost[i]) 
                {
                    return false; 
                }
            }
            


            /*var resources = Enum.GetValues(typeof(Resource));
            foreach (var type in resources)
            {
                int resource = (int)type;

                if (cost[resource] > totalResources[resource])
                    return false;
            }*/
            coinExchange(p);
            resetResources(p);
            return true;
        }

        //returns base+temp resources
        public int[] trArray(PlayerState p)
        {
            int[] totalResources = { 0, 0, 0, 0, 0, 0, 0 };
            List<int> playersResources = baseResources[p.getName()];
            List<int> tempR = TempResources[p.getName()];

            for (int i = 0; i < playersResources.Count(); i++)
            {
                totalResources[i] = (playersResources[i] + tempR[i]);
            }
            return totalResources;
        }

        //returns base+temp+special resources
        public int[] InflatedResourceArray(PlayerState p)
        {
            int[] totalResources = { 0, 0, 0, 0, 0, 0, 0 };
            List<int> playersResources = baseResources[p.getName()];
            List<int> tempR = TempResources[p.getName()];

            for (int i = 0; i < playersResources.Count(); i++)
            {
                totalResources[i] = (playersResources[i] + SpecialResourceArray(p.getName())[i] + tempR[i]);
            }
            return totalResources;
        }

        //returns base+temp+special resources, but can return it for left and right
        public int[] tradableArray(PlayerState p, int lr)
        {
            PlayerState player = null;
            int[] tradeTable = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            switch (lr)
            {
                case 0:
                    return trArray(p);
                case 1:
                    player = gameState.getLeftPlayer(p);
                    break;
                case 2:
                    player = gameState.getRightPlayer(p);
                    break;
            }

            int[] boardResources = player.getBoard().getResources();
            for (int i = 0; i < 7; i++)
            {
                tradeTable[i] = SpecialResourceArray(player.getName())[i]
                              + baseResources[player.getName()][i]
                              + TempResources[player.getName()][i];
            }

            return tradeTable;
        }


    }
}
