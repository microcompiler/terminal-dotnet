namespace Bytewizer.TinyCLR.SecureShell.Messages.Userauth
{
    public class FailureMessage : UserauthServiceMessage
    {
        private const byte MessageNumber = 51;

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write("password,publickey");
            writer.Write(false);
        }
    }
}
