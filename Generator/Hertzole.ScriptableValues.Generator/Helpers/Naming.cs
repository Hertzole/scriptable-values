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

	public static string CreateCallbackName(string name, in ScriptableType scriptableType, in CallbackFlags flags)
	{
		using ArrayBuilder<char> builder = new ArrayBuilder<char>(32);

		ReadOnlySpan<char> prettyName = FormatVariableName(name.AsSpan());

		builder.AddRange("On");
		builder.AddRange(prettyName);

		switch (scriptableType)
		{
			case ScriptableType.Value:
				AppendFlagSuffix(in builder, in scriptableType, in flags);
				break;
			case ScriptableType.Event:
			case ScriptableType.GenericEvent:
				builder.AddRange("Invoked");
				break;
			case ScriptableType.Pool:
			case ScriptableType.List:
			case ScriptableType.Dictionary:
				builder.AddRange("Changed");
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		return builder.ToString();
	}

	public static void AppendFlagSuffix(in ArrayBuilder<char> builder, in ScriptableType type, in CallbackFlags flags)
	{
		if (type == ScriptableType.Value)
		{
			if ((flags & CallbackFlags.PostInvoke) != 0)
			{
				builder.AddRange("Changed");
			}
			else if ((flags & CallbackFlags.PreInvoke) != 0)
			{
				builder.AddRange("Changing");
			}
		}
	}
}