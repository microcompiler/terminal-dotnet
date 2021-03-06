using System;

using Bytewizer.TinyCLR.Security.Cryptography;

using Bytewizer.TinyCLR.SecureShell.Messages;
using Bytewizer.TinyCLR.SecureShell.Messages.Userauth;

namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public class UserauthService : SshService, IDynamicInvoker
    {
        public UserauthService(ShellSession session)
            : base(session)
        {
        }

        public event EventHandler<UserauthArgs> Userauth;

        public event EventHandler<string> Succeed;

        protected internal override void CloseService()
        {
        }

        internal void HandleMessageCore(UserauthServiceMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.InvokeHandleMessage(message);
        }

        private void HandleMessage(RequestMessage message)
        {
            switch (message.MethodName)
            {
                case "publickey":
                    var keyMsg = (PublicKeyRequestMessage)Message.LoadFrom(message, typeof(PublicKeyRequestMessage));
                    HandleMessage(keyMsg);
                    break;
                case "password":
                    var pswdMsg = (PasswordRequestMessage)Message.LoadFrom(message, typeof(PasswordRequestMessage));
                    HandleMessage(pswdMsg);
                    break;
                case "hostbased":
                case "none":
                default:
                    _session.SendMessage(new FailureMessage());
                    break;
            }
        }

        private void HandleMessage(PasswordRequestMessage message)
        {
            var verifed = false;

            var args = new UserauthArgs(_session, message.Username, message.Password);
            if (Userauth != null)
            {
                Userauth(this, args);
                verifed = args.Result;
            }

            if (verifed)
            {
                _session.RegisterService(message.ServiceName, args);

                Succeed?.Invoke(this, message.ServiceName);

                _session.SendMessage(new SuccessMessage());
                return;
            }
            else
            {
                _session.SendMessage(new FailureMessage());
            }
        }

        private void HandleMessage(PublicKeyRequestMessage message)
        {
            if (ShellSession._publicKeyAlgorithms.ContainsKey(message.KeyAlgorithmName))
            {
                var verifed = false;

                //var keyAlg = (PublicKeyAlgorithm)SshSession._publicKeyAlgorithms[message.KeyAlgorithmName];

                var keyAlg = ShellSession._publicKeyAlgorithms[message.KeyAlgorithmName](new RSAParameters());
                keyAlg.LoadKeyAndCertificatesData(message.PublicKey);

                var args = new UserauthArgs(base._session, message.Username, message.KeyAlgorithmName, keyAlg.GetFingerprint(), message.PublicKey);
                Userauth?.Invoke(this, args);
                verifed = args.Result;

                if (!verifed)
                {
                    _session.SendMessage(new FailureMessage());
                    return;
                }

                if (!message.HasSignature)
                {
                    _session.SendMessage(new PublicKeyOkMessage { KeyAlgorithmName = message.KeyAlgorithmName, PublicKey = message.PublicKey });
                    return;
                }

                var sig = keyAlg.GetSignature(message.Signature);

                using (var worker = new SshDataStream())
                {
                    worker.WriteBinary(_session.SessionId);
                    worker.Write(message.PayloadWithoutSignature);

                    verifed = keyAlg.VerifyData(worker.ToByteArray(), sig);
                }

                if (!verifed)
                {
                    _session.SendMessage(new FailureMessage());
                    return;
                }

                _session.RegisterService(message.ServiceName, args);
                Succeed?.Invoke(this, message.ServiceName);
                _session.SendMessage(new SuccessMessage());
            }
        }
    }
}
