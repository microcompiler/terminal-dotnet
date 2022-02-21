﻿
namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public class SessionChannel : Channel
    {
        public SessionChannel(ConnectionService connectionService,
            uint clientChannelId, uint clientInitialWindowSize, uint clientMaxPacketSize,
            uint serverChannelId)
            : base(connectionService, clientChannelId, clientInitialWindowSize, clientMaxPacketSize, serverChannelId)
        {

        }
    }
}
