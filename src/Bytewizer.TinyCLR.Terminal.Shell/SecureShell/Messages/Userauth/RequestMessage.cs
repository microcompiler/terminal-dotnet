namespace Bytewizer.TinyCLR.SecureShell.Messages.Userauth
{
    public class RequestMessage : UserauthServiceMessage
    {
        protected const byte MessageNumber = 50;

        public string Username { get; protected set; }
        public string ServiceName { get; protected set; }
        public string MethodName { get; protected set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            Username = reader.ReadString();
            ServiceName = reader.ReadString();
            MethodName = reader.ReadString();
        }
    }
}
