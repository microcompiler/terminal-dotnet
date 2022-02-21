using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Terminal.Features;

using Bytewizer.TinyCLR.SecureShell;
using Bytewizer.TinyCLR.SecureShell.Services;

using MiniTerm;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Terminal
{
    internal class ShellMiddleware : Middleware
    {
        private static int windowWidth, windowHeight;

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
            ShellContext ctx = context as ShellContext;

            // Set features objects
            var sessionFeature = new SessionFeature();
            context.Features.Set(typeof(SessionFeature), sessionFeature);

            var endpointFeature = new EndpointFeature();
            context.Features.Set(typeof(EndpointFeature), endpointFeature);

            // Set network connection timeout
            ctx.Channel.InputStream.ReadTimeout = _shellOptions.ConnectionTimeout;

            // Validate SSH protocol version
            //if (ValidateHandshakeVersion(ctx, out string clientProtocol))
            //{
            //    var secureShellFeature = new SecureShellFeature();
            //    secureShellFeature.ClientProtocol = clientProtocol;

            //    context.Features.Set(typeof(SecureShellFeature), secureShellFeature);
            //    _logger.HandshakeSucceeded(clientProtocol);
            //}
            //else
            //{
            //    // Exit and close connection
            //    _logger.HandshakeFailed(clientProtocol);
            //    return;
            //}

            next(context);

            if (endpointFeature.AutoMapping == true)
            {
                foreach(DictionaryEntry endpoint in _endpointProvider.GetEndpoints())
                {
                    endpointFeature.Endpoints.Add(endpoint.Key, endpoint.Value);
                }
            };

            endpointFeature.AvailableCommands = _endpointProvider.GetCommands();

            ctx.Session = new ShellSession(_logger, ctx, _shellOptions);
            ctx.Session.KeysExchanged += KeysExchanged;
            ctx.Session.ServiceRegistered += ServiceRegistered;
           
            ctx.Session.EstablishConnection();
        }

        private void KeysExchanged(object sender, KeyExchangeArgs args)
        {

        }

        private void ServiceRegistered(object sender, SshService e)
        {
            var session = (ShellSession)sender;
            Console.WriteLine("Session {0} requesting {1}.",
            BitConverter.ToString(session.SessionId).Replace("-", ""), e.GetType().Name);

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

        static void Userauth(object sender, UserauthArgs args)
        {
            Console.WriteLine("Client {0} fingerprint: {1}.", args.KeyAlgorithm, args.Fingerprint);         
            args.Result = true;
        }

        static void PtyReceived(object sender, PtyArgs args)
        {
            Console.WriteLine("Request to create a PTY received for terminal type {0}", args.Terminal);
            windowWidth = (int)args.WidthChars;
            windowHeight = (int)args.HeightRows;
        }

        static void WindowChange(object sender, WindowChangeArgs args)
        {
            Debug.WriteLine(args.WidthColumns);
        }

        static void EnvReceived(object sender, EnvironmentArgs args)
        {
            Console.WriteLine("Received environment variable {0}:{1}", args.Name, args.Value);
        }

        static void CommandOpened(object sender, CommandRequestedArgs args)
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
                var terminal = new MiniTerminal("cmd.exe", windowWidth, windowHeight);

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