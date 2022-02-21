namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public class EnvironmentArgs
    {
        public EnvironmentArgs(SessionChannel channel, string name, string value, UserauthArgs userauthArgs)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (userauthArgs == null)
            {
                throw new ArgumentNullException(nameof(userauthArgs));
            }

            Channel = channel;
            Name = name;
            Value = value;
            AttachedUserauthArgs = userauthArgs;
        }

        public SessionChannel Channel { get; private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }
        public UserauthArgs AttachedUserauthArgs { get; private set; }
    }
}
