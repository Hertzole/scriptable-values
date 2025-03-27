using System;
using System.Text;
using Microsoft.CodeAnalysis.Text;
using SourceProductionContext = Microsoft.CodeAnalysis.SourceProductionContext;
using SymbolDisplayFormat = Microsoft.CodeAnalysis.SymbolDisplayFormat;

namespace Hertzole.ScriptableValues.Generator;

partial class ScriptableCallbackGenerator
{
	private static void GenerateCode(SourceProductionContext context, (HierarchyInfo Key, EquatableArray<CallbackData> Elements) item)
	{
		CodeWriter writer = new CodeWriter();

		try
		{
			writer.AppendLine("// <auto-generated/>");
			writer.AppendLine("#nullable enable");

			bool hasNamespace = !string.IsNullOrEmpty(item.Key.Namespace);

			if (hasNamespace)
			{
				writer.Append("namespace ");
				writer.AppendLine(item.Key.Namespace!);
				writer.AppendLine("{");
				writer.Indent++;
			}

			//TODO: Support other types?
			writer.Append("partial class ");
			writer.AppendLine(item.Key.MetadataName);
			writer.AppendLine("{");
			writer.Indent++;

			WriteElements(writer, in context, in item.Key, in item.Elements);

			writer.Indent--;
			writer.Append("}");

			if (hasNamespace)
			{
				writer.AppendLine();
				writer.Indent--;
				writer.Append("}");
			}
		}
		catch (Exception e)
		{
			writer.Clear();
			writer.AppendLine("// Error generating code:");
			writer.AppendLine("// " + e.Message);
			writer.AppendLine("// " + e.StackTrace);
		}

		context.AddSource($"{item.Key.FilenameHint}.g.cs", SourceText.From(writer.ToString(), Encoding.UTF8));
	}

	private static void WriteElements(CodeWriter writer,
		in SourceProductionContext context,
		in HierarchyInfo hierarchy,
		in EquatableArray<CallbackData> elements)
	{
		WriteSubscribedBitMask(writer, in context, in elements);
		writer.AppendLine();

		WriteCachedFields(writer, in context, in hierarchy, in elements);
		writer.AppendLine();

		WriteSubscribeAndUnsubscribeMethods(in writer, in context, in hierarchy, in elements);
		writer.AppendLine();

		WriteCallbacksMethods(in writer, in context, in elements);
	}

	private static void WriteSubscribedBitMask(CodeWriter writer, in SourceProductionContext context, in EquatableArray<CallbackData> data)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

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

		writer.Append("private enum SubscribedCallbacksMask : ");
		if (data.Length < 8)
		{
			writer.AppendLine("byte");
		}
		else if (data.Length < 16)
		{
			writer.AppendLine("ushort");
		}
		else if (data.Length < 32)
		{
			writer.AppendLine("uint");
		}
		else
		{
			writer.AppendLine("ulong");
		}

		writer.AppendLine("{");
		writer.Indent++;

		writer.Append("None = 0");

		for (int i = 0; i < data.Length; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			writer.AppendLine(",");
			writer.Append(data[i].MaskName);
			writer.Append(" = 1 << ");
			writer.Append(i);
		}

		writer.AppendLine();

		writer.Indent--;
		writer.AppendLine("}");

		writer.AppendLine();
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

