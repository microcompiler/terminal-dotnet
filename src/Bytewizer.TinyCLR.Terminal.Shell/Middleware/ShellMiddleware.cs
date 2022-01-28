using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Terminal.Features;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Terminal
{
    internal class ShellMiddleware : Middleware
    {
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
            var ctx = context as ShellContext;

            // Set features objects
            var sessionFeature = new SessionFeature();
            context.Features.Set(typeof(SessionFeature), sessionFeature);

            var endpointFeature = new EndpointFeature();
            context.Features.Set(typeof(EndpointFeature), endpointFeature);

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
        }
    }
}