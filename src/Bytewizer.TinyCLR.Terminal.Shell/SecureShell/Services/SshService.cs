using System.Diagnostics.Contracts;

namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public abstract class SshService
    {
        protected internal readonly ShellSession _session;

        public SshService(ShellSession session)
        {
            Contract.Requires(session != null);

            _session = session;
        }

        internal protected abstract void CloseService();
    }
}
