using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SevenWondersGUI
{
    class PlayerThreadData
    {
        PlayerComm comm;
        Thread playerThread;

        public PlayerThreadData(PlayerComm c, Thread pt)
        {
            comm = c;
            playerThread = pt;
        }

        public PlayerComm getComm()
        {
            return comm;
        }

        public Thread getPlayerThread()
        {
            return playerThread;
        }
    }
}
