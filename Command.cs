namespace RemoteKey
{
    public class Command {
        public readonly string[] Arguments;
        public int ExitCode;

        public bool IsSuccessful => ExitCode == 0;
        public Command(string[] args)
        {
            Arguments = args;
        }

        public void Succeed()
        {
            ExitCode = 0;
        }
    }
}