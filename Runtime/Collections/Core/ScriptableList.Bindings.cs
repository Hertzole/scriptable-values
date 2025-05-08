#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System.Collections.Generic;

namespace Hertzole.ScriptableValues
{
	partial class ScriptableList<T>
	{
		/// <inheritdoc />
		protected override long GetViewHashCode()
		{
			unchecked
			{
				long hash = 17;
				hash = hash * 23 + base.GetViewHashCode();
				hash = hash * 23 + Capacity.GetHashCode();
				hash = hash * 23 + Count.GetHashCode();
				hash = hash * 23 + IsReadOnly.GetHashCode();
				hash = hash * 23 + ClearOnStart.GetHashCode();
				hash = hash * 23 + SetEqualityCheck.GetHashCode();

				EqualityComparer<T> comparer = EqualityComparer<T>.Default;

				for (int i = 0; i < list.Count; i++)
				{
					hash = hash * 23 + comparer.GetHashCode(list[i]);
				}

				return hash;
			}
		}
	}
}
#endif