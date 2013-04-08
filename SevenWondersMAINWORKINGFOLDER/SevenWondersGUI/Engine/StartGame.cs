using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    class StartGame
    {
        GameState game;
        int numPlayers = 0;

        List<Card> allCards = new List<Card>();
        List<Card> cards    = new List<Card>();
        List<Board> boards  = new List<Board>();
        List<PlayerState> players = new List<PlayerState>();
        
        public StartGame(List<Player> pending)
        {
            numPlayers = pending.Count;
            
            cardlistCreator lstC = new cardlistCreator();
            allCards = lstC.getCardList();

            cards = filterCards(allCards);

            boards = new CreateBoards().getBoards();
            makePlayers(pending);

            game = new GameState(cards, players, boards);
           
            ResourceManager.GetInstance(game);//

        }

        private void makePlayers(List<Player> pending)
        {           
            for (int i = 0; i < pending.Count; i++)
            {
                if (pending[i].isAIPlayer())
                {
                    AIPlayer aip = new AIPlayer(pending[i].Name(), i);
                    aip.setStrategy(pending[i].Strategy());
                    players.Add(aip);
                }
                else
                {
                    players.Add(new PlayerState(pending[i].Name(),i));
                }
            }
        }

        public GameState getGame()
        {
            return game;
        }
        
        //method that returns only the list of cards needed for the amount of players
        private List<Card> filterCards(List<Card> lst)
        {
            List<Card> local = new List<Card>();
            foreach (Card card in lst)
            {
                if (card.getPlayerCount() <= numPlayers)
                {
                    local.Add(card);
                }
            }
            return local;
        }

    }
}
