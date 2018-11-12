using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace SocketBoy
{
    public class ClientManager
    {

        //public List<SocketClient> clients = new List<SocketClient>();

        SocketClient client;

        Encoding encoding;

        public ClientManager()
        {
            encoding = Encoding.ASCII;
        }

        public void setEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public void createClient(String address)
        {
            string ip = address;
            int port = 80;

            //if a port is specified, parse it
            if (address.Contains(":"))
            {
                ip = address.Split(':')[0];
                if (ip == "")
                    ip = "localhost"; //just a quick shortcut
                port = Convert.ToInt32(address.Split(':')[1]);
            }

            try
            {
                //try to connect to ip, then tell the user what happened
                if (clientExists())
                    destroyClient();

                client = new SocketClient(ip, port);
                Output.speak("Client created");

                if (client.tcp.Connected)
                    Output.speak("Connected to " + clientAddress());
                Output.updateTitle(clientConnection: clientAddress()); //update title
            }
            catch (Exception e)
            {
                Output.error(e.Message);
            }
        }

        public void disconnectClient()
        {
            if(clientExists())
            {
                String formerAddress = clientAddress();
                client.tcp.Close();
                Output.speak("Disconnected from " + formerAddress);
            }
        }

        public void destroyClient()
        {
            if (clientExists())
            {
                client.tcp.Close();
                client = null;
                Output.speak("Connection to " + clientAddress() + " destroyed");
            }
            else
                Output.error("There are is no connection to destroy");
            Output.updateTitle(clientConnection: clientAddress()); //update title
        }

        public void sendMessage(string message)
        {
            byte[] sendIt = encoding.GetBytes(message);

            bool success = true;
            try
            {
                client.stream.Write(sendIt, 0, sendIt.Length);
            }
            catch (Exception e)
            {
                Output.error(e.Message);
                success = false;
            }

            if (success)
            {
                //append status to end of last line
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(message); //what the user had there previously;
                Output.status(" [sent]");
            }
        }

        //canSend() is for when no specific client is identified to send a message
        public bool canSend()
        {
            if (!clientExists()) //is there a client?
            {
                Output.error("Cannot send: no client");
                return false;
            }
            if (!client.tcp.Connected) //is the client connected?
            {
                Output.error("Cannot send: client is not connected");
                return false;
            }
            return true;
        }

        public bool clientExists()
        {
            return (client != null);
        }

        public String clientAddress()
        {
            if (clientExists())
                return client.ip + ":" + client.port;
            else
                return "[no client]";
        }

    }
}
