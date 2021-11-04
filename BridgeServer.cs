using WebSocketSharp;
using WebSocketSharp.Server;
using System.Net;

namespace RemoteKey
{
    public class Listen : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            BridgeServer.WaitForIncome();
            Send(BridgeServer.ListenerBuffer);
        }
    }

    public class Send : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            BridgeServer.ListenerBuffer = e.RawData;
            Send(new byte[] { 0 });
        }
    }

    public class BridgeServer
    {
        private static WebSocketServer server;
        public static bool Initialized => server != null;
        public static int Port
        {
            private set; get;
        }
        public static bool IsListening => server?.IsListening ?? false;

        public static byte[] ListenerBuffer
        {
            set {
                buffer = value;
                isWaiting = false;
            }

            get {
                isWaiting = true;
                return buffer;
            }
        }

        private static byte[] buffer;
        private static bool isWaiting = true;

        public static void WaitForIncome()
        {
            while (isWaiting);
        }

        public static void Initialize(int port)
        {
            server = new WebSocketServer(IPAddress.Parse("ws://localhost"), port);
            Port = port;

            server.AuthenticationSchemes = WebSocketSharp.Net.AuthenticationSchemes.Digest;
            server.UserCredentialsFinder = (id) => 
                id.Name == "bridge_client" ? new WebSocketSharp.Net.NetworkCredential("bridge_client", );
        }

        public static void Start()
        {
            if (server.IsListening)
            {
                return;
            }

            server.Start();
            Program.Log(TAG_BRIDGE, string.Format("WebSocket server listening on port {0}...", Port));

            server.AddWebSocketService<Listen>("/listen");
            server.AddWebSocketService<Send>("/send");

            return;
        }

        public static void Stop()
        {
            if (!server.IsListening)
            {
                return;
            }

            server.Stop();
        }

        const string TAG_BRIDGE = "Bridge";
    }
}