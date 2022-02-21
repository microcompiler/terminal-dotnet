﻿using System;
using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class ServiceRequestMessage : Message
    {
        private const byte MessageNumber = 5;

        public string ServiceName { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataWorker reader)
        {
            ServiceName = reader.ReadString(Encoding.ASCII);
        }
    }
}
