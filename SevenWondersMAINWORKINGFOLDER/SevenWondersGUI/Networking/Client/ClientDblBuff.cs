using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Semaphore;
using System.Threading;

namespace SevenWondersGUI
{
    class ClientDblBuff
    {
        private Queue<string> send;
        private Queue<string> receive;
        private object sendLock;
        private object receiveLock;

        public ClientDblBuff()
        {
            send = new Queue<string>();
            receive = new Queue<string>();
            sendLock = new object();
            receiveLock = new object();
        }

        //adds a message to send
        public void addSend(string s)
        {
            lock (sendLock)
            {
                send.Enqueue(s);
            }
        }

        //adds a message to receive
        public void addReceive(string s)
        {
            lock (receiveLock)
            {
                receive.Enqueue(s);
            }
        }

        //checks if send is empty
        public bool sendEmpty()
        {
            if (send.Count == 0)
                return true;
            return false;
        }

        //checks if receive is empty
        public bool receiveEmpty()
        {
            if (receive.Count == 0)
                return true;
            return false;
        }

        //returns the first message from send
        //returns null if send is empty
        public string fromSend()
        {
            string message;
            lock (sendLock)
            {
                try
                {
                    message = send.Dequeue();
                }
                catch (InvalidOperationException e)
                {
                    message = null;
                }
            }
            return message;
        }

        //returns the first message from receive
        //returns null if receive is empty
        public string fromReceive()
        {
            string message;
            lock (receiveLock)
            {
                try
                {
                    message = receive.Dequeue();
                }
                catch (InvalidOperationException e)
                {
                    message = null;
                }
            }
            return message;
        }
    }
}
