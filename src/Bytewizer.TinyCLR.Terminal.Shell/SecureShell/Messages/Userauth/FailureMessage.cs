using System;
using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Userauth
{
    public class FailureMessage : UserauthServiceMessage
    {
        private const byte MessageNumber = 51;

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataWorker writer)
        {
            writer.Write("password,publickey", Encoding.ASCII);
            writer.Write(false);
        }
    }
}
