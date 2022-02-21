using System;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class ForwardedTcpIpMessage : ChannelOpenMessage
    {
        public string Address { get; private set; }
        public uint Port { get; private set; }
        public string OriginatorIPAddress { get; private set; }
        public uint OriginatorPort { get; private set; }

        protected override void OnLoad(SshDataStream reader)
        {
            base.OnLoad(reader);

            if (ChannelType != "forwarded-tcpip")
            { 
                throw new ArgumentException(string.Format("Channel type {0} is not valid.", ChannelType)); 
            }

            Address = reader.ReadString();
            Port = reader.ReadUInt32();
            OriginatorIPAddress = reader.ReadString();
            OriginatorPort = reader.ReadUInt32();
        }
    }
}
