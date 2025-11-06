using System;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

[StructLayout(LayoutKind.Auto)]
internal readonly struct CallbackData : IEquatable<CallbackData>
{
    public readonly string Name;
    public readonly CallbackType CallbackType;
    public readonly CallbackFlags Flags;
    public readonly ScriptableType ScriptableType;
    public readonly ITypeSymbol GenericType;

    public string MaskName { get; }
    public string CachedFieldName { get; }

    public string TargetEvent { get; }

    public string CallbackName { get; }
    public string RegisterCallbackMethod { get; }

    public string UnregisterCallbackMethod { get; }

    public CallbackData(string name,
        CallbackType callbackType,
        CallbackFlags flags,
        ISymbol memberType,
        ScriptableType scriptableType,
        ITypeSymbol genericType,
        string? callbackName)
    {
        Name = name;
        CallbackType = callbackType;
        Flags = flags;
        ScriptableType = scriptableType;
        GenericType = genericType;
        MaskName = CreateMaskName(name, in scriptableType, in flags);
        CachedFieldName = CreateCachedFieldName(name, in scriptableType, in flags);
        TargetEvent = CreateTargetEventName(in scriptableType, in flags);
        RegisterCallbackMethod = CreateRegisterMethodName(true, in scriptableType, in flags);
        UnregisterCallbackMethod = CreateRegisterMethodName(false, in scriptableType, in flags);
        CallbackName = string.IsNullOrEmpty(callbackName) ? Naming.CreateCallbackName(name, in scriptableType, in flags) : callbackName!;
    }

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
                builder.Add(("sender", "object"));
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
                builder.Add(("action", "global::Hertzole.ScriptableValues.PoolAction"));
                builder.Add(("item", genericType));
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
                builder.Add(("action", DocumentationHelper.POOL_ACTION));
                builder.Add(("item", DocumentationHelper.POOL_ITEM));
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

    private static string CreateTargetEventName(in ScriptableType scriptableType, in CallbackFlags flags)
    {
        using ArrayBuilder<char> builder = new ArrayBuilder<char>(32);
        builder.AddRange("On");
        switch (scriptableType)
        {
            case ScriptableType.Event:
            case ScriptableType.GenericEvent:
                builder.AddRange("Invoked");
                break;
            case ScriptableType.Value:
                builder.AddRange("Value");
                Naming.AppendFlagSuffix(in builder, in scriptableType, in flags);
                break;
            case ScriptableType.Pool:
                builder.AddRange("PoolChanged");
                break;
            case ScriptableType.List:
            case ScriptableType.Dictionary:
                builder.AddRange("CollectionChanged");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scriptableType), scriptableType, null);
        }

        return builder.ToString();
    }

    /// <inheritdoc />
    public bool Equals(CallbackData other)
    {
        return CallbackType == other.CallbackType && Flags == other.Flags && ScriptableType == other.ScriptableType && Name == other.Name &&
               GenericType.Equals(other.GenericType, SymbolEqualityComparer.Default) && MaskName == other.MaskName &&
               CachedFieldName == other.CachedFieldName && TargetEvent == other.TargetEvent && CallbackName == other.CallbackName &&
               RegisterCallbackMethod == other.RegisterCallbackMethod && UnregisterCallbackMethod == other.UnregisterCallbackMethod;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is CallbackData other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ (int) CallbackType;
            hashCode = (hashCode * 397) ^ (int) Flags;
            hashCode = (hashCode * 397) ^ (int) ScriptableType;
            hashCode = (hashCode * 397) ^ SymbolEqualityComparer.Default.GetHashCode(GenericType);
            hashCode = (hashCode * 397) ^ MaskName.GetHashCode();
            hashCode = (hashCode * 397) ^ CachedFieldName.GetHashCode();
            hashCode = (hashCode * 397) ^ TargetEvent.GetHashCode();
            hashCode = (hashCode * 397) ^ CallbackName.GetHashCode();
            hashCode = (hashCode * 397) ^ RegisterCallbackMethod.GetHashCode();
            hashCode = (hashCode * 397) ^ UnregisterCallbackMethod.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(CallbackData left, CallbackData right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CallbackData left, CallbackData right)
    {
        return !left.Equals(right);
    }
}