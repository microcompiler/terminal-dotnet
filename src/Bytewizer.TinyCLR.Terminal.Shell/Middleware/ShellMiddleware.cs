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

            ctx.Session = new Session(_logger, ctx, _shellOptions);
            ctx.Session.KeysExchanged += Session_KeysExchanged;
            ctx.Session.ServiceRegistered += Session_ServiceRegistered;

            ctx.Session.EstablishConnection();
        }

        private bool ValidateHandshakeVersion(ShellContext context, out string clientProtocol)
        {
            // Write server protocol message
            context.Channel.Write(_shellOptions.ProtocolExchangeMessage);

            // Wait for client to response and validate SSH version
            using (var reader = new StreamReader(context.Channel.InputStream))
            {
                clientProtocol = string.Empty;

                try
                {
                    do
                    {
                        clientProtocol = reader.ReadLine() ?? string.Empty;
                    } while (!clientProtocol.ToUpper().StartsWith("SSH-2.0"));

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        private void Session_KeysExchanged(object? sender, KeyExchangeArgs e)
        {
            //Console.WriteLine("Client {0} fingerprint: {1}.", e.KeyAlgorithm, e.Fingerprint);

            //e.Result = true;
        }

        private void Session_ServiceRegistered(object sender, SshService e)
        {
            var session = (Session)sender;
            Console.WriteLine("Session {0} requesting {1}.",
                BitConverter.ToString(session.SessionId).Replace("-", ""), e.GetType().Name);

            if (e is UserauthService)
            {
                var service = (UserauthService)e;
                service.Userauth += service_Userauth;
            }
            else if (e is ConnectionService)
            {
                var service = (ConnectionService)e;
                service.CommandOpened += service_CommandOpened;
                service.EnvReceived += service_EnvReceived;
                service.PtyReceived += service_PtyReceived;
                //service.TcpForwardRequest += service_TcpForwardRequest;
                //service.WindowChange += Service_WindowChange;
            }
        }

        static void service_PtyReceived(object sender, PtyArgs e)
        {
            Console.WriteLine("Request to create a PTY received for terminal type {0}", e.Terminal);
            windowWidth = (int)e.WidthChars;
            windowHeight = (int)e.HeightRows;
        }

        static void service_EnvReceived(object sender, EnvironmentArgs e)
        {
            Console.WriteLine("Received environment variable {0}:{1}", e.Name, e.Value);
        }

        static void service_Userauth(object sender, UserauthArgs e)
        {
            Console.WriteLine("Client {0} fingerprint: {1}.", e.KeyAlgorithm, e.Fingerprint);
            e.Result = true;
        }

        static void service_CommandOpened(object sender, CommandRequestedArgs e)
        {
            Console.WriteLine($"Channel {e.Channel.ServerChannelId} runs {e.ShellType}: \"{e.CommandText}\".");

            var allow = true;  // func(e.ShellType, e.CommandText, e.AttachedUserauthArgs);

            if (!allow)
            {
                return;
            }

            if (e.ShellType == "shell")
            {
                // requirements: Windows 10 RedStone 5, 1809
                // also, you can call powershell.exe
                var terminal = new MiniTerminal("cmd.exe", windowWidth, windowHeight);

                e.Channel.DataReceived += (ss, ee) => terminal.OnInput(ee);
                e.Channel.CloseReceived += (ss, ee) => terminal.OnClose();
                terminal.DataReceived += (ss, ee) => e.Channel.SendData(ee);
                terminal.CloseReceived += (ss, ee) => e.Channel.SendClose(ee);

                terminal.Run();
            }
            else if (e.ShellType == "subsystem")
            {
                // do something more
            }
        }
    }
}