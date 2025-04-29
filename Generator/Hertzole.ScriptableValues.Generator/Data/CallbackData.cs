using System;
using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

internal readonly record struct CallbackData(
	string Name,
	CallbackType CallbackType,
	CallbackFlags Flags,
	ISymbol MemberType,
	ScriptableType ScriptableType,
	ITypeSymbol GenericType)
{
	public string MaskName { get; } = CreateMaskName(Name, in ScriptableType, in Flags);
	public string CachedFieldName { get; } = CreateCachedFieldName(Name, in ScriptableType, in Flags);

	public string RegisterCallbackMethod { get; } = CreateRegisterMethodName(true, in ScriptableType, in Flags);

	public string UnregisterCallbackMethod { get; } = CreateRegisterMethodName(false, in ScriptableType, in Flags);

	public string CallbackName { get; } = Naming.CreateCallbackName(Name, in ScriptableType, in Flags);

	public void AppendParameterTypes(in ArrayBuilder<(string name, string type)> builder)
	{
		string genericType = string.Empty;
		if (ScriptableType != ScriptableType.Event)
		{
			genericType = GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
		}

		switch (CallbackType)
		{
			case CallbackType.Value:
				builder.Add(("oldValue", genericType));
				builder.Add(("newValue", genericType));
				break;
			case CallbackType.Event:
				builder.Add(("object", "sender"));
				builder.Add(("args", ScriptableType == ScriptableType.GenericEvent ? genericType : "global::System.EventArgs"));
				break;
			case CallbackType.Collection:
				using (ArrayBuilder<char> nameBuilder = new ArrayBuilder<char>(57 + genericType.Length))
				{
					nameBuilder.AddRange("global::Hertzole.ScriptableValues.CollectionChangedArgs<");
					nameBuilder.AddRange(genericType);
					nameBuilder.Add('>');

					builder.Add(("args", nameBuilder.ToString()));
				}

				break;
			case CallbackType.Pool:
				using (ArrayBuilder<char> nameBuilder = new ArrayBuilder<char>(51 + genericType.Length))
				{
					nameBuilder.AddRange("global::Hertzole.ScriptableValues.PoolChangedArgs<");
					nameBuilder.AddRange(genericType);
					nameBuilder.Add('>');

					builder.Add(("args", nameBuilder.ToString()));
				}

				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public void AppendParameterDescriptions(in ArrayBuilder<(string name, string description)> builder)
	{
		switch (CallbackType)
		{
			case CallbackType.Value:
				builder.Add(("oldValue", DocumentationHelper.GetOldValueDescription(Flags)));
				builder.Add(("newValue", DocumentationHelper.GetNewValueDescription(Flags)));
				break;
			case CallbackType.Event:
				builder.Add(("sender", DocumentationHelper.EVENT_SENDER));
				builder.Add(("args", DocumentationHelper.EVENT_ARGS));
				break;
			case CallbackType.Collection:
				builder.Add(("args", DocumentationHelper.COLLECTION_ARGS));
				break;
			case CallbackType.Pool:
				builder.Add(("args", DocumentationHelper.POOL_ARGS));
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private static string CreateMaskName(string name, in ScriptableType scriptableType, in CallbackFlags flags)
	{
		using ArrayBuilder<char> builder = new ArrayBuilder<char>(32);

		builder.AddRange(name);
		Naming.AppendFlagSuffix(in builder, in scriptableType, in flags);

		return builder.ToString();
	}

	private static string CreateCachedFieldName(string name, in ScriptableType scriptableType, in CallbackFlags flags)
	{
		using ArrayBuilder<char> builder = new ArrayBuilder<char>(32);

		builder.AddRange(name);

		switch (scriptableType)
		{
			case ScriptableType.Value:
				builder.AddRange("ScriptableValueCallback");
				Naming.AppendFlagSuffix(in builder, in scriptableType, in flags);
				break;
			case ScriptableType.Event:
			case ScriptableType.GenericEvent:
				builder.AddRange("ScriptableEventCallback");
				break;
			case ScriptableType.List:
			case ScriptableType.Dictionary:
				builder.AddRange("ScriptableCollectionChanged");
				break;
			case ScriptableType.Pool:
				builder.AddRange("ScriptablePoolCallback");
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scriptableType), scriptableType, null);
		}

		return builder.ToString();
	}

	private static string CreateRegisterMethodName(bool subscribe, in ScriptableType scriptableType, in CallbackFlags flags)
	{
		using ArrayBuilder<char> builder = new ArrayBuilder<char>(32);

		builder.AddRange(subscribe ? "Register" : "Unregister");
		switch (scriptableType)
		{
			case ScriptableType.Value:
				builder.AddRange("Value");
				Naming.AppendFlagSuffix(in builder, in scriptableType, in flags);
				builder.AddRange("Listener");
				break;
			case ScriptableType.Event:
			case ScriptableType.GenericEvent:
				builder.AddRange("InvokedListener");
				break;
			case ScriptableType.List:
			case ScriptableType.Dictionary:
				builder.AddRange("ChangedListener");
				break;
			case ScriptableType.Pool:
				builder.AddRange("ChangedCallback");
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scriptableType), scriptableType, null);
		}

		return builder.ToString();
	}
}