using System;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceProductionContext = Microsoft.CodeAnalysis.SourceProductionContext;
using SymbolDisplayFormat = Microsoft.CodeAnalysis.SymbolDisplayFormat;

namespace Hertzole.ScriptableValues.Generator;

partial class ScriptableCallbackGenerator
{
	private static void GenerateCode(SourceProductionContext context, (HierarchyInfo Key, EquatableArray<CallbackData> Elements) item)
	{
		if (!ScriptableValueHelper.DoesTypeSupportCallbacks((ITypeSymbol) item.Key.Symbol, out _))
		{
			return;
		}

		CodeWriter writer = new CodeWriter();
		CancellationToken cancellationToken = context.CancellationToken;

		try
		{
			writer.AppendFileHeader();
			writer.AppendNamespace(item.Key.Namespace);

			int nestLevel = 0;
			INamedTypeSymbol? containingType = item.Key.Symbol.ContainingType;
			while (containingType != null && !containingType.IsNamespace)
			{
				writer.Append("partial  ");
				writer.Append(containingType.IsValueType ? "struct" : "class");
				writer.AppendLine(containingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
				writer.AppendLine("{");
				writer.Indent++;

				nestLevel++;
				containingType = containingType.ContainingType;
			}

			writer.Append("partial ");

			writer.Append(item.Key.IsStruct ? "struct " : "class ");

			writer.AppendLine(item.Key.TypeName);
			writer.AppendLine("{");
			writer.Indent++;

			WriteElements(writer, in item.Key, in item.Elements, in cancellationToken);

			writer.Indent--;
			writer.Append("}");

			for (int i = 0; i < nestLevel; i++)
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
		in HierarchyInfo hierarchy,
		in EquatableArray<CallbackData> elements,
		in CancellationToken cancellationToken)
	{
		WriteSubscribedBitMask(writer, in hierarchy, in elements, in cancellationToken);
		writer.AppendLine();

		WriteSubscribeAndUnsubscribeMethods(in writer, in hierarchy, in elements, in cancellationToken);
		writer.AppendLine();

		WriteCallbacksMethods(in writer, in elements, in cancellationToken);
	}

	private static void WriteSubscribedBitMask(CodeWriter writer,
		in HierarchyInfo hierarchy,
		in EquatableArray<CallbackData> data,
		in CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		writer.AppendLine(DocumentationHelper.SUBSCRIBED_MASK_SUMMARY);
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

		WriteSubscribedEnumMask(in writer, in data, in cancellationToken);

		writer.AppendLine();
		writer.AppendLine("/// <summary>The current mask of all subscribed callbacks.</summary>");
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

		writer.Append("private SubscribedCallbacksMask subscribedCallbacks");

		if (!hierarchy.IsStruct)
		{
			writer.Append(" = SubscribedCallbacksMask.None");
		}

		writer.AppendLine(";");
	}

	internal static void WriteSubscribedEnumMask(in CodeWriter writer, in EquatableArray<CallbackData> data, in CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		writer.Append("private enum SubscribedCallbacksMask : ");
		if (data.Length <= 8)
		{
			writer.AppendLine("byte");
		}
		else if (data.Length <= 16)
		{
			writer.AppendLine("ushort");
		}
		else if (data.Length <= 32)
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
			cancellationToken.ThrowIfCancellationRequested();

			writer.AppendLine(",");
			writer.Append(data[i].MaskName);
			writer.Append(" = 1 << ");
			writer.Append(i);
		}

		writer.AppendLine();

		writer.Indent--;
		writer.AppendLine("}");
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
			writer.Append("/// <summary>Cached callback for when <see cref=\"");
			writer.Append(data.Name);
			writer.Append("\" /> is ");
			if (data.CallbackType == CallbackType.Event)
			{
				writer.Append("invoked");
			}
			else if (data.CallbackType == CallbackType.Value && (data.Flags & CallbackFlags.PreInvoke) != 0)
			{
				writer.Append("changing");
			}
			else
			{
				writer.Append("changed");
			}

			writer.AppendLine(".</summary>");

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
		in HierarchyInfo hierarchy,
		in EquatableArray<CallbackData> elements,
		in CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		writer.AppendLine("/// <summary>Subscribes to all scriptable callbacks.</summary>");

		WriteGeneratedCodeAttribute(in writer, true);
		writer.AppendExcludeFromCodeCoverageAttribute();

		// If it should be inherited, it should be protected override.
		// If it should not be inherited, and is sealed, it should be private.
		// If it should not be inherited, and is not sealed, it should be protected virtual.
		if (hierarchy.ShouldInherit)
		{
			writer.Append("protected override ");
		}
		else
		{
			writer.Append(hierarchy.IsSealed ? "private " : "protected virtual ");
		}

		writer.AppendLine("void SubscribeToAllScriptableCallbacks()");
		writer.AppendLine("{");
		writer.Indent++;

		if (hierarchy.ShouldInherit)
		{
			writer.AppendLine("base.SubscribeToAllScriptableCallbacks();");
		}

		for (int i = 0; i < elements.Length; i++)
		{
			cancellationToken.ThrowIfCancellationRequested();
			WriteIfCheck(in writer, in elements[i], true);
		}

		writer.Indent--;
		writer.AppendLine("}");
		writer.AppendLine();

		writer.AppendLine("/// <summary>Unsubscribes from all scriptable callbacks.</summary>");

		WriteGeneratedCodeAttribute(in writer, true);
		writer.AppendExcludeFromCodeCoverageAttribute();

		// If it should be inherited, it should be protected override.
		// If it should not be inherited, and is sealed, it should be private.
		// If it should not be inherited, and is not sealed, it should be protected virtual.
		if (hierarchy.ShouldInherit)
		{
			writer.Append("protected override ");
		}
		else
		{
			writer.Append(hierarchy.IsSealed ? "private " : "protected virtual ");
		}

		writer.AppendLine("void UnsubscribeFromAllScriptableCallbacks()");
		writer.AppendLine("{");
		writer.Indent++;

		if (hierarchy.ShouldInherit)
		{
			writer.AppendLine("base.UnsubscribeFromAllScriptableCallbacks();");
		}

		for (int i = 0; i < elements.Length; i++)
		{
			cancellationToken.ThrowIfCancellationRequested();
			WriteIfCheck(in writer, in elements[i], false);
		}

		writer.Indent--;
		writer.AppendLine("}");
		return;

		static void WriteIfCheck(in CodeWriter writer,
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
			writer.Append(data.TargetEvent);
			writer.Append(subscribe ? " += " : " -= ");
			writer.Append(data.CallbackName);
			writer.AppendLine(";");

			if (subscribe)
			{
				writer.Append("subscribedCallbacks |= SubscribedCallbacksMask.");
			}
			else
			{
				writer.Append("subscribedCallbacks &= ~SubscribedCallbacksMask.");
			}

			writer.Append(data.MaskName);
			writer.AppendLine(";");

			writer.Indent--;
			writer.AppendLine("}");
		}
	}

	private static void WriteCallbacksMethods(in CodeWriter writer, in EquatableArray<CallbackData> elements, in CancellationToken cancellationToken)
	{
		ArrayBuilder<(string name, string type)> parametersBuilder = new ArrayBuilder<(string name, string type)>(2);
		ArrayBuilder<(string name, string description)> descriptionsBuilder = new ArrayBuilder<(string name, string description)>(2);

		try
		{
			for (int i = 0; i < elements.Length; i++)
			{
				cancellationToken.ThrowIfCancellationRequested();

				parametersBuilder.Clear();
				descriptionsBuilder.Clear();

				if (i > 0)
				{
					writer.AppendLine();
				}

				elements[i].AppendParameterTypes(in parametersBuilder);
				elements[i].AppendParameterDescriptions(in descriptionsBuilder);

				WriteCallbackMethod(in writer, in elements[i], elements[i].CallbackName.AsSpan(), parametersBuilder.AsSpan(), descriptionsBuilder.AsSpan());
			}
		}
		finally
		{
			parametersBuilder.Dispose();
			descriptionsBuilder.Dispose();
		}

		return;

		static void WriteCallbackMethod(in CodeWriter writer,
			in CallbackData data,
			in ReadOnlySpan<char> name,
			in ReadOnlySpan<(string name, string type)> parameters,
			in ReadOnlySpan<(string argName, string description)> parameterDescriptions)
		{
			writer.AppendLine(DocumentationHelper.GetMethodCallbackDescription(data.Name.AsSpan(), data.CallbackType, data.Flags));

			for (int i = 0; i < parameterDescriptions.Length; i++)
			{
				writer.Append("/// <param name=\"");
				writer.Append(parameterDescriptions[i].argName);
				writer.Append("\">");
				writer.Append(parameterDescriptions[i].description);
				writer.AppendLine("</param>");
			}

			writer.Append("private partial void ");
			writer.Append(name);
			writer.Append("(");

			for (int i = 0; i < parameters.Length; i++)
			{
				if (i > 0)
				{
					writer.Append(", ");
				}

				writer.Append(parameters[i].type);
				writer.Append(" ");
				writer.Append(parameters[i].name);
			}

			writer.AppendLine(");");
		}
	}
}