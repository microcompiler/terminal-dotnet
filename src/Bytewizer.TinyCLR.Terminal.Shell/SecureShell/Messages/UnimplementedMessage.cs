namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class UnimplementedMessage : Message
    {
        private const byte MessageNumber = 3;

        public uint SequenceNumber { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            SequenceNumber = reader.ReadUInt32();
        }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write(SequenceNumber);
        }
    }
}
