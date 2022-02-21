namespace Bytewizer.TinyCLR.SecureShell.Messages.Userauth
{
    public class PublicKeyOkMessage : UserauthServiceMessage
    {
        private const byte MessageNumber = 60;

        public string KeyAlgorithmName { get; set; }
        public byte[] PublicKey { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write(KeyAlgorithmName);
            writer.WriteBinary(PublicKey);
        }
    }
}
