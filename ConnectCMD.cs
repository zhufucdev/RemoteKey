namespace RemoteKey
{
    public class ConnectCommand : AbstractCommand
    {
        public string Address, Password = "";
        public ConnectCommand(Command command) : base(command)
        {
            if (command.Arguments.Length == 1)
            {
                Address = command.Arguments[0];
            }
            else
            {
                Address = command.Arguments[0];
                Password = command.Arguments[1];
            }
        }

        public override Command Execute()
        {
            OBSClient.Connect(Address, Password);
            return Content;
        }
    }
}