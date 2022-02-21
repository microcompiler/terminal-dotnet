using System.Diagnostics;
using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Terminal.Features;

using Bytewizer.TinyCLR.SecureShell;
using Bytewizer.TinyCLR.SecureShell.Services;

using MiniTerm;

namespace Bytewizer.TinyCLR.Terminal
{
    internal class ShellMiddleware : Middleware
    {
        private ShellContext _ctx;

        private readonly ILogger _logger;
        private readonly ShellServerOptions _shellOptions;
        private readonly EndpointProvider _endpointProvider;
        
        public ShellMiddleware(ILogger logger, ShellServerOptions options)
        {
            _logger = logger;
            _shellOptions = options;
            _endpointProvider = new EndpointProvider(options.Assemblies);
        }

        protected override void Invoke(ITerminalContext context, RequestDelegate next)
        {
            _ctx = context as ShellContext;

            // Set features objects
            var sessionFeature = new SessionFeature();
            context.Features.Set(typeof(SessionFeature), sessionFeature);

            var endpointFeature = new EndpointFeature();
            context.Features.Set(typeof(EndpointFeature), endpointFeature);

            // Set network connection timeout
            _ctx.Channel.InputStream.ReadTimeout = _shellOptions.ConnectionTimeout;

            next(context);

            if (endpointFeature.AutoMapping == true)
            {
                foreach(DictionaryEntry endpoint in _endpointProvider.GetEndpoints())
                {
                    endpointFeature.Endpoints.Add(endpoint.Key, endpoint.Value);
                }
            };

            endpointFeature.AvailableCommands = _endpointProvider.GetCommands();

            _ctx.Session = new ShellSession(_logger, _ctx, _shellOptions);
            _ctx.Session.KeysExchanged += KeysExchanged;
            _ctx.Session.ServiceRegistered += ServiceRegistered;        
            _ctx.Session.EstablishConnection();
        }

        private void KeysExchanged(object sender, KeyExchangeArgs args)
        {
            foreach (var keyExchangeAlg in args.KeyExchangeAlgorithms)
            {
                Debug.WriteLine($"Key exchange algorithm: {keyExchangeAlg}");
            }
        }

        private void ServiceRegistered(object sender, SshService e)
        {
            var session = (ShellSession)sender;
            Debug.WriteLine($"Session {BitConverter.ToString(session.SessionId).Replace("-", "")} requesting {e.GetType().Name}.");

            if (e is UserauthService)
            {
                var service = (UserauthService)e;
                service.Userauth += Userauth;
            }
            else if (e is ConnectionService)
            {
                var service = (ConnectionService)e;
                service.PtyReceived += PtyReceived;
                service.WindowChange += WindowChange;
                service.EnvReceived += EnvReceived;
                service.CommandOpened += CommandOpened;               
            }
        }

        private void Userauth(object sender, UserauthArgs args)
        {
            Debug.WriteLine("Client {0} fingerprint: {1}.", args.KeyAlgorithm, args.Fingerprint);         
            args.Result = true;
        }

        private void PtyReceived(object sender, PtyArgs args)
        {
            Debug.WriteLine($"Request to create a PTY received for terminal type {args.Terminal}");
            _ctx.Options.TerminalType = args.Terminal;
            _ctx.Options.Modes = args.Modes;
            _ctx.Options.Width  = args.WidthColumns;
            _ctx.Options.WidthPixels = args.WidthPixels;
            _ctx.Options.Height = args.HeightRows;
            _ctx.Options.HeightPixels = args.HeightPixels;
        }

        private void WindowChange(object sender, WindowChangeArgs args)
        {
            Debug.WriteLine($"Windows change requst Width-{args.WidthColumns} Height-{args.HeightRows}");
            _ctx.Options.Width = args.WidthColumns;
            _ctx.Options.WidthPixels = args.WidthPixels;
            _ctx.Options.Height = args.HeightRows;
            _ctx.Options.HeightPixels = args.HeightPixels;
        }

        private void EnvReceived(object sender, EnvironmentArgs args)
        {
            Console.WriteLine("Received environment variable {0}:{1}", args.Name, args.Value);
        }

        private void CommandOpened(object sender, CommandRequestedArgs args)
        {
            Console.WriteLine($"Channel {args.Channel.ServerChannelId} runs {args.ShellType}: \"{args.CommandText}\".");

            var allow = true;  // func(e.ShellType, e.CommandText, e.AttachedUserauthArgs);

            if (!allow)
            {
                return;
            }

            if (args.ShellType == "shell")
            {
                // requirements: Windows 10 RedStone 5, 1809
                // also, you can call powershell.exe
                var terminal = new MiniTerminal("cmd.exe", (int)_ctx.Options.Width, (int)_ctx.Options.Height);

                args.Channel.DataReceived += (ss, ee) => terminal.OnInput(ee);
                args.Channel.CloseReceived += (ss, ee) => terminal.OnClose();
                terminal.DataReceived += (ss, ee) => args.Channel.SendData(ee);
                terminal.CloseReceived += (ss, ee) => args.Channel.SendClose(ee);

                terminal.Run();
            }
            else if (args.ShellType == "subsystem")
            {
                // do something more
            }
        }
    }
}