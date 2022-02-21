namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public class UserauthArgs
    {
        public UserauthArgs(ShellSession session, string username, string keyAlgorithm, string fingerprint, byte[] key)
        {

            if (keyAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(keyAlgorithm));
            }

            if (fingerprint == null)
            {
                throw new ArgumentNullException(nameof(fingerprint));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            AuthMethod = "publickey";
            KeyAlgorithm = keyAlgorithm;
            Fingerprint = fingerprint;
            Key = key;
            Session = session;
            Username = username;
        }

        public UserauthArgs(ShellSession session, string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            AuthMethod = "password";
            Username = username;
            Password = password;
            Session = session;
        }

        public string AuthMethod { get; private set; }
        public ShellSession Session { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string KeyAlgorithm { get; private set; }
        public string Fingerprint { get; private set; }
        public byte[] Key { get; private set; }
        public bool Result { get; set; }
    }
}
