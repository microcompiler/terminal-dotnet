namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class ChannelDataMessage : ConnectionServiceMessage
    {
        private const byte MessageNumber = 94;

        public uint RecipientChannel { get; set; }
        public byte[] Data { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            RecipientChannel = reader.ReadUInt32();
            Data = reader.ReadBinary();
        }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write(RecipientChannel);
            writer.WriteBinary(Data);
        }
    }
}