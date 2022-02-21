namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class ChannelOpenMessage : ConnectionServiceMessage
    {
        private const byte MessageNumber = 90;

        public string ChannelType { get; private set; }
        public uint SenderChannel { get; private set; }
        public uint InitialWindowSize { get; private set; }
        public uint MaximumPacketSize { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            ChannelType = reader.ReadString();
            SenderChannel = reader.ReadUInt32();
            InitialWindowSize = reader.ReadUInt32();
            MaximumPacketSize = reader.ReadUInt32();
        }
    }
}
