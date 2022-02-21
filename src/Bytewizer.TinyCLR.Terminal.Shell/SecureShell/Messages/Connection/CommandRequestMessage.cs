namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class CommandRequestMessage : ChannelRequestMessage
    {
        public string Command { get; private set; }

        protected override void OnLoad(SshDataStream reader)
        {
            base.OnLoad(reader);

            Command = reader.ReadString();
        }
    }
}
