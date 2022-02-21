using System;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class SessionOpenMessage : ChannelOpenMessage
    {
        protected override void OnLoad(SshDataStream reader)
        {
            base.OnLoad(reader);

            if (ChannelType != "session")
            {
                throw new ArgumentException(string.Format("Channel type {0} is not valid.", ChannelType));
            }
        }
    }
}
