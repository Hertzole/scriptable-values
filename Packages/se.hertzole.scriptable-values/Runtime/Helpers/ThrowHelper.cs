#nullable enable
using System;
using System.Diagnostics;
#if NETSTANDARD2_1
using System.Diagnostics.CodeAnalysis;
#endif

namespace Hertzole.ScriptableValues.Helpers
{
	internal static class ThrowHelper
	{
		[Conditional("DEBUG")]
		public static void ThrowIfNull(
#if NETSTANDARD2_1
			[NotNull]
#endif
			object? obj,
			string name)
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