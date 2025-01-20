#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
	partial class ScriptableList<T>
	{
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

				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != null)
					{
						hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(list[i]);
					}
				}

				return hash;
			}
		}
	}
}
#endif