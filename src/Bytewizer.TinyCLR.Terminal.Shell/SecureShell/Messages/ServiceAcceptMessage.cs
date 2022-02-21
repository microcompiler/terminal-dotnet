namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class ServiceAcceptMessage : Message
    {
        private const byte MessageNumber = 6;

        public ServiceAcceptMessage(string name)
        {
            ServiceName = name;
        }

        public string ServiceName { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write(ServiceName);
        }
    }
}
