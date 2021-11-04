using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace RemoteKey
{
    public class CommandHandler
    {
        private static Dictionary<string, Type> CommandDict = new Dictionary<string, Type>()
        {
            { "connect", typeof(ConnectCommand) },
            { "debug", typeof(DebugCommand) },
            { "bridge", typeof(BridgeCommand) }
        };

        public static Command Handle(string command)
        {
            var split = command.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            var name = split[0];
            Type type = null;
            CommandDict.TryGetValue(name.ToLower(), out type);

            if (type == null)
            {
                throw new CommandNotFoundException();
            }
            var content = new Command(split.ToList().Skip(1).ToArray());
            try {
                var exec = type.GetConstructor(new Type[]{ typeof(Command) }).Invoke(new object[]{ content }) as AbstractCommand;
                exec.Execute();
            }
            catch(TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            }
            return content;
        }
    }
}