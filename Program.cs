using System;

namespace RemoteKey
{
    class Program
    {
        
        public static bool Debug = false;

        static void Main(string[] args)
        {
            Console.WriteLine("RemoteKey by zhufucdev");
            Command lastResult = null;
            while (true)
            {
                // Display the exit code of last command if not successful
                if (lastResult != null && !lastResult.IsSuccessful)
                {
                    Console.Write(string.Format("{0}> ", lastResult.ExitCode));
                }
                else
                {
                    Console.Write("> ");
                }
                // Handle current command
                var line = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(line))
                {
                    // skip empty command
                    continue;
                }

                // exit
                if (line == "exit")
                {
                    break;
                }

                try
                {
                    lastResult = CommandHandler.Handle(line);
                }
                catch(CommandNotFoundException _)
                {
                    Log(TAG_WARN, "Command not found");
                }
                catch(Exception e)
                {
                    lastResult = null;
                    Log(TAG_ERROR, "Failed to execute this command");
                    Log(TAG_ERROR, string.Format("::{0} {1}", e.GetType().Name, e.Message));
                    if (Debug)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }
        }

        public static void Log(string tag, object content)
        {
            Console.WriteLine(string.Format("{0}: {1}", tag, content));
        }

        public const string TAG_ERROR = "ERROR";
        public const string TAG_WARN = "WARNING";
    }
}
