using System;
using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

internal enum ScriptableType
{
	None = 0,
	Event = 1,
	GenericEvent = 2,
	Value = 3,
	Pool = 4,
	List = 5,
	Dictionary = 6
}

internal static class ScriptableValueHelper
{
	public static bool TryGetScriptableType(in SemanticModel semanticModel,
		INamedTypeSymbol? type,
		out ScriptableType scriptableType,
		out ITypeSymbol? genericType)
	{
		genericType = null;

		if (type != null)
		{
			if (TryGetScriptableTypeFromType(in semanticModel, type, out scriptableType, out genericType))
			{
				return true;
			}

			if (TryGetScriptableTypeFromAddressable(in semanticModel, type, out scriptableType, out genericType))
			{
				return true;
			}
		}

		scriptableType = ScriptableType.None;
		return false;
	}

	private static bool TryGetScriptableTypeFromType(in SemanticModel semanticModel,
		INamedTypeSymbol type,
		out ScriptableType scriptableType,
		out ITypeSymbol? genericType)
	{
		if (type.ConstructedFrom.StringEquals(Types.SCRIPTABLE_VALUE) ||
		    type.ConstructedFrom.StringEquals(Types.VALUE_REFERENCE))
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

		if (type.ConstructedFrom.StringEquals(Types.SCRIPTABLE_LIST))
		{
			genericType = type.TypeArguments[0];
			scriptableType = ScriptableType.List;
			return true;
		}

		if (type.ConstructedFrom.StringEquals(Types.SCRIPTABLE_DICTIONARY))
		{
			// Get the key and value types from ScriptableDictionary<TKey, TValue>
			ITypeSymbol keyType = type.TypeArguments[0];
			ITypeSymbol valueType = type.TypeArguments[1];

			// Get the KeyValuePair<,> generic type definition
			INamedTypeSymbol? keyValuePairType = semanticModel.Compilation.GetTypeByMetadataName("System.Collections.Generic.KeyValuePair`2");

			if (keyValuePairType != null)
			{
				// Construct the KeyValuePair<TKey, TValue> with the specific type arguments
				genericType = keyValuePairType.Construct(keyType, valueType);
				scriptableType = ScriptableType.Dictionary;
				return true;
			}
		}

		if (type.BaseType != null)
		{
			return TryGetScriptableTypeFromType(in semanticModel, type.BaseType, out scriptableType, out genericType);
		}

		scriptableType = ScriptableType.None;
		genericType = null;
		return false;
	}

	private static bool TryGetScriptableTypeFromAddressable(in SemanticModel semanticModel,
		INamedTypeSymbol type,
		out ScriptableType scriptableType,
		out ITypeSymbol? genericType)
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

				return TryGetScriptableTypeFromType(in semanticModel, namedTypeSymbol, out scriptableType, out genericType);
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

	public static bool IsSupportedType(in CallbackType callbackType, in ScriptableType scriptableType)
	{
		switch (callbackType)
		{
			case CallbackType.Value:
				return scriptableType == ScriptableType.Value;
			case CallbackType.Event:
				return scriptableType == ScriptableType.Event || scriptableType == ScriptableType.GenericEvent;
			case CallbackType.Collection:
				return scriptableType == ScriptableType.List || scriptableType == ScriptableType.Dictionary;
			case CallbackType.Pool:
				return scriptableType == ScriptableType.Pool;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public static bool DoesTypeSupportCallbacks(in ITypeSymbol symbol, out NotSupportedReason reason)
	{
		if (symbol.IsValueType && symbol.IsReadOnly)
		{
			reason = NotSupportedReason.ReadOnlyStruct;
			return false;
		}

		if (symbol.IsStatic)
		{
			reason = NotSupportedReason.StaticClass;
			return false;
		}

		reason = NotSupportedReason.None;
		return true;
	}
}

public enum NotSupportedReason
{
	None = 0,
	ReadOnlyStruct = 1,
	StaticClass = 2
}