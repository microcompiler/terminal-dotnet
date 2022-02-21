using System.Diagnostics.Contracts;

namespace Bytewizer.TinyCLR.SecureShell.Services
{
    public abstract class SshService
    {
        protected internal readonly SshSession _session;

        public SshService(SshSession session)
        {
            Contract.Requires(session != null);

            _session = session;
        }

        internal protected abstract void CloseService();
    }
}
