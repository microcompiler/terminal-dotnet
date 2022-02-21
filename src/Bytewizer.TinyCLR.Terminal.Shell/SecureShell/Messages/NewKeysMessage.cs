using System;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class NewKeysMessage : Message
    {
        private const byte MessageNumber = 21;

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
        }

        protected override void OnGetPacket(SshDataStream writer)
        {
        }
    }
}
