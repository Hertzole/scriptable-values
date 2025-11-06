using System;
using SymbolDisplayFormat = Microsoft.CodeAnalysis.SymbolDisplayFormat;

namespace Hertzole.ScriptableValues.Generator;

partial class ScriptableCallbackGenerator
{
    private static void WriteValueField(in CodeWriter writer, in HierarchyInfo hierarchy, in CallbackData data)
    {
        string genericTypeName = data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        ArrayBuilder<string> genericParameters = new ArrayBuilder<string>(3);
        ArrayBuilder<string> callbackParameters = new ArrayBuilder<string>(3);
        ArrayBuilder<char> callback = new ArrayBuilder<char>(64);

        try
        {
            genericParameters.Add(genericTypeName);
            genericParameters.Add(genericTypeName);
            genericParameters.Add(hierarchy.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

            callbackParameters.Add("oldValue");
            callbackParameters.Add("newValue");
            callbackParameters.Add("context");

            callback.AddRange("context.");
            callback.AddRange(data.CallbackName);
            callback.AddRange("(oldValue, newValue);");

            WriteCachedField(in writer, "global::System.Action", genericParameters.AsSpan(), data.CachedFieldName.AsSpan(), callbackParameters.AsSpan(),
                callback.AsSpan());
        }
        finally
        {
            // Dispose manually instead of using 'using' to avoid creating a bunch of try blocks.
            genericParameters.Dispose();
            callbackParameters.Dispose();
            callback.Dispose();
        }
    }

    private static void WriteEventField(in CodeWriter writer, in HierarchyInfo hierarchy, in CallbackData data)
    {
        ArrayBuilder<string> genericParameters = new ArrayBuilder<string>(3);
        ArrayBuilder<string> callbackParameters = new ArrayBuilder<string>(3);
        ArrayBuilder<char> callback = new ArrayBuilder<char>(64);

        try
        {
            // If it's a generic event, write the generic type. 
            if (data.ScriptableType == ScriptableType.GenericEvent)
            {
                genericParameters.Add(data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            }

            genericParameters.Add(hierarchy.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

            callbackParameters.Add("sender");
            callbackParameters.Add("args");
            callbackParameters.Add("context");

            callback.AddRange("context.");
            callback.AddRange(data.CallbackName);
            callback.AddRange("(sender, args);");

            WriteCachedField(in writer, "global::Hertzole.ScriptableValues.EventHandlerWithContext", genericParameters.AsSpan(), data.CachedFieldName.AsSpan(),
                callbackParameters.AsSpan(), callback.AsSpan());
        }
        finally
        {
            genericParameters.Dispose();
            callbackParameters.Dispose();
            callback.Dispose();
        }
    }

    private static void WritePoolField(in CodeWriter writer, in HierarchyInfo hierarchy, in CallbackData data)
    {
        ArrayBuilder<string> genericParameters = new ArrayBuilder<string>(3);
        ArrayBuilder<string> callbackParameters = new ArrayBuilder<string>(3);
        ArrayBuilder<char> callback = new ArrayBuilder<char>(64);

        try
        {
            genericParameters.Add(data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            genericParameters.Add(hierarchy.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

            callbackParameters.Add("action");
            callbackParameters.Add("item");
            callbackParameters.Add("context");

            callback.AddRange("context.");
            callback.AddRange(data.CallbackName);
            callback.AddRange("(action, item);");

            WriteCachedField(in writer, "global::Hertzole.ScriptableValues.PoolEventArgsWithContext", genericParameters.AsSpan(), data.CachedFieldName.AsSpan(),
                callbackParameters.AsSpan(), callback.AsSpan());
        }
        finally
        {
            genericParameters.Dispose();
            callbackParameters.Dispose();
            callback.Dispose();
        }
    }

    private static void WriteCollectionField(in CodeWriter writer, in HierarchyInfo hierarchy, in CallbackData data)
    {
        ArrayBuilder<string> genericParameters = new ArrayBuilder<string>(3);
        ArrayBuilder<string> callbackParameters = new ArrayBuilder<string>(3);
        ArrayBuilder<char> callback = new ArrayBuilder<char>(64);

        try
        {
            genericParameters.Add(data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            genericParameters.Add(hierarchy.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

            callbackParameters.Add("args");
            callbackParameters.Add("context");

            callback.AddRange("context.");
            callback.AddRange(data.CallbackName);
            callback.AddRange("(args);");

            WriteCachedField(in writer, "global::Hertzole.ScriptableValues.CollectionChangedWithContextEventHandler", genericParameters.AsSpan(),
                data.CachedFieldName.AsSpan(),
                callbackParameters.AsSpan(), callback.AsSpan());
        }
        finally
        {
            genericParameters.Dispose();
            callbackParameters.Dispose();
            callback.Dispose();
        }
    }

    private static void WriteCachedField(in CodeWriter writer,
        string action,
        in ReadOnlySpan<string> genericParameters,
        in ReadOnlySpan<char> name,
        in ReadOnlySpan<string> callbackParameters,
        in ReadOnlySpan<char> callback)
    {
        using (writer.WithIndent(0))
        {
            writer.AppendLine("#if UNITY_EDITOR");
        }

        writer.AppendLine("[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
        WriteGeneratedCodeAttribute(writer);
        using (writer.WithIndent(0))
        {
            writer.AppendLine("#endif // UNITY_EDITOR");
        }

        writer.Append("private static ");
        writer.Append(action);
        writer.Append("<");
        for (int i = 0; i < genericParameters.Length; i++)
        {
            if (i > 0)
            {
                writer.Append(", ");
            }

            writer.Append(genericParameters[i]);
        }

        writer.Append("> ");
        writer.Append(name);
        writer.Append(" = (");

        for (int i = 0; i < callbackParameters.Length; i++)
        {
            if (i > 0)
            {
                writer.Append(", ");
            }

            writer.Append(callbackParameters[i]);
        }

        writer.Append(") => { ");
        writer.Append(callback);
        writer.AppendLine(" };");
    }

    private static void WriteGeneratedCodeAttribute(in CodeWriter writer, bool withIfDefines = false)
    {
        if (withIfDefines)
        {
            using (writer.WithIndent(0))
            {
                writer.AppendLine("#if UNITY_EDITOR");
            }
        }

        writer.AppendGeneratedCodeAttribute(generatorName, generatorVersion);

        if (withIfDefines)
        {
            using (writer.WithIndent(0))
            {
                writer.AppendLine("#endif // UNITY_EDITOR");
            }
        }
    }
}