using System;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class DisconnectMessage : Message
    {
        private const byte MessageNumber = 1;

        public DisconnectMessage()
        {
        }

        public DisconnectMessage(DisconnectReason reasonCode, string description = "", string language = "en")
        {
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            ReasonCode = reasonCode;
            Description = description;
            Language = language;
        }

        public DisconnectReason ReasonCode { get; private set; }
        public string Description { get; private set; }
        public string Language { get; private set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            ReasonCode = (DisconnectReason)reader.ReadUInt32();
            Description = reader.ReadString();
            if (reader.DataAvailable >= 4)
                Language = reader.ReadString();
        }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write((uint)ReasonCode);
            writer.Write(Description);
            writer.Write(Language ?? "en");
        }
    }
}
