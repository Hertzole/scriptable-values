#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System.Collections.Generic;

namespace Hertzole.ScriptableValues
{
	partial class ScriptableDictionary<TKey, TValue>
	{
		/// <inheritdoc />
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

				EqualityComparer<TKey> keyComparer = EqualityComparer<TKey>.Default;
				EqualityComparer<TValue> valueComparer = EqualityComparer<TValue>.Default;

				for (int i = 0; i < keys.Count; i++)
				{
					hash = hash * 23 + keyComparer.GetHashCode(keys[i]);
					hash = hash * 23 + valueComparer.GetHashCode(values[i]);
				}

				return hash;
			}
		}
	}
}
#endif