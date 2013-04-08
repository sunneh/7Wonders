using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class Logger : Card
    {
        private static Logger _instance = null; 
        
        private static bool _log = true;       
        private static Object _class = null;        
        private static GameState _gameState = null;

        private string _playerName = null;
        private PlayerState player = null; 
        private List<PlayerState> players = null;

        private Logger(Object o, GameState g)
        {
            _class = o; 
            _gameState = g;            
            players = _gameState.getPlayers();       
        }

        public static Logger GetInstance(Object o, GameState g)
        {
            if (_instance == null)
                _instance = new Logger(o, g);
            return _instance;
        }

        public void Reset()
        {
            _instance = null;
        }

        public bool Log(PlayerState p)
        {
            return _log ? (_playerName == null || p.getName() == _playerName) : false;
        }

        public bool Log()
        {
            return _log ? (_playerName == null || player.getName() == _playerName) : false;
        }

        public void SetLogging(bool b)
        {
            _log = b;
        }

        public void LogAll()
        {
           _log = true;
           _playerName = null;
            player = null;
        }

        public void SetGameState(GameState g)
        {
            _gameState = g;
        }

        public void LogPlayer(int index)
        {            
            player = _gameState.getPlayers()[index];
           _playerName = player.getName();
        }

        public void LogPlayer (PlayerState p)
        {
           _playerName = p.getName();
            player = p;
        }

        public void CheckingPlayersCoins(PlayerState p)
        {
            if (Log(p))
                System.Console.WriteLine("[{0}] Player now has {1} coins", _class.GetType().Name, p.getCoins());     
        }

        public void CheckingCoins(PlayerState p, Card c)
        {
            if (Log(p))
                Console.WriteLine("[{0}] Checking to see if we have enough coins: Cost {1} Has {2}"
                                 , _class.GetType().Name
                                 , c.getCoinCost()
                                 , p.getCoins());
        }

        public void CheckCommerceCard(PlayerState p, Card c)
        {
            if (Log(p))
            {
                Console.WriteLine("\n--------------------------------------------------------------------------------------------------------");

                CommerceCard card = (CommerceCard)c;

                int totalResourceCost = c.getTotalResourceCost();
                int[] costs = card.getCost();
                int[] collect = card.getCollect();
                int[] resourceTradable = card.getResourceTradable();

                Console.WriteLine("[{0}] Commerce Card  [{1}] Total Resource Cost [{2}]", _class.GetType().Name
                                  , c.getName()
                                  , totalResourceCost);

                var resources = Enum.GetValues(typeof(Resource));

                // Get the cost and display it
                Console.WriteLine("[Card.getCost()            ] ");
                foreach (var resource in resources)
                {
                    int index = (int)resource;
                    Console.Write("[{0} : {1}] ", ((Resource)index).ToString(), costs[index]);
                }
                Console.WriteLine();

                //0 = none, 1 = resource card, 2 = manufactured good
                int resourceType = card.getResources();
                Console.WriteLine("[Card.getCollect()] RT [{0}] ] ", resourceType);
                // resources = Enum.GetValues(typeof(Resource));             
                for (int index = 0; index < collect.Count(); index++)
                {
                    Console.Write("[{0} : {1}] ", ((Resource)index).ToString(), collect[index]);
                }
                Console.WriteLine();

                // resources = Enum.GetValues(typeof(Resource));
                // Console.WriteLine("[Card.getResourceTradable ({0})] ", card.hasTradableResources());
                for (int index = 0; index < resourceTradable.Count(); index++)
                {
                    Console.Write("[{0} : {1}] ", ((Resource)index).ToString(), resourceTradable[index]);
                }
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------------------------------------------------\n");
            }
        }
    
        public void CheckDictionary(PlayerState p, Dictionary<string, List<int>> dictionary)
        {
            if (Log(p))
            {
                System.Console.WriteLine("\n\n=============================================================================================================");
                System.Console.WriteLine("[{0}] Checking Dictionary", _class.GetType().Name);
                foreach (string key in dictionary.Keys)
                {
                    Console.Write(key);
                    Console.Write(" :: ");
                    List<int> a = dictionary[key];
                    for (int i = 0; i < a.Count; i++)
                        Console.Write(a[i] + ":");
                    Console.WriteLine();
                }
            }
        }

        public void CheckResourceCard(PlayerState p, Card c)
        {
            if (Log(p))
            {
                Console.WriteLine("\n--------------------------------------------------------------------------------------------------------");

                ResourceCard card = (ResourceCard)c;
                int[] resources = card.getResources();
                Console.WriteLine("[{0}] [Card.getResources ({1})] ", _class.GetType().Name, card.hasTradableResources());
                for (int index = 0; index < resources.Count(); index++)
                {
                    Console.Write("[{0} : {1}] ", ((Resource)index).ToString(), resources[index]);
                }
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------------------------------------------------\n");
            }
        }

        public void CheckWonderBoard(PlayerState p)
        {
            if (Log(p))
            {
                Console.WriteLine("\n--------------------------------------------------------------------------------------------------------");

                int[] resources = p.getBoard().getResources();
                Console.WriteLine("[{0}] [Board.getResources ([{1}] [{2}])] ", _class.GetType().Name, p.getName(), p.getBoard().getName());
                for (int index = 0; index < resources.Count(); index++)
                {
                    Console.Write("[{0} : {1}] ", ((Resource)index).ToString(), resources[index]);
                }
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------------------------------------------------\n");
            }
        }

        public void DisplayAllPlayersResources(PlayerState p, Dictionary<string, List<int>> hashtable)
        {
            if (Log(p))
            {
                foreach (KeyValuePair<string, List<int>> entry in hashtable)
                {
                    string player = entry.Key;
                    List<int> resources = entry.Value;
                    Console.Write ("[{0}] Player  [{1}] Has  : ", _class.GetType().Name, p.getName());
                    for (int index = 0; index < resources.Count; index++)
                    {
                        Console.Write("[{0} : {1}] ", ((Resource)index).ToString(), resources[index]);
                    }
                    Console.WriteLine();
                }
            }
        }

        public void DisplayPlayersResources(PlayerState p, Dictionary<string, List<int>> hashtable)
        {
            if (Log(p))
            {
                List<int> playersResources = hashtable[p.getName()];

                Console.Write ("[{0}] Player  [{1}] Has  : ", _class.GetType().Name, p.getName());
                for (int index = 0; index < playersResources.Count; index++)
                {
                    Console.Write("[{0} : {1}] ", ((Resource)index).ToString(), playersResources[index]);
                }
                Console.WriteLine();
            }
        }

        public void DisplayResourceCost(PlayerState p, Card c)
        {
            if (Log(p))
            {
                int[] cardsResourceCost = c.getCost();

                Console.Write("[{0}] [{1}] Card  [{2}] Costs: ", _class.GetType().Name, c.getName(), c.getCost());

                var resources = Enum.GetValues(typeof(Resource));
                foreach (var resource in resources)
                {
                    int index = (int)resource;
                    Console.Write("[{0} : {1:N2}] ", resource, cardsResourceCost[index]);
                }
                Console.WriteLine();
            }
        }

        public void UpdatingPlayersResources(PlayerState p, Card c)
        {
            if (Log(p))
                System.Console.WriteLine( "[{0}] Updating Resources: (Name [{1}] Type [{2}])"
                                        , _class.GetType().Name
                                        , c.GetType().Name
                                        , ((Structure)c.getType()).ToString());
        }

        public void ValidatingCard(PlayerState p, Card c)
        {
            if (Log(p))
            {
                System.Console.WriteLine("\n\n=============================================================================================================");
                System.Console.WriteLine("{0}:: \nValidateCard( Player [{1}] Card [{2}] Coin Cost [{3}] Resource Cost [{4}])"
                                         , _class.GetType().Name
                                         , p.getName()
                                         , c.getName()
                                         , c.getCoinCost()
                                         , c.getTotalResourceCost());
                System.Console.WriteLine("=============================================================================================================");
            }
        }

        public void print(PlayerState p, string msg)
        {
            if (Log(p))
                System.Console.WriteLine("[{0}] [{1}]",_class.GetType().Name,msg);
        }
    }
}
