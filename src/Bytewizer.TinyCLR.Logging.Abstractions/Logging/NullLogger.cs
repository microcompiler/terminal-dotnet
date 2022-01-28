﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

#if NanoCLR
namespace Bytewizer.NanoCLR.Logging
#else
namespace Bytewizer.TinyCLR.Logging
#endif
{
    /// <summary>
    /// Minimalistic logger that does nothing.
    /// </summary>
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Returns the shared instance of <see cref="NullLogger"/>.
        /// </summary>
        public static NullLogger Instance { get; } = new NullLogger();
        private NullLogger()
        {
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        /// <inheritdoc />
        public void Log(LogLevel logLevel, EventId eventId, object state, Exception exception)
        {
        }
    }
}
