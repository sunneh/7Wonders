/**********************************************************************
 * 
 *   
 * 
 * Synchronizes the timing of the transmissions to the games
 * Uses objserver parttern to update the players with the number
 * if connected players
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace SevenWondersGUI
{
    class ServerMessageQueue : WondObserverable
    {
        private Object timingLock;
        private Object joinLock;
        private object subLock;
        private object notifyLock;
        private int players;
        private string[] messages;
        private byte[][] messagesByte;
        private int messagesRec;
        private int messRec;
        private int messagesRel;
        private int turn;
        private bool started;
        private LinkedList<WondObserver> obsList;
        //public event EventHandler playersUpdated;

        public ServerMessageQueue()
        {
            players = 0;
            joinLock = new Object();
            timingLock = new Object();
            subLock = new object();
            notifyLock = new object();
            messagesRec = 0;
            messRec = 0;
            messagesRel = 0;
            started = false;
            turn = 0;
            obsList = new LinkedList<WondObserver>();
            //playersUpdated
        }

        public bool isStarted()
        {
            return started;
        }

        public bool startGame()
        {
            started = true;
            return started;
        }

        public int getPlayers()
        {
            return players;
        }

        public bool isGameReady()
        {
            if ((messagesRec == players)
                && (messagesRec != 0))
                return true;
            return false;
        }

        //return the number of messages received
        public int getMessagesRec()
        {
            return messagesRec;
        }

        public string[] getMessages()
        {
            lock (timingLock)
            {
                messagesRel++;
                if (messagesRel == players)
                {
                    messagesRel = messagesRec = 0;
                    turn++;
                }
                return messages;
            }
        }
        
        //registers the observers
        public void subscribe(WondObserver obs)
        {
            lock (subLock)
            {

                obsList.AddFirst(obs);
                Console.WriteLine("added subscriber");
            }
        }

        //unregisters the observers
        public void unsubscribe(WondObserver obs)
        {
            obsList.Remove(obs);
        }

        public void notifyAll()
        {
            Console.WriteLine("turn: " + turn + " rec " + messagesRec + " rel " + messagesRel);
            lock (subLock)
            {

                //enumeration the observers and invoke their notify method
                foreach (WondObserver obs in obsList)
                {
                    obs.update(new object());
                }//foreach
            }
        }

        //adds player and increases the size of the message array
        public int addPlayer()
        {
            lock (joinLock)
            {
                if (players >= 7)
                    return -1;
                messages = new string[++players];
                messagesByte = new byte[players][];
                return players;
            }
        }

        //adds messages too the messages queue
        public void addMessage(string message, int id)
        {
            lock (joinLock)
            {
                messages[id-1] = message;
                messagesRec++;

                /*
                if (messagesRec == players)
                {
                    Monitor.Pulse(timingLock); //Signals server 
                } */
            }
        }
        //takes in messages and holds the thread and message until all the players
        //have played
        public string[] passMessages(string inMessage)
        {
            
            lock (timingLock)
            {
                messages[messagesRec++] = inMessage;
                while (messagesRec < players)
                    Monitor.Wait(timingLock);
                Monitor.PulseAll(timingLock);
            }

            if (++messagesRel >= messagesRec)
            {
                messagesRec = messagesRel = 0;
            }

            return messages;
        }

        //takes in messages and holds the thread and message until all the players
        //have played
        public string[] passMessages()
        {

            lock (timingLock)
            {
                messRec++;
                while (messRec < players)
                    Monitor.Wait(timingLock);
                Monitor.PulseAll(timingLock);
            }

            if (++messagesRel >= messRec)
            {
                messagesRec = messagesRel = messRec = 0;
                turn++;
            }

            return messages;
        }

        //same as passMessages but uses byte arrays
        public byte[][] passMessagesByte(byte[] inMessage)
        {

            lock (timingLock)
            {
                messagesByte[messagesRec++] = inMessage;
                while (messagesRec < players)
                    Monitor.Wait(timingLock);
                Monitor.PulseAll(timingLock);
            }

            if (++messagesRel >= messagesRec)
            {
                messagesRec = messagesRel = 0;
            }

            return messagesByte;
        }
    }
}
