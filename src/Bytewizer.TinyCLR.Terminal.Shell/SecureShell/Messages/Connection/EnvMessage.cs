using System.Text;

using Bytewizer.TinyCLR.SecureShell.Messages.Connection;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class EnvMessage : ChannelRequestMessage
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        protected override void OnLoad(SshDataWorker reader)
        {
            base.OnLoad(reader);

            Name = reader.ReadString(Encoding.ASCII);
            Value = reader.ReadString(Encoding.ASCII);
        }
    }
}