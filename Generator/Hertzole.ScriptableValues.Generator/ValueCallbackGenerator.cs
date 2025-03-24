using System;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Hertzole.ScriptableValues.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class ValueCallbackGenerator : IIncrementalGenerator
{
	/// <inheritdoc />
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValueProvider<ImmutableArray<GenerateType>> provider = context.SyntaxProvider
		                                                                         .ForAttributeWithMetadataName(
			                                                                         "Hertzole.ScriptableValues.GenerateScriptableCallbacksAttribute",
			                                                                         IsValidDeclaration,
			                                                                         Get)
		                                                                         .Collect();

		context.RegisterImplementationSourceOutput(provider, GenerateCode);
	}

	private static bool IsValidDeclaration(SyntaxNode node, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		if (node is not TypeDeclarationSyntax typeDeclaration)
		{
			return false;
		}

		SyntaxList<MemberDeclarationSyntax> members = typeDeclaration.Members;

		if (members.Count == 0)
		{
			return false;
		}

		for (int i = 0; i < members.Count; i++)
		{
			if (members[i] is not (FieldDeclarationSyntax or PropertyDeclarationSyntax))
			{
				continue;
			}

			if (members[i].AttributeLists.Count > 0)
			{
				return true;
			}
		}

		return false;
	}

	private static GenerateType Get(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
		{
			return default;
		}

		ImmutableArray<ISymbol> members = typeSymbol.GetMembers();
		using ArrayBuilder<GenerateTypeArguments> arguments = new ArrayBuilder<GenerateTypeArguments>();

		for (int i = 0; i < members.Length; i++)
		{
			cancellationToken.ThrowIfCancellationRequested();

			ITypeSymbol memberType;

			switch (members[i])
			{
				case IFieldSymbol field:
					if (field.Name.EndsWith("k__BackingField"))
					{
						continue;
					}

					memberType = field.Type;
					break;
				case IPropertySymbol property:
					memberType = property.Type;
					break;
				default:
					continue;
			}

			ImmutableArray<AttributeData> attributes = members[i].GetAttributes();
			if (attributes.IsDefaultOrEmpty)
			{
				continue;
			}

			foreach (AttributeData attribute in attributes)
			{
				cancellationToken.ThrowIfCancellationRequested();

				INamedTypeSymbol? c = attribute.AttributeClass;

				if (c == null)
				{
					continue;
				}

				if (c.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat) !=
				    "global::Hertzole.ScriptableValues.GenerateValueCallbackAttribute")
				{
					continue;
				}

				bool hasChanged = false;
				bool hasChanging = false;
				GenerateFlags flags = GenerateFlags.None;

				// No arguments means it's a OnXChanged
				if (attribute.ConstructorArguments.Length == 0)
				{
					hasChanged = true;
				}
				else if (attribute.ConstructorArguments.Length > 0)
				{
					// If the argument is 0, it's a OnXChanging.
					if (attribute.ConstructorArguments[0].Value is int value && value == 0)
					{
						hasChanging = true;
					}
					else
					{
						hasChanged = true;
					}
				}

				if (hasChanging)
				{
					flags |= GenerateFlags.PreInvoke;
				}

				if (hasChanged)
				{
					flags |= GenerateFlags.PostInvoke;
				}

				if (flags == GenerateFlags.None)
				{
					continue;
				}

				arguments.Add(new GenerateTypeArguments
				{
					ParentType = typeSymbol,
					TargetSymbol = members[i],
					MemberType = memberType,
					GenerateFlags = flags
				});
			}
		}

		return new GenerateType
		{
			ParentType = typeSymbol,
			Arguments = arguments.ToImmutable()
		};
	}

	private static void GenerateCode(SourceProductionContext context, ImmutableArray<GenerateType> data)
	{
		CodeWriter writer = new CodeWriter();

		foreach (GenerateType type in data)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			try
			{
				bool hasGlobalNamespace = type.ParentType.ContainingNamespace.IsGlobalNamespace;

				writer.Clear();

				if (!hasGlobalNamespace)
				{
					writer.Append("namespace ");
					writer.AppendLine(type.ParentType.ContainingNamespace.ToDisplayString());
					writer.AppendLine("{");
					writer.Indent++;
				}

				writer.Append("partial class ");
				writer.AppendLine(type.ParentType.Name);
				writer.AppendLine("{");
				writer.Indent++;

				ImmutableArray<GenerateTypeArguments> arguments = type.Arguments;

				WriteSubscribedBitMask(writer, in context, in arguments);
				writer.AppendLine();

				WriteCachedFields(writer, in context, in arguments);
				writer.AppendLine();

				WriteSubscribeAndUnsubscribeMethods(writer, in context, in arguments);
				writer.AppendLine();

				WriteCallbacksMethods(writer, in context, in arguments);

				writer.Indent--;
				writer.AppendLine("}");

				if (!hasGlobalNamespace)
				{
					writer.Indent--;
					writer.AppendLine("}");
				}

				context.AddSource(type.ParentType.Name + ".g.cs", SourceText.From(writer.ToString(), Encoding.UTF8));
			}

			catch (Exception e)
			{
				writer.Clear();
				writer.AppendLine("// Error: " + e.Message);

				context.AddSource("GENERATED_ERRORS.g.cs", SourceText.From(writer.ToString(), Encoding.UTF8));
			}
		}
	}

	private static void WriteSubscribedBitMask(CodeWriter writer, in SourceProductionContext context, in ImmutableArray<GenerateTypeArguments> data)
	{
		context.CancellationToken.ThrowIfCancellationRequested();
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

		ReadOnlySpan<GenerateTypeArguments> span = data.AsSpan();
		for (int i = 0; i < span.Length; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			writer.AppendLine(",");
			writer.Append(span[i].TargetSymbolName);
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
		using (writer.WithIndent(0))
		{
			writer.AppendLine("#endif // UNITY_EDITOR");
		}

		writer.AppendLine("private SubscribedCallbacksMask subscribedCallbacks = SubscribedCallbacksMask.None;");
	}

	private static void WriteCachedFields(CodeWriter writer, in SourceProductionContext context, in ImmutableArray<GenerateTypeArguments> data)
	{
		ReadOnlySpan<GenerateTypeArguments> span = data.AsSpan();
		for (int i = 0; i < span.Length; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			if (span[i].MemberType is not INamedTypeSymbol namedType)
			{
				writer.Append("// Error: Could not get named type from ");
				writer.Append(span[i].TargetSymbol.Name);
				writer.Append(" (is ");
				writer.Append(span[i].MemberType.GetType().Name);
				writer.AppendLine(")");
				continue;
			}

			if (!ScriptableValueHelper.TryGetScriptableType(namedType, out ScriptableType type, out ITypeSymbol? genericType))
			{
				writer.Append("// Error: Could not get scriptable type from ");
				writer.Append(span[i].TargetSymbol.Name);
				writer.Append(" | ");
				writer.Append(type.ToString());
				writer.Append(" | ");
				writer.AppendLine(genericType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) ?? "null");
				continue;
			}

			// Generic type is not null if the scriptable type could be found.
			string genericTypeName = genericType!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

			if ((span[i].GenerateFlags & GenerateFlags.PreInvoke) != 0)
			{
				WriteField(writer, span[i], genericTypeName, "Changing");
			}

			if ((span[i].GenerateFlags & GenerateFlags.PostInvoke) != 0)
			{
				WriteField(writer, span[i], genericTypeName, "Changed");
			}
		}

		return;

		static void WriteField(CodeWriter writer, in GenerateTypeArguments data, string genericTypeName, string suffix)
		{
			using (writer.WithIndent(0))
			{
				writer.AppendLine("#if UNITY_EDITOR");
			}

			writer.AppendLine("[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
			using (writer.WithIndent(0))
			{
				writer.AppendLine("#endif // UNITY_EDITOR");
			}

			writer.Append("private static global::System.Action<");
			writer.Append(genericTypeName);
			writer.Append(", ");
			writer.Append(genericTypeName);
			writer.Append(", ");
			writer.Append(data.ParentType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
			writer.Append("> ");
			data.AppendCallbackName(writer);
			writer.Append(suffix);
			writer.Append(" = (oldValue, newValue, context) => { context.On");
			writer.Append(Naming.FormatVariableName(data.TargetSymbolName));
			writer.Append(suffix);
			writer.AppendLine("(oldValue, newValue); };");
		}
	}

	private static void WriteSubscribeAndUnsubscribeMethods(CodeWriter writer,
		in SourceProductionContext context,
		in ImmutableArray<GenerateTypeArguments> data)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		writer.AppendLine("private void SubscribeToCallbacks()");
		writer.AppendLine("{");
		writer.Indent++;

		for (int i = 0; i < data.Length; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			writer.Append("if ((subscribedCallbacks & SubscribedCallbacksMask.");
			writer.Append(data[i].TargetSymbolName);
			writer.AppendLine(") == 0)");
			writer.AppendLine("{");
			writer.Indent++;

			if ((data[i].GenerateFlags & GenerateFlags.PreInvoke) != 0)
			{
				WriteRegisterMethod(writer, data[i], "Changing");
			}

			if ((data[i].GenerateFlags & GenerateFlags.PostInvoke) != 0)
			{
				WriteRegisterMethod(writer, data[i], "Changed");
			}

			writer.Append("subscribedCallbacks |= SubscribedCallbacksMask.");
			writer.Append(data[i].TargetSymbolName);
			writer.AppendLine(";");

			writer.Indent--;
			writer.AppendLine("}");
		}

		writer.Indent--;
		writer.AppendLine("}");
		writer.AppendLine();

		writer.AppendLine("private void UnsubscribeFromCallbacks()");
		writer.AppendLine("{");
		writer.Indent++;

		for (int i = 0; i < data.Length; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			writer.Append("if ((subscribedCallbacks & SubscribedCallbacksMask.");
			writer.Append(data[i].TargetSymbolName);
			writer.AppendLine(") != 0)");
			writer.AppendLine("{");
			writer.Indent++;

			if ((data[i].GenerateFlags & GenerateFlags.PreInvoke) != 0)
			{
				WriteUnregisterMethod(writer, data[i], "Changing");
			}

			if ((data[i].GenerateFlags & GenerateFlags.PostInvoke) != 0)
			{
				WriteUnregisterMethod(writer, data[i], "Changed");
			}

			writer.Append("subscribedCallbacks &= ~SubscribedCallbacksMask.");
			writer.Append(data[i].TargetSymbolName);
			writer.AppendLine(";");

			writer.Indent--;
			writer.AppendLine("}");
		}

		writer.Indent--;
		writer.AppendLine("}");
		return;

		static void WriteRegisterMethod(CodeWriter writer, in GenerateTypeArguments data, string suffix)
		{
			writer.Append(data.TargetSymbolName);
			writer.Append(".RegisterValue");
			writer.Append(suffix);
			writer.Append("Listener(");
			data.AppendCallbackName(writer);
			writer.Append(suffix);
			writer.Append(", this);");
			writer.AppendLine();
		}

		static void WriteUnregisterMethod(CodeWriter writer, in GenerateTypeArguments data, string suffix)
		{
			writer.Append(data.TargetSymbolName);
			writer.Append(".UnregisterValue");
			writer.Append(suffix);
			writer.Append("Listener(");
			data.AppendCallbackName(writer);
			writer.Append(suffix);
			writer.Append(");");
			writer.AppendLine();
		}
	}

	private static void WriteCallbacksMethods(CodeWriter writer, in SourceProductionContext context, in ImmutableArray<GenerateTypeArguments> data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			if (i > 0)
			{
				writer.AppendLine();
			}

			context.CancellationToken.ThrowIfCancellationRequested();

			if ((data[i].GenerateFlags & GenerateFlags.PreInvoke) != 0)
			{
				WriteCallbackMethod(writer, in context, data[i], "Changing");

				// If there's also a Changed method, append a line in between.
				if ((data[i].GenerateFlags & GenerateFlags.PostInvoke) != 0)
				{
					writer.AppendLine();
				}
			}

			if ((data[i].GenerateFlags & GenerateFlags.PostInvoke) != 0)
			{
				WriteCallbackMethod(writer, in context, data[i], "Changed");
			}
		}
	}

	private static void WriteCallbackMethod(CodeWriter writer, in SourceProductionContext context, GenerateTypeArguments data, string suffix)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		if (data.MemberType is not INamedTypeSymbol namedType)
		{
			writer.Append("// Error: Could not get named type from ");
			writer.Append(data.TargetSymbol.Name);
			writer.Append(" (is ");
			writer.Append(data.MemberType.GetType().Name);
			writer.AppendLine(")");
			return;
		}

		if (!ScriptableValueHelper.TryGetScriptableType(namedType, out ScriptableType type, out ITypeSymbol? genericType))
		{
			writer.Append("// Error: Could not get scriptable type from ");
			writer.Append(data.TargetSymbol.Name);
			writer.Append(" | ");
			writer.Append(type.ToString());
			writer.Append(" | ");
			writer.AppendLine(genericType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) ?? "null");
			return;
		}

		string genericTypeName = genericType!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		writer.Append("private partial void On");
		writer.Append(Naming.FormatVariableName(data.TargetSymbolName));
		writer.Append(suffix);
		writer.Append("(");
		writer.Append(genericTypeName);
		writer.Append(" oldValue, ");
		writer.Append(genericTypeName);
		writer.AppendLine(" newValue);");
	}
}

public readonly record struct GenerateType
{
	public INamedTypeSymbol ParentType { get; init; }
	public ImmutableArray<GenerateTypeArguments> Arguments { get; init; }
}