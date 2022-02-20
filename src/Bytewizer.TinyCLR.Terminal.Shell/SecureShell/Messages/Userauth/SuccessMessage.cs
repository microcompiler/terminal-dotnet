using System;

namespace FxSsh.Messages.Userauth
{
    public class SuccessMessage : UserauthServiceMessage
    {
        private const byte MessageNumber = 52;

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnGetPacket(SshDataWorker writer)
        {
        }
    }
}
