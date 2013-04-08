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
        private Dictionary<string, List<int>> TempResources = null; //Map of Temporary Resources
        private static GameState gameState = null; 
        private static ResourceManager _instance = null;
        //private int[] coinTransactions;
        //private Logger _logger;

        //Constructor
        private ResourceManager(GameState g)
        {   
            gameState = g;

            baseResources = new Dictionary<string,List<int>>();
            SResources = new Dictionary<string, List<ResourceCard>>();
            TempResources = new Dictionary<string, List<int>>();
            //_logger = Logger.GetInstance(this, g);
            //_logger.SetLogging(true);
            //_logger.LogPlayer(1);

            List<PlayerState> players = gameState.getPlayers();
            for (int i = 0; i < players.Count; i++)
            {               
                
                List<int> boardResources = new List<int>();
                List<int> empty = new List<int>();
                List<ResourceCard> rclist = new List<ResourceCard>();
                
                for (int j = 0; j < 7; j++)
                {
                    boardResources.Add(players[i].getBoard().getResources()[j]);
                    empty.Add(0);

                }

                baseResources.Add(players[i].getName(), boardResources);
                SResources.Add(players[i].getName(), rclist);
                TempResources.Add(players[i].getName(), empty);
            }
        }

        //Instance/GameState Stuff
        public static ResourceManager GetInstance()
        {
            if (_instance == null)
                _instance = new ResourceManager(null);

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

        public void Reset()
        {
            _instance = null;
        }

        public void SetGameState(GameState g)
        {
            gameState = g;
           //_logger.SetGameState(g);
        }

        public GameState getGameState()
        {
            return gameState;
        }

        //Actual Important Functions
        //Checks for precard within the player's hand
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

        //Initial call to check for card validity
        public bool ValidateCard (PlayerState p, Card c)
        {
            //_logger.ValidatingCard(p, c);
            //_logger.CheckDictionary(p,hashtable);

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

        //Initial call to check for wonder validity
        public bool ValidateWonder(PlayerState p)
        {
            return CheckResourceCost(p, p.getBoard().getBuildCost(p.getBoard().getCurrentWonderLevel()));
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

        public Resources GetCombinedResources(PlayerState p)
        {
            List<int> l = new List<int>();
            List<int> r = new List<int>();
            List<int> c = new List<int>();

            for (int i = 0; i < 7; i++) 
            {
                l.Add(baseResources[gameState.getLeftPlayer(p).getName()][i] + TempResources[gameState.getLeftPlayer(p).getName()][i]);
                r.Add(baseResources[gameState.getRightPlayer(p).getName()][i] + TempResources[gameState.getRightPlayer(p).getName()][i]);
                c.Add(baseResources[p.getName()][i] + TempResources[p.getName()][i]);
            }
            return new Resources(l.ToArray(), r.ToArray(), c.ToArray(), p.getName());
        }

        //Resets the Special Resource Card List
        public void resetSpecialResources(PlayerState p)
        {
            resetSpecialResourceSingular(p);
            resetSpecialResourceSingular(gameState.getLeftPlayer(p));
            resetSpecialResourceSingular(gameState.getRightPlayer(p));
        }


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

        public void usingSpecialResource(PlayerState p, int resourceIndex) {
            //Can't allocate resource values higher then type 7
            if (resourceIndex > 6) { return; }

            int count = SResources[p.getName()].Count;
            List<int> playersResources = baseResources[p.getName()];
            //Cycle through list of special resource cards and look for cards with said resources
            for (int i = 0; i < count ; i++)
            {                
                if (SResources[p.getName()][i].getResources()[resourceIndex] > 0)
                {
                    TempResources[p.getName()][resourceIndex]++;
                    System.Console.WriteLine("REMOVING " + SResources[p.getName()][i].getName() + " FROM " + p.getName());
                    ResourceCard rc = SResources[p.getName()][i];
                    int[] resources = rc.getResources();

                    for (int index = 0; index < resources.Count(); index++)
                    {
                        playersResources[index] = playersResources[index] + resources[index];
                    }
                    SResources[p.getName()].RemoveAt(i);
                }
            }
            baseResources[p.getName()] = playersResources;
        }

        private void UpdateResources(PlayerState p, Card c)
        {
           //_logger.UpdatingPlayersResources(p,c);
           //_logger.DisplayResourceCost(p,c);

            if (c is CommerceCard)
            {
                KensUtilityfunctions k = new KensUtilityfunctions();
                int income = k.calcCommerceIncome(c, gameState, p.getSeatNumber());
                p.updateCoins(income);                               
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
                        System.Console.WriteLine(p.getName() + " is adding Special Resource Card: {" + ((ResourceCard)c).getResources()[0] + "," + ((ResourceCard)c).getResources()[1] + "," + ((ResourceCard)c).getResources()[2] + "," + ((ResourceCard)c).getResources()[3] + "," + ((ResourceCard)c).getResources()[4] + "," + ((ResourceCard)c).getResources()[5] + "," + ((ResourceCard)c).getResources()[6] + "}");
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
                        System.Console.WriteLine(p.getName() +" is adding: {" + tradableResources[0] + "," + tradableResources[1] + "," + tradableResources[2] + "," + tradableResources[3] + "," + tradableResources[4] + "," + tradableResources[5] + "," + tradableResources[6] + "}");
                        baseResources[p.getName()] = current;
                        
                        //_logger.DisplayPlayersResources(p, hashtable);
                    }
                }
                return;
            }
           //_logger.CheckDictionary(p,hashtable);
           //_logger.DisplayPlayersResources(p, hashtable);
        }

        public int[] tradableArray(PlayerState p, int lr) {
            int[] tradeTable = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 7; i++)
            {
                if (lr == 0)//Left
                {
                    tradeTable[i] = SpecialResourceArray(gameState.getLeftPlayer(p).getName())[i] + 
                        baseResources[gameState.getLeftPlayer(p).getName()][i] + 
                        TempResources[gameState.getLeftPlayer(p).getName()][i];
                }
                if (lr == 1)//Right
                {
                    tradeTable[i] = SpecialResourceArray(gameState.getRightPlayer(p).getName())[i] +
                        baseResources[gameState.getRightPlayer(p).getName()][i] +
                        TempResources[gameState.getRightPlayer(p).getName()][i];
                }
            }
            return tradeTable;
        }

        public void tradeTo(PlayerState p, int resourceIndex, int player){
            //player = 0, left, player = 1, right
            if (resourceIndex > 6)
            {
                System.Console.WriteLine("resourceIndex over 6");
                return;
            }
            //1. Check for resource in base
            
            //2. Then check resource in SR
            //3. 
        }

        //Creates an int[7] of special resources from player name
        public int[] SpecialResourceArray(string s) {
            int[] specialResourceTable = new int[7]{0,0,0,0,0,0,0};
            for (int i = 0; i < SResources[s].Count; i++) {
                for (int j = 0; j < 7; j++) { specialResourceTable[j] += SResources[s][i].getResources()[j]; }
            }
            return specialResourceTable;
        }

        //Returns true if the player can afford the cost
        public bool CheckResourceCost(PlayerState p, int[] cost)
        {
            int[] totalResources = { 0, 0, 0, 0, 0, 0, 0 };
            List<int> playersResources = baseResources[p.getName()];
            List<int> tempR = TempResources[p.getName()];

            for (int i = 0; i < playersResources.Count(); i++)
            {
                totalResources[i] = (playersResources[i] + tempR[i]);
            }

            var resources = Enum.GetValues(typeof(Resource));
            foreach (var type in resources)
            {
                int resource = (int)type;
                if (cost[resource] > totalResources[resource])
                    return false;
            }
            return true;
        }
    }

}
