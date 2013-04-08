using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace SevenWondersGUI
{

    //Creates a timer to control the pace of the game
    class CommTimer
    {
        private System.Timers.Timer timer;
        private ServerMessageQueue mQueue;

        public CommTimer(ServerMessageQueue q)
        {
            mQueue = q;
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        //Starts the timer
        public void startTimer()
        {
            timer.Enabled = true;
        }

        //when timer ticks notifies server when to send a message
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            mQueue.notifyAll();
        }

    }
}
