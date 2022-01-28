using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Terminal
{
    public class ShellSession
    {
        private readonly ILogger _logger;
        private readonly ShellContext _context;
        private readonly ShellServerOptions _shellOptions;

        public ShellSession(
                ILogger logger,
                TerminalContext context,
                ShellServerOptions terminalOptions)
        {
            _logger = logger;
            _context = context as ShellContext;
            _shellOptions = terminalOptions;

            // Process command loop
            while (_context.Active || _context.Channel.Connected)
            {
                Thread.Sleep(1);
            }
        }
    }
}