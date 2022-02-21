using System;

namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public class CommandRequestedArgs
    {
        public CommandRequestedArgs(SessionChannel channel, string type, string command, UserauthArgs userauthArgs)
        {
            //if (channel == null)
            //{
            //    throw new ArgumentNullException(nameof(channel));
            //}
            //if (command == null)
            //{
            //    throw new ArgumentNullException(nameof(command));
            //}
            //if (userauthArgs == null)
            //{
            //    throw new ArgumentNullException(nameof(userauthArgs));
            //};

            Channel = channel;
            ShellType = type;
            CommandText = command;
            AttachedUserauthArgs = userauthArgs;
        }

        public SessionChannel Channel { get; private set; }
        public string ShellType { get; private set; }
        public string CommandText { get; private set; }
        public UserauthArgs AttachedUserauthArgs { get; private set; }
    }
}