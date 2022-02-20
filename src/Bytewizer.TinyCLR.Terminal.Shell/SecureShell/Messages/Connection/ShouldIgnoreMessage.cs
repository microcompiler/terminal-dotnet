
namespace FxSsh.Messages.Connection
{
    public class ShouldIgnoreMessage : ConnectionServiceMessage
    {
        private const byte MessageNumber = 2;

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataWorker reader)
        {
        }
    }
}
