using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    [Serializable()]
    class NetMessage
    {
        private string cardId;
        private int playerId;
        private int coins;

        NetMessage(string n, int pi, int c)
        {
            cardId = n;
            playerId = pi;
            coins = c;
        }

        public string getCardId()
        {
            return cardId;
        }

        public int getPlayerId()
        {
            return playerId;
        }

        public int getCoins()
        {
            return coins;
        }

        public override string ToString()
        {
            return cardId + " " + playerId + " " + coins;
        }
    }
}
