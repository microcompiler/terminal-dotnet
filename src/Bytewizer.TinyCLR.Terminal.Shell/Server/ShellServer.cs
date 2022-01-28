﻿using System;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents an implementation of the <see cref="ShellServer"/> for creating terminal servers.
    /// </summary>
    public class ShellServer : SocketService, IServerService
    {
        private readonly ILogger _logger;
        private readonly ContextPool _contextPool;
        private readonly ShellServerOptions _terminalOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="ShellServer"/> specific features.</param>
        public ShellServer(ServerOptionsDelegate configure)
            : this(NullLoggerFactory.Instance, new ShellServerOptions())
        {
            Initialize(configure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="ShellServer"/> specific features.</param>
        public ShellServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
            : this(loggerFactory, new ShellServerOptions())
        {
            Initialize(configure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options of <see cref="ShellServer"/> specific features.</param>
        public ShellServer(ILoggerFactory loggerFactory, ServerOptions options)
            : base(loggerFactory)
        {
            _options = options;
            _contextPool = new ContextPool();
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Terminal");
            _terminalOptions = _options as ShellServerOptions;

            SetListener();
        }

        private void Initialize(ServerOptionsDelegate configure)
        {
            _options.Listen(22);
            _options.Pipeline(app =>
            {
                app.Use(new ShellMiddleware(_logger, _terminalOptions));
            });

            configure(_options as ShellServerOptions);
        }

        /// <summary>
        /// Gets configuration options of socket specific features.
        /// </summary>
        public SocketListenerOptions ListenerOptions { get => _listener?.Options; }

        /// <summary>
        /// Gets port that the server is actively listening on.
        /// </summary>
        public int ActivePort { get => _listener.ActivePort; }


        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The socket channel for the connected end point.</param>
        protected override void ClientConnected(object sender, SocketChannel channel)
        {
            _logger.RemoteConnected(channel);

            // Set channel error handler
            channel.ChannelError += ChannelError;

            try
            {
                // get context from context pool
                var context = _contextPool.GetContext(typeof(ShellContext)) as ShellContext;

                // assign channel
                context.Channel = channel;

                try
                {
                    // Invoke pipeline
                    _options.Application.Invoke(context);
                }
                catch (Exception ex)
                {
                    _logger.UnhandledException(ex);
                }

                _logger.RemoteClosed(channel);

                // release context back to pool and close connection once pipeline is complete
                _contextPool.Release(context);
            }
            catch (Exception ex)
            {
                _logger.UnhandledException(ex);
                return;
            }
        }

        /// <summary>
        /// A client has disconnected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="execption">The socket <see cref="Exception"/> for the disconnected endpoint.</param>
        protected override void ClientDisconnected(object sender, Exception execption)
        {
            _logger.RemoteDisconnect(execption);
        }

        /// <summary>
        /// An internal channel error occured.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="execption">The <see cref="Exception"/> for the channel error.</param>
        private void ChannelError(object sender, Exception execption)
        {
            _logger.ChannelExecption(execption);
        }
    }
}