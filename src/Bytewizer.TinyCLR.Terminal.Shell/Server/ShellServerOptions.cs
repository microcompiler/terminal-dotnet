using System;
using System.Collections;
using System.Reflection;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents an implementation of the <see cref="ServerOptions"/> for creating terminal servers.
    /// </summary>
    public class ShellServerOptions : ServerOptions
    {
        private readonly Hashtable _hostKey = new Hashtable();

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellServerOptions"/> class.
        /// </summary>
        public ShellServerOptions()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ProtocolExchangeMessage =  $"SSH-2.0-TinyCLR_{version}";
            WelcomeMessage = $"Welcome to TinyCLR Terminal Server [{version}]";
            HelpMessage = "Enter 'help' for a list of built-in commands";
            Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        public void AddHostKeys(string type, string parameters)
        {
            if (!_hostKey.ContainsKey(type))
            {
                _hostKey.Add(type, parameters);
            }
        }

        /// <summary>
        /// Gets the host keys.
        /// </summary>
        public Hashtable HostKeys { get { return _hostKey; } }

        /// <summary>
        /// Specifies the command buffer size.
        /// </summary>
        public int BufferSize { get; set; } = 1024;

        /// <summary>
        /// Specifies the assemblies used to search for command providers.
        /// </summary>
        public Assembly[] Assemblies { get; set; }

        /// <summary>
        /// Specifies the session command prompt.
        /// </summary>
        public string CommandPrompt { get; set; } = "$";

        /// <summary>
        /// Specifies the session welecom message.
        /// </summary>
        public string WelcomeMessage { get; set; }

        /// <summary>
        /// Specifies the protocol verion exchange message.
        /// </summary>
        public string ProtocolExchangeMessage { get; set; }

        /// <summary>
        /// Specifies the session help message.
        /// </summary>
        public string HelpMessage { get; set; }

        /// <summary>
        /// Amount of password retries. (default: 3 tries)
        /// </summary>
        public int Retries { get; set; } = 3;

        /// <summary>
        /// Time to login in seconds. (default: 60 seconds)
        /// </summary>
        public int TimeToLogin { get; set; } = 60;

        /// <summary>
        /// Time server waits to establish session. (default: 5 seconds)
        /// </summary>
        public int ConnectionTimeout { get; set; } = 5000;
    }
}