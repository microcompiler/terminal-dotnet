namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// The terminal options used for a connection.
    /// </summary>
    public class TerminalOptions
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TerminalOptions" /> class.
        /// </summary>
        public TerminalOptions()
        {
            Clear();
        }

        /// <summary>
        /// Gets or sets the height of the user terminal in charters.
        /// </summary>     
        public uint Height { get; set; }

        /// <summary>
        /// Gets or sets the height of the user terminal in pixels.
        /// </summary>  
        public uint HeightPixels { get; set; }

        /// <summary>
        /// Gets or sets the width of the user terminal in charters.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// Gets or sets the width of the user terminal in pixels.
        /// </summary>
        public uint WidthPixels { get; set; }
       
        /// <summary>
        /// Gets or sets the type of terminal attached (used to decide if we can send ANSI, etc.)
        /// </summary>
        public string TerminalType { get; set; }

        /// <summary>
        /// Gets or sets the mode of the user terminal.
        /// </summary>
        public string Modes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client wants to communicate ANSI escape sequences (for output colorization and such).
        /// </summary>
        public bool UseANSI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client wants to receive output text echoing what was sent to the server.
        /// </summary>
        public bool UseEcho { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client wants the server to perform word-wrapping.
        /// </summary>
        public bool UseWordWrap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the server side text output buffer should be used.
        /// </summary>
        public bool UseBuffer { get; set; }

        /// <summary>
        /// Clears the <see cref="TerminalOptions"/> object.
        /// </summary>
        public void Clear()
        {
            Height = 20;
            HeightPixels = 0;
            Width = 80;
            WidthPixels = 0;
            TerminalType = string.Empty;
            UseANSI = true;
            UseEcho = false;
            UseWordWrap = true;
            UseBuffer = true;
        }
    }
}