using OBSWebsocketDotNet;

namespace RemoteKey
{
    public class OBSClient
    {
        public static OBSWebsocket OBSSocket = new OBSWebsocket();

        public static void Connect(string url, string password)
        {
            OBSSocket.Connect(url, password);
        }
    }
}