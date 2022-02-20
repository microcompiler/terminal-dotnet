using Bytewizer.TinyCLR.Features;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Terminal
{
    public interface ITerminalContext : IContext
    {
        IFeatureCollection Features { get; }
        TerminalOptions Options { get; }
        TerminalRequest Request { get; }
        TerminalResponse Response { get; }
    }
}