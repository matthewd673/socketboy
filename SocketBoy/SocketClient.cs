using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketBoy
{
    public class SocketClient
    {
        public TcpClient tcp;
        public NetworkStream stream;
        public String id;

        public String ip;
        public int port;

        public Thread listenThread;

        Random rng = new Random();

        public SocketClient(String ip, int port)
        {
            tcp = new TcpClient(ip, port);
            stream = tcp.GetStream();
            this.ip = ip;
            this.port = port;
        }
    }
}
