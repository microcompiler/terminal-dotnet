namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class ChannelEofMessage : ConnectionServiceMessage
    {
        private const byte MessageNumber = 96;

        public uint RecipientChannel { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            RecipientChannel = reader.ReadUInt32();
        }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write(RecipientChannel);
        }
    }
}