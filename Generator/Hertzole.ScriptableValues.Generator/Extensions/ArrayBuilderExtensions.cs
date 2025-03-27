using System;

namespace Hertzole.ScriptableValues.Generator;

public static class ArrayBuilderExtensions
{
	public static void AddRange(this ArrayBuilder<char> builder, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return;
		}

		builder.AddRange(value.AsSpan());
	}
}