﻿#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System.Collections.Generic;

namespace Hertzole.ScriptableValues
{
	partial class ScriptableValue<T>
	{
		protected override long GetViewHashCode()
		{
			unchecked
			{
				long hash = 17;
				hash = hash * 23 + base.GetViewHashCode();
				hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(Value);
				hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(PreviousValue);
				hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(DefaultValue);
				return hash;
			}
		}
	}
}
#endif