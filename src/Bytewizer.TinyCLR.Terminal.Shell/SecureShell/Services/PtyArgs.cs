namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public class PtyArgs
    {
        public PtyArgs(SessionChannel channel, string terminal, uint heightPx, uint heightRows, uint widthPx, uint widthChars, string modes, UserauthArgs userauthArgs)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            if (terminal == null)
            {
                throw new ArgumentNullException(nameof(terminal));
            }

            if (modes == null)
            {
                throw new ArgumentNullException(nameof(modes));
            }
            if (userauthArgs == null)
            {
                throw new ArgumentNullException(nameof(userauthArgs));
            }

            Channel = channel;
            Terminal = terminal;
            HeightPx = heightPx;
            HeightRows = heightRows;
            WidthPx = widthPx;
            WidthChars = widthChars;
            Modes = modes;

            AttachedUserauthArgs = userauthArgs;
        }

        public SessionChannel Channel { get; private set; }
        public string Terminal { get; private set; }
        public uint HeightPx { get; private set; }
        public uint HeightRows { get; private set; }
        public uint WidthPx { get; private set; }
        public uint WidthChars { get; private set; }
        public string Modes { get; private set; }
        public UserauthArgs AttachedUserauthArgs { get; private set; }
    }
}
