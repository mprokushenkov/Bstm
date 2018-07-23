using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Bstm.Common
{
    public static class Extensions
    {
        public static void ForEach<T>([NotNull] this IEnumerable<T> enumerable, [NotNull] Action<T> action)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}