using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketBoy
{
    public static class Output
    {
        
        //for keeping title updated
        static String connection;

        public static void error(string error = "You messed this up somehow")
        {
            //use scary red text
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        //SOCKETBOY SPEAKS!
        public static void speak(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void serverText(string message)
        {
            //servers speak in blue
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void status(string message)
        {
            //status is in purple
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        //update client count
        public static void updateTitle(String clientConnection)
        {
            connection = clientConnection;
            setTitle();
        }

        //update all title stats behind the scenes
        static void setTitle()
        {
            String newTitle = "SocketBoy :)";
            if (connection != "[no client]")
                newTitle += " - " + connection;
            Console.Title = newTitle;
        }

    }
}
