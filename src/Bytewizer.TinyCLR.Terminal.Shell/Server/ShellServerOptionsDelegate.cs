namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents an options method to configure <see cref="ShellServerOptions"/> specific features.
    /// </summary>
    /// <param name="configure">The <see cref="ShellServerOptions"/> configuration specific features.</param>
    public delegate void ServerOptionsDelegate(ShellServerOptions configure);
}