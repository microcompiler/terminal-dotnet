namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public class PtyArgs
    {
        public PtyArgs(SessionChannel channel, string terminal, uint heightPixels, uint heightRows, uint widthPixels, uint widthColumns, string modes, UserauthArgs userauthArgs)
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
            HeightPixels = heightPixels;
            HeightRows = heightRows;
            WidthPixels = widthPixels;
            WidthColumns = widthColumns;
            Modes = modes;

            AttachedUserauthArgs = userauthArgs;
        }

        public SessionChannel Channel { get; private set; }
        public string Terminal { get; private set; }
        public uint HeightPixels { get; private set; }
        public uint HeightRows { get; private set; }
        public uint WidthPixels { get; private set; }
        public uint WidthColumns { get; private set; }
        public string Modes { get; private set; }
        public UserauthArgs AttachedUserauthArgs { get; private set; }
    }
}
