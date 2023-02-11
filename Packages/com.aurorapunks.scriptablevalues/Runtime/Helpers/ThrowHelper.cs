using System;
using System.Diagnostics;

namespace AuroraPunks.ScriptableValues.Helpers
{
	internal static class ThrowHelper
	{
		[Conditional("DEBUG")]
		public static void ThrowIfNull(object obj, string name)
		{
#if DEBUG
			if (obj == null)
			{
				throw new ArgumentNullException(name, $"{name} is null.");
			}
#endif
		}
	}
}