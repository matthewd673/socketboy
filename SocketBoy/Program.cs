using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SocketBoy
{
    class Program
    {

        static TcpClient client;
        static NetworkStream clientStream;

        static void Main(string[] args)
        {

            speak("Hello, my name is SocketBoy :)");
            speak("I'm here to fix your horrible server!");

            bool abort = false;
            bool connected = false;
            
            //loop until closed
            while(!abort)
            {

                //constantly check if client is connected
                if (client != null)
                    connected = client.Connected;
                else
                    connected = false;

                //read from server on a new thread
                new Thread(new ThreadStart(readFromServer)).Start();

                string input = Console.ReadLine();

                //check for a command
                if (input.StartsWith("/connect") && input.Split(' ').Length == 2)
                {
                    createClient(input.Split(' ')[1]);
                }
                else if (input.StartsWith("/disconnect"))
                {
                    client = null;
                    clientStream = null;
                    speak("Disconnected from server");
                }
                else if (input == "/quit")
                {
                    abort = true;
                }
                else if (input == "/help")
                {
                    speak("SocketBoy Commands:");
                    speak("/connect [ip address] - connect to a server, port defaults to 80 if none is specified");
                    speak("/disconnect - disconnect from server");
                    speak("/quit - stop everything, then press any key to quit");
                    speak("If you are connected to a server, type a message with no command to send it");
                }
                else
                {
                    //if connected and no command, send a message
                    if (connected)
                    {
                        sendOnStream(input);
                    }
                    //if not connected and no command you messed up
                    else { error(); }
                }

            }

            speak("Goodbye!");
            Console.ReadKey();

        }

        static void createClient(string ip)
        {
            string pureIp = ip;
            int port = 80;

            //if a port is specified, parse it
            if (ip.Contains(":"))
            {
                pureIp = ip.Split(':')[0];
                if (pureIp == "")
                    pureIp = "localhost"; //just a quick shortcut
                port = Convert.ToInt32(ip.Split(':')[1]);
            }

            try
            {
                //try to connect to ip, then tell the user what happened
                client = new TcpClient(pureIp, port);
                clientStream = client.GetStream();

                speak("Connected? " + client.Connected);
            }
            catch (Exception e)
            {
                error(e.Message);
            }

        }

        static void sendOnStream(string message)
        {
            //write message to stream
            byte[] sendIt = ASCIIEncoding.ASCII.GetBytes(message);
            speak("Sending");
            clientStream.Write(sendIt, 0, sendIt.Length);
        }

        static void error(string error = "You messed this up somehow")
        {
            //use scary red text
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void readFromServer()
        {
            //make sure client and stream exist and are connected
            if (client != null && clientStream != null)
            {
                if (client.Connected)
                {
                    //await a response
                    byte[] data = new Byte[256];

                    String response = "";

                    //turn response into text
                    Int32 bytes = clientStream.Read(data, 0, data.Length);
                    response = Encoding.ASCII.GetString(data, 0, bytes);

                    //speak the response
                    serverText(response);
                }
            }
        }

        //SOCKETBOY SPEAKS!
        static void speak(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void serverText(string message)
        {
            //servers speak in blue
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}
