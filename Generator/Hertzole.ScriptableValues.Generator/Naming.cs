using System;
using System.Buffers;

namespace Hertzole.ScriptableValues.Generator;

internal static class Naming
{
	public static ReadOnlySpan<char> FormatVariableName(ReadOnlySpan<char> value)
	{
		value = RemovePrefix(value);
		value = UppercaseStart(value);

		return value;
	}

	private static ReadOnlySpan<char> RemovePrefix(ReadOnlySpan<char> value)
	{
		if (value.Length > 2 && value[1] == '_')
		{
			return value.Slice(2);
		}

		if (value.Length > 1 && value[0] == '_')
		{
			return value.Slice(1);
		}

		return value;
	}

	private static ReadOnlySpan<char> UppercaseStart(ReadOnlySpan<char> value)
	{
		if (value.Length == 0)
		{
			return value;
		}

		if (value[0] == char.ToUpperInvariant(value[0]))
		{
			return value;
		}

		char[] newValue = ArrayPool<char>.Shared.Rent(value.Length);
		try
		{
			value.CopyTo(newValue);

			newValue[0] = char.ToUpperInvariant(value[0]);

			return new ReadOnlySpan<char>(newValue, 0, value.Length);
		}
		finally
		{
			ArrayPool<char>.Shared.Return(newValue);
		}
	}
}