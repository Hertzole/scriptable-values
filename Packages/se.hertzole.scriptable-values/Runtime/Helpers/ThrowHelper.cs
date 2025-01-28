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
				ThrowNullArgumentException(name);
			}
#endif
		}

		public static void ThrowIfDisposed(in bool isDisposed)
		{
			if (isDisposed)
			{
				ThrowDisposedException();
			}
		}

#if NETSTANDARD2_1
		[DoesNotReturn]
#endif
		private static void ThrowNullArgumentException(string name)
		{
			throw new ArgumentNullException(name, $"{name} is null.");
		}

#if NETSTANDARD2_1
		[DoesNotReturn]
#endif
		private static void ThrowDisposedException()
		{
			throw new ObjectDisposedException("The object has been disposed.");
		}
	}
}