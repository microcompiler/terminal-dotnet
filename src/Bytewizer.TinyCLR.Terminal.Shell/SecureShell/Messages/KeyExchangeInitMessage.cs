using Bytewizer.TinyCLR.Security.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public class KeyExchangeInitMessage : Message
    {
        private const byte MessageNumber = 20;

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create(); // new RNGCryptoServiceProvider();

        public KeyExchangeInitMessage()
        {
            Cookie = new byte[16];
            _rng.GetBytes(Cookie);
        }

        public byte[] Cookie { get; private set; }

        public string[] KeyExchangeAlgorithms { get; set; }

        public string[] ServerHostKeyAlgorithms { get; set; }

        public string[] EncryptionAlgorithmsClientToServer { get; set; }

        public string[] EncryptionAlgorithmsServerToClient { get; set; }

        public string[] MacAlgorithmsClientToServer { get; set; }

        public string[] MacAlgorithmsServerToClient { get; set; }

        public string[] CompressionAlgorithmsClientToServer { get; set; }

        public string[] CompressionAlgorithmsServerToClient { get; set; }

        public string[] LanguagesClientToServer { get; set; }

        public string[] LanguagesServerToClient { get; set; }

        public bool FirstKexPacketFollows { get; set; }

        public uint Reserved { get; set; }

        public override byte MessageType { get { return MessageNumber; } }

        protected override void OnLoad(SshDataStream reader)
        {
            Cookie = reader.ReadBinary(16);
            KeyExchangeAlgorithms = reader.ReadString().Split(',');
            ServerHostKeyAlgorithms = reader.ReadString().Split(',');
            EncryptionAlgorithmsClientToServer = reader.ReadString().Split(',');
            EncryptionAlgorithmsServerToClient = reader.ReadString().Split(',');
            MacAlgorithmsClientToServer = reader.ReadString().Split(',');
            MacAlgorithmsServerToClient = reader.ReadString().Split(',');
            CompressionAlgorithmsClientToServer = reader.ReadString().Split(',');
            CompressionAlgorithmsServerToClient = reader.ReadString().Split(',');
            LanguagesClientToServer = reader.ReadString().Split(',');
            LanguagesServerToClient = reader.ReadString().Split(',');
            FirstKexPacketFollows = reader.ReadBoolean();
            Reserved = reader.ReadUInt32();
        }

        protected override void OnGetPacket(SshDataStream writer)
        {
            writer.Write(Cookie);
            writer.Write(KeyExchangeAlgorithms.Join(","));
            writer.Write(ServerHostKeyAlgorithms.Join(","));
            writer.Write(EncryptionAlgorithmsClientToServer.Join(","));
            writer.Write(EncryptionAlgorithmsServerToClient.Join(","));
            writer.Write(MacAlgorithmsClientToServer.Join(","));
            writer.Write(MacAlgorithmsServerToClient.Join(","));
            writer.Write(CompressionAlgorithmsClientToServer.Join(","));
            writer.Write(CompressionAlgorithmsServerToClient.Join(","));
            writer.Write(LanguagesClientToServer.Join(","));
            writer.Write(LanguagesServerToClient.Join(","));
            writer.Write(FirstKexPacketFollows);
            writer.Write(Reserved);
        }
    }
}
