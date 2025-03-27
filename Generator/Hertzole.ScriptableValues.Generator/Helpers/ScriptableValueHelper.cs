using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

public enum ScriptableType
{
	None = 0,
	Event = 1,
	GenericEvent = 2,
	Value = 3,
	Pool = 4
}

internal static class ScriptableValueHelper
{
	public static bool TryGetScriptableType(INamedTypeSymbol? type, out ScriptableType scriptableType, out ITypeSymbol? genericType)
	{
		genericType = null;

		if (type != null)
		{
			if (TryGetScriptableTypeFromType(type, out scriptableType, out genericType))
			{
				return true;
			}

			if (TryGetScriptableTypeFromAddressable(type, out scriptableType, out genericType))
			{
				return true;
			}
		}

		scriptableType = ScriptableType.None;
		return false;
	}

	private static bool TryGetScriptableTypeFromType(INamedTypeSymbol type, out ScriptableType scriptableType, out ITypeSymbol? genericType)
	{
		if (type.ConstructedFrom.StringEquals(Types.SCRIPTABLE_VALUE))
		{
			genericType = type.TypeArguments[0];
			scriptableType = ScriptableType.Value;
			return true;
		}

		if (type.ConstructedFrom.StringEquals(Types.SCRIPTABLE_EVENT))
		{
			genericType = null;
			scriptableType = ScriptableType.Event;
			return true;
		}

		if (type.ConstructedFrom.StringEquals(Types.GENERIC_SCRIPTABLE_EVENT))
		{
			genericType = type.TypeArguments[0];
			scriptableType = ScriptableType.GenericEvent;
			return true;
		}

		if (type.ConstructedFrom.StringEquals(Types.SCRIPTABLE_POOL))
		{
			genericType = type.TypeArguments[0];
			scriptableType = ScriptableType.Pool;
			return true;
		}

		if (type.BaseType != null)
		{
			return TryGetScriptableTypeFromType(type.BaseType, out scriptableType, out genericType);
		}

		scriptableType = ScriptableType.None;
		genericType = null;
		return false;
	}

	private static bool TryGetScriptableTypeFromAddressable(INamedTypeSymbol type, out ScriptableType scriptableType, out ITypeSymbol? genericType)
	{
		while (true)
		{
			if (type.ConstructedFrom.StringEquals(Types.ASSET_REFERENCE_T))
			{
				if (type.TypeArguments[0] is not INamedTypeSymbol namedTypeSymbol)
				{
					scriptableType = ScriptableType.None;
					genericType = null;
					return false;
				}

				return TryGetScriptableTypeFromType(namedTypeSymbol, out scriptableType, out genericType);
			}

			if (type.BaseType != null)
			{
				type = type.BaseType;
				continue;
			}

			scriptableType = ScriptableType.None;
			genericType = null;
			return false;
		}
	}
}