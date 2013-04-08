using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace SevenWondersGUI
{
    class PlayerComm : WondObserver
    {
        //PlayerThreadData passedInfo;// = (ToPass)thePlayer;
        private TcpClient player; // = passedInfo.getTcp();
        private ServerMessageQueue messages; // = passedInfo.getMQueue();
        private NetworkStream playerStream; // = player.GetStream();
        private int messageSize;
        private byte[] receivedBuff;
        private byte[] sendBuff;
        private int bytesRead;
        private int playerID;
        private ASCIIEncoding encoder;
        private bool obsChanged;
        private object joinLock;

        public PlayerComm(TcpClient p, ServerMessageQueue smq, int mess, int pn)
        {
            player = p;
            messages = smq;
            playerStream = player.GetStream();
            messageSize = mess;
            encoder = new ASCIIEncoding();
            playerID = pn;
            obsChanged = false;
            joinLock = new object();
        }

        public void handlePlayer()
        {
            joinLoop();
            gameLoop();
        }

        //runs the game loop for each player
        public void gameLoop()
        {
            string[] outMess;
            byte[][] outMessByte;
            //BinaryFormatter formatter = new BinaryFormatter();

            while (true)
            {
                waitForUpdate();

                if (messages.isGameReady())
                {
                    sendBuff = encoder.GetBytes("Ready");
                    playerStream.Write(sendBuff, 0, sendBuff.Length);
                    outMess = messages.passMessages();

                    for (int i = 0; i < messages.getPlayers(); i++)
                    {
                        Thread.Sleep(15);
                        sendBuff = encoder.GetBytes(outMess[i]);
                        playerStream.Write(sendBuff, 0, sendBuff.Length);
                    }
                }
                else
                {
                    sendBuff = encoder.GetBytes("Game Waiting");
                    Console.WriteLine("Game Waiting");
                    playerStream.Write(sendBuff, 0, sendBuff.Length);
                }


                bytesRead = 0;
                receivedBuff = new byte[messageSize];
                try
                {
                    bytesRead = playerStream.Read(receivedBuff, 0, messageSize);
                }
                catch
                {
                    break;
                }
                string strReceived = encoder.GetString(receivedBuff, 0, bytesRead);
                Console.WriteLine(strReceived);

                if (!strReceived.Equals("Game Waiting"))
                {
                    messages.addMessage(strReceived, playerID);
                }
            }
        }

        //runs the join loop and waits for players
        public void joinLoop()
        {
            string response = "";

            sendBuff = encoder.GetBytes(playerID + "");
            playerStream.Write(sendBuff, 0, sendBuff.Length);

            while (!messages.isStarted())
            {
                receivedBuff = new byte[messageSize];
                waitForUpdate();
                if (!messages.isStarted()) //sends number of players 
                {
                    if (response.Equals("Start"))
                    {
                        messages.startGame();
                        Console.WriteLine("start game sent");
                    }
                    sendBuff = encoder.GetBytes("" + messages.getPlayers());
                    playerStream.Write(sendBuff, 0, sendBuff.Length);
                    Thread.Sleep(10);
                    Console.WriteLine("players: " + messages.getPlayers());

                    playerStream.Flush();

                    try
                    {
                        bytesRead = playerStream.Read(receivedBuff, 0, messageSize);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.ReadKey();
                    }

                    response = encoder.GetString(receivedBuff, 0, bytesRead);
                    Console.WriteLine("rcv: " + response);

                }
            }
            sendBuff = encoder.GetBytes("Started"); // 8 means game has started
            playerStream.Write(sendBuff, 0, sendBuff.Length);
        }

        public void waitForUpdate()
        {
            lock (joinLock)
            {
                //while (!obsChanged)
                //{
                    Monitor.Wait(joinLock);
                //}
                obsChanged = false;
                Thread.Sleep(50);
            }
            //Console.WriteLine("player released");
        }

        public void update(object obj)
        {
            //Console.WriteLine("in update");

            lock (joinLock)
            {
                obsChanged = true;
                Monitor.PulseAll(joinLock);
            }
        }
    }
}
