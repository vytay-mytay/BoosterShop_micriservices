using System.Collections.Generic;

namespace Catalog.API.Common
{
    public static class ListExtensions
    {
        public static List<TResult> Empty<TResult>(this List<TResult> list)
        {
            return new List<TResult>();
        }
    }
}
