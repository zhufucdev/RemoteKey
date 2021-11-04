using System;
using System.Net;

namespace RemoteKey
{
    public class BridgeCommand : AbstractCommand
    {
        // Op: Start
        public int Port = -1;
        // Op: Connect
        public string Address, Password;
        public Operation Op;
        public BridgeCommand(Command command) : base(command)
        {
            var operationStr = command.Arguments[0];
            if (operationStr.Equals("start", System.StringComparison.OrdinalIgnoreCase))
            {
                // Only when the server hasn't been initialized
                // do we need port as a parameter
                if (!BridgeServer.Initialized && command.Arguments.Length < 2)
                {
                    throw new ArgumentException("pass one more argument for port");
                }
                
                Op = Operation.START;
            }
            else if (operationStr.Equals("stop", System.StringComparison.OrdinalIgnoreCase))
            {
                Op = Operation.STOP;
            }
            else if (operationStr.Equals("connect", StringComparison.OrdinalIgnoreCase))
            {
                if (command.Arguments.Length < 3)
                {
                    throw new ArgumentException("pass second and third argument for remote url and password");
                }
                Op = Operation.CONNECT;
                Address = command.Arguments[1];
                Password = command.Arguments[2];
            }
            else
            {
                throw new ArgumentException("pass Start, Stop or Connect as the first parameter");
            }
        }

        public override Command Execute()
        {
            switch (Op)
            {
                case Operation.START:
                    if (BridgeServer.IsListening)
                    {
                        // fails if already started
                        Content.ExitCode = 128;
                    }
                    else
                    {
                        if (!BridgeServer.Initialized)
                        {
                            BridgeServer.Initialize(Port);
                        }
                        BridgeServer.Start();
                    }
                    break;
                case Operation.STOP:
                    if (!BridgeServer.IsListening)
                    {
                        // fails if not started
                        Content.ExitCode = 128;
                    }
                    else
                    {
                        BridgeServer.Stop();
                    }
                    break;
                case Operation.CONNECT:
                    BridgeClient.Connect(Address, Password);
                    break;
                
            }
            return Content;
        }

        public enum Operation
        {
            START, STOP, CONNECT
        }
    }
}