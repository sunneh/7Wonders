using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    public class ResourceManager : Card
    {               
        private Dictionary<string,List<int>> hashtable = null;
        
        private static GameState gameState = null; 
        private static ResourceManager _instance = null;
        private bool boardRAdded = false;

        //private Logger _logger;

        private ResourceManager(GameState g)
        {            
            gameState = g;
            hashtable = new Dictionary<string,List<int>>();

            //_logger = Logger.GetInstance(this, g);
            //_logger.SetLogging(true);
            //_logger.LogPlayer(1);

            List<PlayerState> players = gameState.getPlayers();
            for (int i = 0; i < players.Count; i++)
            {               
                List<int> list = new List<int>();
                for (int j = 0; j < 7; j++)
                    list.Add(0);                
                hashtable.Add(players[i].getName(), list);
            }
        }

        public static ResourceManager GetInstance()
        {
            if (_instance == null)
                _instance = new ResourceManager(null);

            Console.WriteLine("================= YEA WERE COOL ===================");

            return _instance;
        }

        public static ResourceManager GetInstance(GameState g)
        {
            if (_instance == null)
                _instance = new ResourceManager(g);
            gameState = g;

            Console.WriteLine("================= YEA WERE COOL - SET GAME STATE ===================");
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

        public bool ValidateCard (PlayerState p, Card c)
        {
           //_logger.ValidatingCard(p, c);
           //_logger.CheckDictionary(p,hashtable);

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
                return CheckResourceCost(p,c);
            }

            return false;
        }

        public Resources GetResources(PlayerState p)
        {
            List<int> l = hashtable[gameState.getLeftPlayer(p).getName()];
            List<int> r = hashtable[gameState.getRightPlayer(p).getName()];
            List<int> c = hashtable[p.getName()];

            // Get the wonder boards default resources
            //_logger.CheckWonderBoard(p);
            if (boardRAdded == false)
            {
                int[] br = p.getBoard().getResources();
                for (int i = 0; i < br.Count(); i++)
                    c[i] = (br[i] + c[i]);
                boardRAdded = true;
            }
            
            return new Resources(l.ToArray(), r.ToArray(), c.ToArray(), p.getName());       
        }

        private void UpdateResources(PlayerState p, Card c)
        {
           //_logger.UpdatingPlayersResources(p,c);
           //_logger.DisplayResourceCost(p,c);

            if (c is CommerceCard)
            {
                //KensUtilityfunctions k = new KensUtilityfunctions();
                //int income = k.calcCommerceIncome(c, gameState, p.getSeatNumber());
                //p.updateCoins(income); 
                if (((CommerceCard)c).hasTradableResources())
                {
                   //_logger.DisplayPlayersResources(p, hashtable);
                   //_logger.CheckCommerceCard(p,c);
                    List<int> current = hashtable[p.getName()]; 
                    int[] tradableResources = ((CommerceCard)c).getResourceTradable();
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
                        }
                    }
                    hashtable[p.getName()] = current;

                   //_logger.DisplayPlayersResources(p, hashtable);
                }                               
                return;
            }

            if (c is ResourceCard)
            {
               //_logger.CheckResourceCard(p,c);
               //_logger.DisplayPlayersResources(p, hashtable);
                
                List<int> current = hashtable[p.getName()];

                if (((ResourceCard)c).hasTradableResources())
                {
                    int[] tradableResources = ((ResourceCard)c).getResources();
                    for (int index = 0; index < tradableResources.Count(); index++)
                    {
                        switch (index)
                        {
                            case (int)Resource.Wood:
                                current[(int)Resource.Wood]  += tradableResources[index];
                                break;
                            case (int)Resource.Stone:
                                current[(int)Resource.Stone] += tradableResources[index];
                                break;
                            case (int)Resource.Clay:
                                current[(int)Resource.Clay]  += tradableResources[index];
                                break;
                            case (int)Resource.Ore:
                                current[(int)Resource.Ore]   += tradableResources[index];
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
                    hashtable[p.getName()] = current;
                   //_logger.DisplayPlayersResources(p, hashtable);
                }
                return;
            }
           //_logger.CheckDictionary(p,hashtable);
           //_logger.DisplayPlayersResources(p, hashtable);
        }

        public bool CheckResourceCost(PlayerState p, Card c)
        {
            int[] totalResources = { 0, 0, 0, 0, 0, 0, 0 };

            int[] cost = c.getCost();
            int[] boardResources = p.getBoard().getResources();
            List<int> playersResources = hashtable[p.getName()];

            for (int i = 0; i < playersResources.Count(); i++)
            {
                totalResources[i] = (playersResources[i] + boardResources[i]);
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
