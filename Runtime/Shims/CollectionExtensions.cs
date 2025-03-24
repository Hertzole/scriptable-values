#if !NET6_0_OR_GREATER
using JetBrains.Annotations;

namespace System.Collections.Generic
{
	internal static class CollectionExtensions
	{
		public static bool TryGetNonEnumeratedCount<T>([NoEnumeration] this IEnumerable<T> enumerable, out int count)
		{
			switch (enumerable)
			{
				case ICollection<T> collection:
					count = collection.Count;
					return true;
				case IReadOnlyCollection<T> rCollection:
					count = rCollection.Count;
					return true;
				default:
					count = 0;
					return false;
			}
		}
	}
}
#endif