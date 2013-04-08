using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace SevenWondersGUI
{
    class WondersServer : WondObserver
    {
        private TcpListener listenSocket;
        private Thread listenThread;
        private int port;
        private PlayerThreadData[] playersList;
        private byte[] full;
        private const int messageSize = 1024 * 1024;
        private ServerMessageQueue mQueue;
        private object serverLock;
        private TcpClient player;
        private bool change;
        private CommTimer timer;
        
        //constructor creates the socket and threads to listen to new connections
        public WondersServer(int p) 
        {
            playersList = new PlayerThreadData[7];
            port = p;
            listenSocket = new TcpListener(IPAddress.Any, port);
            mQueue = new ServerMessageQueue();
            ASCIIEncoding encoder = new ASCIIEncoding();
            full = encoder.GetBytes("full");
            serverLock = new object();
            change = false;
            timer = new CommTimer(mQueue);
        }

        //entry point for the server to 
        public void serverStart()
        {
            listenThread = new Thread(new ThreadStart(listenForPlayers));
            listenThread.Start();
            timer.startTimer();
            while (!mQueue.isStarted())
            {
                serverWait();
            }
            listenSocket.Server.Close(); //stops socket from listening and allows thread to close
            listenThread.Join();
            Console.WriteLine("listen Thread Joined");
            while (mQueue.isStarted())
            {
                serverWait();
            }
        }

        //listens for new connections and creates a new thread to handle communication
        private void listenForPlayers(){
            PlayerComm playComm;
            Thread playerThread;

            listenSocket.Start();
            //PlayerThreadData tp; 
            do
            {
                if (mQueue.isStarted())
                    return;

                player = listenSocket.AcceptTcpClient();
                int playerNum = mQueue.addPlayer();
                if (playerNum < 0)
                {
                    player.GetStream().Write(full, 0, full.Length);
                    continue;
                }

                playComm = new PlayerComm(player, mQueue, messageSize, playerNum);
                mQueue.subscribe(playComm);
                //mQueue.notifyAll();
                playerThread = new Thread(new ThreadStart(playComm.handlePlayer));
                playerThread.Start();
                playersList[mQueue.getPlayers() - 1] = new PlayerThreadData(playComm, playerThread);


                /*
            tp = new ToPass(player, mQueue); 
            Thread playerThread = new Thread(new ParameterizedThreadStart(handlePlayer));
            playerThread.Start(tp); */
            } while (true);
        }

        public void serverWait()
        {
            lock (serverLock)
            {
                while (!change)
                {
                    Monitor.Wait(serverLock);
                }
                change = true;
            }
        }

        public void update(object obj)
        {
            Console.WriteLine("in update");

            lock (serverLock)
            {
                change = true;
                Monitor.PulseAll(serverLock);
            }
        }
        /*
        static void Main(string[] args)
        {
            
            WondersServer server = new WondersServer(3004);
            server.serverStart();
        }*/
    }
}
