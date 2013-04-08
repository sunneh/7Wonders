using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{

   
    class StrategyFactory
    {
        /*
        public static readonly int _NULL= 0;
        public static readonly int _RANDOM = 1;
        public static readonly int _ADAPTIVE = 2;
        public static readonly int _AGRESSIVE = 3;
        public static readonly int _CIVILAIN = 4;
        public static readonly int _DISCRETE = 5;
        public static readonly int _COMMERCE = 6;
        public static readonly int _MILITARY = 7;
        public static readonly int _SCIENCE = 8;
        */

        public static readonly int _NULL = 0;
        public static readonly int _HUMAN = 1;
        public static readonly int _RANDOM = 2;
        public static readonly int _AGRESSIVE = 3;
        public static readonly int _CIVILIAN = 4;
        public static readonly int _COMMERCE = 5;
        public static readonly int _DISCRETE = 6;
        public static readonly int _MILITARY = 7;
        public static readonly int _SCIENCE = 8;
        public static readonly int _ADAPTIVE = 9;

        private static StrategyFactory _instance;

        private StrategyFactory() { }

        public static StrategyFactory getInstance()
        {
            if (_instance == null)
                _instance = new StrategyFactory();

            return _instance;
        }

        public GameStrategy getGameStrategy(int strategy)
        {
            if (strategy == _RANDOM)
                return new RandomStrategy();

            if (strategy == _ADAPTIVE)
                return new AdaptiveStrategy();

            if (strategy == _AGRESSIVE)
                return new AggressiveStrategy();

            if (strategy == _CIVILIAN)
                return new CivilianStrategy();

            if (strategy == _COMMERCE)
                return new CommerceStrategy();

            if (strategy == _DISCRETE)
                return new DiscreteStrategy();

            if (strategy == _MILITARY)
                return new MilitaryStrategy();

            if (strategy == _SCIENCE)
                return new ScienceStrategy();

            return null;
        }
    }
}
