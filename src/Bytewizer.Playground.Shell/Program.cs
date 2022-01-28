using Bytewizer.TinyCLR.Terminal;

namespace Bytewizer.Playground.Terminal
{
    internal class Program
    {
        private static ShellServer? _telnetServer;

        static void Main()
        {
            _telnetServer = new ShellServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseAutoMapping();
                });
            });
            _telnetServer.Start();
        }
    }
}
