using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SevenWondersGUI
{
    class ClientComm
    {
        private const int messageSize = 1024 * 1024;
        private int playerId;
        private ASCIIEncoding encoder;
        private TcpClient clientSocket;
        private ClientDblBuff commBuffer;
        private byte[] receiveBuf;
        private byte[] sendBuf;
        private NetworkStream clientStream;
        private string received;
        private string send;
        private int bytesRead;
        private int players;

        //creates the communication thread
        public ClientComm(string ip, ClientDblBuff CB)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), 3004);
            clientSocket = new TcpClient();
            clientSocket.Connect(serverEndPoint);
            commBuffer = CB;
            receiveBuf = new byte[messageSize];
            clientStream = clientSocket.GetStream();
            players = 0;
            encoder = new ASCIIEncoding();
        }

        //entry point for the commclient receives playerId
        public void ClientRun()
        {
            bytesRead = clientStream.Read(receiveBuf, 0, messageSize);
            received = encoder.GetString(receiveBuf, 0, bytesRead);
            Console.WriteLine(received);
            commBuffer.addReceive(received);
            if (received.Equals("full"))
            {
                return;
            }
            playerId = int.Parse(received);

            clientJoinLoop();

        }

        //runs the join loop for the client
        private void clientJoinLoop(){

            while (!received.Equals("Started"))
            {
                bytesRead = clientStream.Read(receiveBuf, 0, messageSize);
                received = encoder.GetString(receiveBuf, 0, bytesRead);
                Console.WriteLine(received);
                Thread.Sleep(10);

                if (received.Equals("Started"))
                {
                    break;
                }
                if (int.Parse(received) != players)
                {
                    commBuffer.addReceive(received);
                    players = int.Parse(received);
                }


                if (!commBuffer.sendEmpty())
                {
                    send = commBuffer.fromSend();
                }
                else
                {
                    send = "waiting";
                }
                Console.WriteLine("sending " + send);
                sendBuf = encoder.GetBytes(send);
                clientStream.Write(sendBuf, 0, sendBuf.Length);
            }
        }


        //runs the game loop taking data from the send queue and putting 
        //data into the receive queue
        private void clientGameLoop()
        {
            do
            {
                receiveBuf = new byte[messageSize];
                clientStream.Flush();
                try
                {
                    bytesRead = clientStream.Read(receiveBuf, 0, messageSize);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return;
                }
                received = encoder.GetString(receiveBuf, 0, bytesRead);

                if (received.Equals("Ready"))
                {
                    for (int j = 0; j < players; j++)
                    {
                        try
                        {
                            bytesRead = clientStream.Read(receiveBuf, 0, messageSize);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            return;
                        }
                        received = encoder.GetString(receiveBuf, 0, bytesRead);
                    }
                }

                if (!commBuffer.sendEmpty())
                {
                    send = commBuffer.fromSend();
                }
                else
                {
                    send = "Game Waiting";
                }
                sendBuf = encoder.GetBytes(send);
                Console.WriteLine("send " + send + " " + playerId);
                clientStream.Write(sendBuf, 0, sendBuf.Length);

            } while (!received.Equals("Game Over"));
        }
        /*
        static void Main(string[] args)
        {
        }*/
    }
}
