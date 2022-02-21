namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class KeyExchangeDhInitMessage : Message
    {
        private const byte MessageNumber = 30;

        public byte[] E { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            E = reader.ReadMpint();
        }
    }
}
