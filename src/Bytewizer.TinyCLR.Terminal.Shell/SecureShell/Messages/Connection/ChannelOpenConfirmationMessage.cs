namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class ChannelOpenConfirmationMessage : ConnectionServiceMessage
    {
        private const byte MessageNumber = 91;

        public uint RecipientChannel { get; set; }
        public uint SenderChannel { get; set; }
        public uint InitialWindowSize { get; set; }
        public uint MaximumPacketSize { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write(RecipientChannel);
            writer.Write(SenderChannel);
            writer.Write(InitialWindowSize);
            writer.Write(MaximumPacketSize);
        }
    }
}