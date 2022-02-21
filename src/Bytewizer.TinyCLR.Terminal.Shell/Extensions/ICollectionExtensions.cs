using System.Collections;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Provides extension methods for instances implementing the ICollection interface.
    /// </summary>
    public static class ICollectionExtensions
    {
        public static string[] ToStringArray(this ICollection source)
        {
            var results = new string[source.Count];
            
            var i = 0;
            foreach (var item in source)
            {
                results[i] = item.ToString();
                i++;
            }
            
            return results;
        }
    }
}
