namespace Bytewizer.TinyCLR.Terminal.Features
{
    /// <summary>
    /// A feature interface for this session. Use <see cref="TerminalContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public class SecureShellFeature 
    {
        /// <summary>
        /// Gets or sets client protocol version information for the current client.
        /// </summary>
        public string ClientProtocol { get; set; } = string.Empty;
    }
}