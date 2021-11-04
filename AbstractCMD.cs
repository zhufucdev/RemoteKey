namespace RemoteKey
{
    public abstract class AbstractCommand
    {
        public readonly Command Content;
        public AbstractCommand(Command command)
        {
            Content = command;
        }

        /// <summary>Execute this command and return its result.</summary>
        public abstract Command Execute();
    }
}