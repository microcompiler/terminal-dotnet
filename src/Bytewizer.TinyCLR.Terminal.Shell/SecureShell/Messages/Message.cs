using System;

namespace Bytewizer.TinyCLR.SecureShell.Messages
{
    public abstract class Message
    {
        public abstract byte MessageType { get; }

        protected byte[] RawBytes { get; set; }

        public void Load(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            RawBytes = bytes;
            using (var worker = new SshDataStream(bytes))
            {
                var number = worker.ReadByte();
                if (number != MessageType)
                {
                    throw new ArgumentException(string.Format("Message type {0} is not valid.", number));
                }

                OnLoad(worker);
            }
        }

        public byte[] GetPacket()
        {
            using (var worker = new SshDataStream())
            {
                worker.Write(MessageType);

                OnGetPacket(worker);

                return worker.ToByteArray();
            }
        }

        public static object LoadFrom(Message message, Type type) 
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var msg = (Message)Activator2.CreateInstance(type); // TODO: Hardcode for performance?
            msg.Load(message.RawBytes);
            return msg;
        }

        //public static T LoadFrom<T>(Message message) where T : Message, new()
        //{
        //    if (message == null)
        //    {
        //        throw new ArgumentNullException(nameof(message));
        //    }

        //    var msg = new T();
        //    msg.Load(message.RawBytes);
        //    return msg;
        //}

        protected virtual void OnLoad(SshDataStream reader)
        {
            throw new NotSupportedException();
        }

        protected virtual void OnGetPacket(SshDataStream writer)
        {
            throw new NotSupportedException();
        }
    }
}
