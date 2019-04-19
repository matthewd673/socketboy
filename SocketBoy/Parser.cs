using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace SocketBoy
{
    public class Parser
    {

        ClientManager manager;

        bool inLoop = false;

        public Parser(ClientManager clientManager)
        {
            manager = clientManager;
        }

        public void parse(String commands, bool script = false)
        {
            String[] lines;

            if (script)
                commands = commands.Replace("\r", "");

            //check if its multiple commands in one
            if (commands.Contains("\n"))
                lines = commands.Split('\n');
            else
                lines = new String[] { commands };

            for(int i = 0; i < lines.Length; i++)
            {
                String input = lines[i];
                //check for a command
                if (input.StartsWith("/connect") && input.Split(' ').Length == 2)
                    manager.createClient(input.Split(' ')[1]);
                else if (input == "/connect")
                    manager.createClient("127.0.0.1:80"); //just a shortcut
                else if (input.StartsWith("/disconnect"))
                    manager.destroyClient();
                else if (input.StartsWith("/hi")) //a relic of the old id system, but not bad enough to remove
                {
                    if (manager.canSend())
                    {
                        if (script)
                            Console.WriteLine("/hi");
                        manager.sendMessage("Hello");
                    }
                    else
                        Output.error("Client cannot greet or send messages");
                }
                else if (input.StartsWith("/wait") && input.Split(' ').Length == 2)
                {
                    int waitTime = Convert.ToInt32(input.Split(' ')[1]);
                    Output.speak("Resuming in " + waitTime + "ms");
                    Thread.Sleep(waitTime);
                }
                else if (input == "/wait")
                {
                    Output.speak("Resuming in 1000ms");
                    Thread.Sleep(1000); //shortcut, wait 1s
                }
                else if (input.StartsWith("/load") && input.Split(' ').Length == 2)
                {
                    //load in a script
                    if (script)
                        Output.speak("Current script is attempting to load another");
                    try
                    {
                        String filename = input.Split(' ')[1];
                        if (File.Exists(filename))
                        {
                            String scriptContents = File.ReadAllText(filename);
                            Parser scriptParser = new Parser(manager);
                            scriptParser.parse(scriptContents, true);
                            Output.speak("Script execution completed");
                        }
                        else
                            Output.error("Specified script file does not exist");
                    }
                    catch (Exception e)
                    {
                        Output.error(e.Message);
                    }
                }
                /*
                else if (input == "/quit")
                {
                    abort = true;
                }
                */
                else if (input == "/help")
                {
                    Output.speak("SocketBoy commands:");
                    Output.speak("/connect [ip address] - connect to a server, port defaults to 80 if none is specified");
                    Output.speak("/disconnect - disconnect from server");
                    Output.speak("/greet [optional: id] - sends client's id to server");
                    Output.speak("/wait [time] - waits for the specified time before continuing script");
                    Output.speak("/load [filename] - loads and executes specified script");
                    Output.speak("If you are connected to a server, type a message with no command to send it");
                }
                else
                {
                    //if connected and no command, send a message
                    if (manager.canSend())
                    {
                        if (script)
                            Console.WriteLine(input);
                        manager.sendMessage(input);
                    }
                    //if not connected and no command you messed up
                    else { Output.error(); }
                }
            }
        }

    }
}
