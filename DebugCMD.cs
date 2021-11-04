using System;

namespace RemoteKey
{
    public class DebugCommand : AbstractCommand
    {
        public bool Target;
        public DebugCommand(Command command) : base(command)
        {
            var t = command.Arguments[0];
            if (t.Equals("true", System.StringComparison.OrdinalIgnoreCase))
            {
                Target = true;
            }
            else if (t.Equals("false", System.StringComparison.OrdinalIgnoreCase))
            {
                Target = false;
            }
            else
            {
                throw new ArgumentException("pass True or False as the first argument");
            }
        }

        public override Command Execute()
        {
            Program.Debug = Target;
            Content.Succeed();
            return Content;
        }
    }
}