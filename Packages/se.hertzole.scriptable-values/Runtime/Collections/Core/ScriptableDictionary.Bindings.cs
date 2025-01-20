#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System.Collections.Generic;

namespace Hertzole.ScriptableValues
{
	partial class ScriptableDictionary<TKey, TValue>
	{
		protected override long GetViewHashCode()
		{
			unchecked
			{
				long hash = 17;
				hash = hash * 23 + base.GetViewHashCode();
				hash = hash * 23 + Count.GetHashCode();
				hash = hash * 23 + IsReadOnly.GetHashCode();
				hash = hash * 23 + ClearOnStart.GetHashCode();
				hash = hash * 23 + SetEqualityCheck.GetHashCode();

				for (int i = 0; i < keys.Count; i++)
				{
					hash = hash * 23 + EqualityComparer<TKey>.Default.GetHashCode(keys[i]);
					hash = hash * 23 + EqualityComparer<TValue>.Default.GetHashCode(values[i]);
				}

				return hash;
			}
		}
	}
}
#endif