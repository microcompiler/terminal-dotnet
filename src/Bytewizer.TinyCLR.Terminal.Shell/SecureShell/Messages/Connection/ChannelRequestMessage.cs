using System.Text;

namespace Bytewizer.TinyCLR.SecureShell.Messages.Connection
{
    public class ChannelRequestMessage : ConnectionServiceMessage
    {
        private const byte MessageNumber = 98;

        public uint RecipientChannel { get; set; }
        public string RequestType { get; set; }
        public bool WantReply { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            RecipientChannel = reader.ReadUInt32();
            RequestType = reader.ReadString();
            WantReply = reader.ReadBoolean();
        }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write(RecipientChannel);
            writer.Write(RequestType);
            writer.Write(WantReply);
        }
    }
}