		writer.AppendLine("private SubscribedCallbacksMask subscribedCallbacks = SubscribedCallbacksMask.None;");
	}

	private static void WriteCachedFields(CodeWriter writer,
		in SourceProductionContext context,
		in HierarchyInfo hierarchy,
		in EquatableArray<CallbackData> data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			WriteField(writer, in hierarchy, in data[i]);
		}

		return;

		static void WriteField(CodeWriter writer, in HierarchyInfo hierarchy, in CallbackData data)
		{
			switch (data.CallbackType)
			{
				case CallbackType.Value:
					WriteValueField(in writer, in hierarchy, in data);
					break;
				case CallbackType.Event:
					WriteEventField(in writer, in hierarchy, in data);
					break;
				case CallbackType.Pool:
					WritePoolField(in writer, in hierarchy, in data);
					break;
				case CallbackType.Collection:
					WriteCollectionField(in writer, in hierarchy, in data);
					break;
			}
		}
	}

	private static void WriteSubscribeAndUnsubscribeMethods(in CodeWriter writer,
		in SourceProductionContext context,
		in HierarchyInfo hierarchy,
		in EquatableArray<CallbackData> elements)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		WriteGeneratedCodeAttribute(in writer, true);
		writer.AppendExcludeFromCodeCoverageAttribute();
		writer.AppendLine("private void SubscribeToScriptableCallbacks()");
		writer.AppendLine("{");
		writer.Indent++;

		for (int i = 0; i < elements.Length; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();
			WriteIfCheck(in writer, in hierarchy, in elements[i], true);
		}

		writer.Indent--;
		writer.AppendLine("}");
		writer.AppendLine();

		WriteGeneratedCodeAttribute(in writer, true);
		writer.AppendExcludeFromCodeCoverageAttribute();
		writer.AppendLine("private void UnsubscribeFromScriptableCallbacks()");
		writer.AppendLine("{");
		writer.Indent++;

		for (int i = 0; i < elements.Length; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();
			WriteIfCheck(in writer, in hierarchy, in elements[i], false);
		}

		writer.Indent--;
		writer.AppendLine("}");
		return;

		static void WriteIfCheck(in CodeWriter writer,
			in HierarchyInfo hierarchy,
			in CallbackData data,
			bool subscribe)
		{
			writer.Append("if ((subscribedCallbacks & SubscribedCallbacksMask.");
			writer.Append(data.MaskName);
			writer.Append(") ");
			writer.Append(subscribe ? "== 0" : "!= 0");
			writer.AppendLine(")");
			writer.AppendLine("{");
			writer.Indent++;

			writer.Append(data.Name);
			writer.Append(".");
			writer.Append(subscribe ? data.RegisterCallbackMethod : data.UnregisterCallbackMethod);

			if (!subscribe)
			{
				writer.Append("<");
				writer.Append(hierarchy.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
				writer.Append(">");
			}

			writer.Append("(");
			writer.Append(data.CachedFieldName);
			if (subscribe)
			{
				writer.AppendLine(", this);");
				writer.Append("subscribedCallbacks |= SubscribedCallbacksMask.");
			}
			else
			{
				writer.AppendLine(");");
				writer.Append("subscribedCallbacks &= ~SubscribedCallbacksMask.");
			}

			writer.Append(data.MaskName);
			writer.AppendLine(";");

			writer.Indent--;
			writer.AppendLine("}");
		}
	}

	private static void WriteCallbacksMethods(in CodeWriter writer, in SourceProductionContext context, in EquatableArray<CallbackData> elements)
	{
		for (int i = 0; i < elements.Length; i++)
		{
			if (i > 0)
			{
				writer.AppendLine();
			}

			context.CancellationToken.ThrowIfCancellationRequested();

			switch (elements[i].CallbackType)
			{
				case CallbackType.Value:
					WriteValueCallbackMethod(in writer, in elements[i]);
					break;
				case CallbackType.Event:
					WriteEventCallbackMethod(in writer, in elements[i]);
					break;
				case CallbackType.Collection:
					WriteCollectionCallbackMethod(in writer, in elements[i]);
					break;
				case CallbackType.Pool:
					WritePoolCallbackMethod(in writer, in elements[i]);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		static void WriteValueCallbackMethod(in CodeWriter writer, in CallbackData data)
		{
			ArrayBuilder<string> parameterTypes = new ArrayBuilder<string>(2);
			ArrayBuilder<string> parameterNames = new ArrayBuilder<string>(2);

			try
			{
				string genericType = data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

				parameterTypes.Add(genericType);
				parameterNames.Add("oldValue");

				parameterTypes.Add(genericType);
				parameterNames.Add("newValue");

				WriteCallbackMethod(in writer, data.CallbackName.AsSpan(), parameterTypes.AsSpan(), parameterNames.AsSpan());
			}
			finally
			{
				parameterTypes.Dispose();
				parameterNames.Dispose();
			}
		}

		static void WriteEventCallbackMethod(in CodeWriter writer, in CallbackData data)
		{
			ArrayBuilder<string> parameterTypes = new ArrayBuilder<string>(2);
			ArrayBuilder<string> parameterNames = new ArrayBuilder<string>(2);

			try
			{
				parameterTypes.Add("object");
				parameterNames.Add("sender");

				if (data.ScriptableType == ScriptableType.GenericEvent)
				{
					string genericType = data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
					parameterTypes.Add(genericType);
				}
				else
				{
					parameterTypes.Add("global::System.EventArgs");
				}

				parameterNames.Add("args");

				WriteCallbackMethod(in writer, data.CallbackName.AsSpan(), parameterTypes.AsSpan(), parameterNames.AsSpan());
			}
			finally
			{
				parameterTypes.Dispose();
				parameterNames.Dispose();
			}
		}

		static void WriteCollectionCallbackMethod(in CodeWriter writer, in CallbackData data)
		{
			ArrayBuilder<string> parameterTypes = new ArrayBuilder<string>(1);
			ArrayBuilder<string> parameterNames = new ArrayBuilder<string>(1);
			ArrayBuilder<char> nameBuilder = new ArrayBuilder<char>(64);

			try
			{
				nameBuilder.AddRange("global::Hertzole.ScriptableValues.CollectionChangedArgs<");
				nameBuilder.AddRange(data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
				nameBuilder.Add('>');

				parameterTypes.Add(nameBuilder.ToString());
				parameterNames.Add("args");

				WriteCallbackMethod(in writer, data.CallbackName.AsSpan(), parameterTypes.AsSpan(), parameterNames.AsSpan());
			}
			finally
			{
				parameterTypes.Dispose();
				parameterNames.Dispose();
				nameBuilder.Dispose();
			}
		}

		static void WritePoolCallbackMethod(in CodeWriter writer, in CallbackData data)
		{
			ArrayBuilder<string> parameterTypes = new ArrayBuilder<string>(2);
			ArrayBuilder<string> parameterNames = new ArrayBuilder<string>(2);

			try
			{
				parameterTypes.Add("global::Hertzole.ScriptableValues.PoolAction");
				parameterNames.Add("action");

				parameterTypes.Add(data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
				parameterNames.Add("item");

				WriteCallbackMethod(in writer, data.CallbackName.AsSpan(), parameterTypes.AsSpan(), parameterNames.AsSpan());
			}
			finally
			{
				parameterTypes.Dispose();
				parameterNames.Dispose();
			}
		}

		static void WriteCallbackMethod(in CodeWriter writer,
			in ReadOnlySpan<char> name,
			in ReadOnlySpan<string> parameterTypes,
			in ReadOnlySpan<string> parameterNames)
		{
			writer.Append("partial void ");
			writer.Append(name);
			writer.Append("(");

			for (int i = 0; i < parameterTypes.Length; i++)
			{
				if (i > 0)
				{
					writer.Append(", ");
				}

				writer.Append(parameterTypes[i]);
				writer.Append(" ");
				writer.Append(parameterNames[i]);
			}

			writer.AppendLine(");");
		}
	}
}