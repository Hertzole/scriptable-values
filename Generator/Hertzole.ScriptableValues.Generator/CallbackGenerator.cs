using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Hertzole.ScriptableValues.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class CallbackGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValueProvider<ImmutableArray<GenerateTypeArguments>> provider = context.SyntaxProvider
		                                                                                  .ForAttributeWithMetadataName(
			                                                                                  "Hertzole.ScriptableValues.GenerateCallbackAttribute",
			                                                                                  (node, token) =>
				                                                                                  true, // No matter what I do, this will always return false. So just return true. ¯\_(ツ)_/¯
			                                                                                  Get)
		                                                                                  .Collect();

		// IncrementalValueProvider<ImmutableArray<GenerateTypeArguments>> provider = context.SyntaxProvider
		//                                                                                   .CreateSyntaxProvider(
		// 	                                                                                  IsValidDeclaration,
		// 	                                                                                  Get)
		//                                                                                   .Collect();

		context.RegisterImplementationSourceOutput(provider, GenerateCode);
	}

	/// <summary>
	///     Determines whether the node is a valid declaration and possible candidate for the [GenerateCallback] attribute.
	/// </summary>
	/// <returns>True if the node is a valid candidate. Otherwise, false.</returns>
	private static bool IsValidDeclaration(SyntaxNode node, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		if (node is not TypeDeclarationSyntax typeDeclaration)
		{
			return false;
		}

		if (typeDeclaration.Members.Count == 0)
		{
			return false;
		}

		for (int i = 0; i < typeDeclaration.Members.Count; i++)
		{
			// If it's a field or property, it's a valid declaration.
			if (typeDeclaration.Members[i] is FieldDeclarationSyntax or PropertyDeclarationSyntax)
			{
				return true;
			}
		}

		return false;
	}

	private static GenerateTypeArguments Get(GeneratorSyntaxContext context, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		GenerateFlags flags = GenerateFlags.None;
		
		bool hasPostInvoke = false;
		bool hasPreInvoke = false;
		
		var typeDeclaration = (TypeDeclarationSyntax)context.Node;

		for (int i = 0; i < typeDeclaration.Members.Count; i++)
		{
			if (typeDeclaration.Members[i] is FieldDeclarationSyntax fieldSyntax)
			{
				
			}
			else if (typeDeclaration.Members[i] is PropertyDeclarationSyntax propertySyntax)
			{
				
			}
		}

		return default;
	}

	private static GenerateTypeArguments Get(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		GenerateFlags flags = GenerateFlags.None;

		bool hasPostInvoke = false;
		bool hasPreInvoke = false;

		foreach (AttributeData attribute in context.Attributes)
		{
			// No arguments means it's a post invoke.
			if (attribute.ConstructorArguments.Length == 0)
			{
				hasPostInvoke = true;
				continue;
			}

			if (attribute.ConstructorArguments.Length > 0)
			{
				// If the argument is 0, it's a pre invoke.
				if (attribute.ConstructorArguments[0].Value is int value && value == 0)
				{
					hasPreInvoke = true;
				}
				else
				{
					hasPostInvoke = true;
				}
			}
		}

		if (hasPostInvoke)
		{
			flags |= GenerateFlags.PostInvoke;
		}

		if (hasPreInvoke)
		{
			flags |= GenerateFlags.PreInvoke;
		}

		ITypeSymbol memberType;

		switch (context.TargetSymbol)
		{
			case IFieldSymbol field:
				memberType = field.Type;
				break;
			case IPropertySymbol property:
				memberType = property.Type;
				break;
			default:
				throw new InvalidOperationException("Invalid target symbol.");
		}

		return new GenerateTypeArguments
		{
			ParentType = context.TargetSymbol.ContainingType,
			TargetSymbol = context.TargetSymbol,
			MemberType = memberType,
			GenerateFlags = flags
		};
	}

	private static void GenerateCode(SourceProductionContext context, ImmutableArray<GenerateTypeArguments> data)
	{
		ImmutableDictionary<INamedTypeSymbol, ImmutableArray<GenerateTypeArguments>> parents = GetParentTypes(data);

		CodeWriter writer = new CodeWriter();

		foreach (KeyValuePair<INamedTypeSymbol, ImmutableArray<GenerateTypeArguments>> pair in parents)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			try
			{
				writer.Clear();
				if (!pair.Key.ContainingNamespace.IsGlobalNamespace)
				{
					writer.Append("namespace ");
					writer.AppendLine(pair.Key.ContainingNamespace.ToDisplayString());
					writer.AppendLine("{");
					writer.Indent++;
				}

				writer.Append("partial class ");
				writer.AppendLine(pair.Key.Name);
				writer.AppendLine("{");
				writer.Indent++;

				WriteSubscribedBitMask(writer, in context, pair.Value);
				writer.AppendLine();

				WriteCachedFields(writer, in context, pair.Value);
				writer.AppendLine();

				WriteSubscribeAndUnsubscribeMethods(writer, in context, pair.Value);
				writer.AppendLine();

				WriteCallbacksMethods(writer, in context, pair.Value);

				writer.Indent--;
				writer.AppendLine("}");

				if (!pair.Key.ContainingNamespace.IsGlobalNamespace)
				{
					writer.Indent--;
					writer.AppendLine("}");
				}

				context.AddSource(pair.Key.Name + ".g.cs", SourceText.From(writer.ToString(), Encoding.UTF8));
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
		if(data.Length < 8)
		{
			writer.AppendLine("byte");
		}
		else if(data.Length < 16)
		{
			writer.AppendLine("ushort");
		}
		else if(data.Length < 32)
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

	private static void WriteSubscribeAndUnsubscribeMethods(CodeWriter writer, in SourceProductionContext context, ImmutableArray<GenerateTypeArguments> data)
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

	private static ImmutableDictionary<INamedTypeSymbol, ImmutableArray<GenerateTypeArguments>> GetParentTypes(in ImmutableArray<GenerateTypeArguments> data)
	{
		ArrayDictionaryBuilder<INamedTypeSymbol, GenerateTypeArguments> builder =
			new ArrayDictionaryBuilder<INamedTypeSymbol, GenerateTypeArguments>(SymbolEqualityComparer.Default);

		ReadOnlySpan<GenerateTypeArguments> span = data.AsSpan();
		for (int i = 0; i < span.Length; i++)
		{
			GenerateTypeArguments generateTypeArguments = span[i];
			builder.Add(generateTypeArguments.ParentType, generateTypeArguments);
		}

		return builder.ToImmutable();
	}
}

public readonly record struct GenerateTypeArguments
{
	public INamedTypeSymbol ParentType { get; init; }
	public ISymbol TargetSymbol { get; init; }
	public GenerateFlags GenerateFlags { get; init; }
	public ISymbol MemberType { get; init; }

	public ReadOnlySpan<char> TargetSymbolName
	{
		get { return TargetSymbol.Name.AsSpan(); }
	}

	public void AppendCallbackName(CodeWriter writer)
	{
		writer.Append("__");
		writer.Append(TargetSymbol.Name);
		writer.Append("ScriptableValueCallback");
	}
}

[Flags]
public enum GenerateFlags
{
	None = 0,
	PreInvoke = 1,
	PostInvoke = 2
}