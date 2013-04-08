using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    interface GameStrategy
    {
        Card getNextCard (PlayerState p,List<Card> hand);
        Card discardCard (PlayerState p, List<Card> hand);
        string Name();
    }
}
