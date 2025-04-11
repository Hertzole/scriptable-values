using System.Collections.Generic;

namespace Hertzole.ScriptableValues
{
	internal static class CollectionExtensions
	{
		public static bool TryEnsureCapacity<T>(this IList<T> list, int capacity)
		{
			if (list is List<T> genericList)
			{
				if (genericList.Capacity < capacity)
				{
					genericList.Capacity = capacity;
				}

				return true;
			}

			if (list is ScriptableList<T> scriptableList)
			{
				scriptableList.EnsureCapacity(capacity);
				return true;
			}

			return false;
		}
	}
}