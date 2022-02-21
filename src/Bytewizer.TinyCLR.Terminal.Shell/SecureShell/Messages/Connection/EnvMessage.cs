namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class EnvMessage : ChannelRequestMessage
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        protected override void OnLoad(SshDataStream reader)
        {
            base.OnLoad(reader);

            Name = reader.ReadString();
            Value = reader.ReadString();
        }
    }
}