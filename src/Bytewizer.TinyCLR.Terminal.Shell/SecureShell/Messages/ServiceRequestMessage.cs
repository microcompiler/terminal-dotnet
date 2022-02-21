namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class ServiceRequestMessage : Message
    {
        private const byte MessageNumber = 5;

        public string ServiceName { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            ServiceName = reader.ReadString();
        }
    }
}
