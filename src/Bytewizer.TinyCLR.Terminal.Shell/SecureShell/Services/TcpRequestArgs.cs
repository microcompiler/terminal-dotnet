namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public class TcpRequestArgs
    {
        public TcpRequestArgs(SessionChannel channel, string host, int port, string originatorIP, int originatorPort, UserauthArgs userauthArgs)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (originatorIP == null)
            {
                throw new ArgumentNullException(nameof(originatorIP));
            }

            Channel = channel;
            Host = host;
            Port = port;
            OriginatorIP = originatorIP;
            OriginatorPort = originatorPort;
            AttachedUserauthArgs = userauthArgs;
        }

        public SessionChannel Channel { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string OriginatorIP { get; private set; }
        public int OriginatorPort { get; private set; }
        public UserauthArgs AttachedUserauthArgs { get; private set; }
    }
}