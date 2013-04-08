using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{    
    class Player
    {        
        private int _strategy;
        private string _name;

        public Player(int id, int strategy)
        {                       
            _name = (strategy == 1 ? ("P" + id) : ("A" + id));
            _strategy = strategy;
        }

        public string Name ()
        {
            return _name;
        }

        public int Strategy()
        {
            return _strategy;
        }

        public bool isAIPlayer()
        {
            return _strategy > 1 ? true : false;
        }

    }
}
