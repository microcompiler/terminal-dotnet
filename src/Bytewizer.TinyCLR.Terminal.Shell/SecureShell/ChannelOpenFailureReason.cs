using System.ComponentModel;

namespace Bytewizer.TinyCLR.SecureShell
{
    public enum ChannelOpenFailureReason
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        None = 0, // Not used by protocol
        AdministrativelyProhibited = 1,
        ConnectFailed = 2,
        UnknownChannelType = 3,
        ResourceShortage = 4,
    }
}
