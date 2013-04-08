using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWondersGUI
{
    interface WondObserverable
    {
        void subscribe(WondObserver obs);

        void unsubscribe(WondObserver obs);

        void notifyAll();
    }
}
