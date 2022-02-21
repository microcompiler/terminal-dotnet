﻿using System.Text;

using Bytewizer.TinyCLR.SecureShell.Messages.Connection;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class SubsystemRequestMessage : ChannelRequestMessage
    {
        public string Name { get; private set; }

        protected override void OnLoad(SshDataWorker reader)
        {
            base.OnLoad(reader);

            Name = reader.ReadString(Encoding.ASCII);
        }
    }
}