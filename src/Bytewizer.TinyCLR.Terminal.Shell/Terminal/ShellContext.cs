using Bytewizer.TinyCLR.Features;
using Bytewizer.TinyCLR.Sockets.Channel;

using Bytewizer.TinyCLR.SecureShell;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Encapsulates all Terminal spcific information about an individual request.
    /// </summary>
    public class ShellContext : TerminalContext
    {
        private bool _active = true;

        /// <summary>
        /// Initializes an instance of the <see cref="ShellContext" /> class.
        /// </summary>
        public ShellContext() 
            : base()
        {
            Channel = new SocketChannel();
        }

        /// <summary>
        /// Gets or sets the <see cref="SocketChannel"/> used to manage user session for this request.
        /// </summary>
        public SocketChannel Channel { get; set; }

        /// <summary>
        /// Get the state for this session.
        /// </summary>
        public override bool Active
        {
            get { return _active || Channel.Connected; }
            set { _active = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Terminal.Session"/> used to manage user session data for this request.
        /// </summary>
        public Session Session { get; set; }

        /// <summary>
        /// Gets information about the underlying connection for this request.
        /// </summary>
        public ConnectionInfo Connection => Channel.Connection;

        /// <summary>
        /// Closes the connected socket channel and clears context.
        /// </summary>
        public override void Clear()
        {
            ((FeatureCollection)Features).Clear();
            Options.Clear();
            Request.Clear();
            Response.Clear();
            Channel.Clear();
            Active = true;
        }
    }
}