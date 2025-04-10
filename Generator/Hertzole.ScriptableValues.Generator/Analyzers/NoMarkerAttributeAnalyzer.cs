using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hertzole.ScriptableValues.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NoMarkerAttributeAnalyzer : DiagnosticAnalyzer
{
	private static readonly ImmutableArray<DiagnosticDescriptor> supportedDiagnostics = ImmutableArray.Create(DiagnosticRules.NoMarkerAttribute);

	private static readonly SyntaxKind[] supportedSyntaxKinds = [SyntaxKind.Attribute];

	/// <inheritdoc />
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
	{
		get { return supportedDiagnostics; }
	}

	/// <inheritdoc />
	public override void Initialize(AnalysisContext context)
	{
		context.EnableConcurrentExecution();
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

		context.RegisterSyntaxNodeAction(Action, supportedSyntaxKinds);
	}

	private static void Action(SyntaxNodeAnalysisContext context)
	{
		if (context.Node is not AttributeSyntax attribute)
		{
			return;
		}

		// Get symbol
		INamedTypeSymbol? attributeSymbol = attribute.GetAttributeSymbol(context.SemanticModel, context.CancellationToken);
		if (attributeSymbol == null)
		{
			return;
		}

		if (!IsGenerateCallbackAttribute(attributeSymbol))
		{
			return;
		}

		// Get the type declaration syntax in parent
		TypeDeclarationSyntax? typeDeclaration = attribute.FirstAncestorOrSelf<TypeDeclarationSyntax>();
		if (typeDeclaration == null)
		{
			return;
		}

		// Get the symbol for the type
		if (context.SemanticModel.GetDeclaredSymbol(typeDeclaration) is not INamedTypeSymbol parentTypeSymbol)
		{
			return;
		}

		// Check if the class has the marker attribute
		if (parentTypeSymbol.GetAttributes().Any(static a =>
			    a.AttributeClass != null && a.AttributeClass.StringEquals("global::Hertzole.ScriptableValues.GenerateScriptableCallbacksAttribute")))
		{
			return;
		}

		context.ReportDiagnostic(Diagnostic.Create(DiagnosticRules.NoMarkerAttribute, attribute.GetLocation()));
	}

	private static bool IsGenerateCallbackAttribute(ITypeSymbol arg)
	{
		string name = arg.ToDisplayString(NullableFlowState.NotNull, SymbolDisplayFormat.FullyQualifiedFormat);

		return string.Equals(name, "global::Hertzole.ScriptableValues.GenerateValueCallbackAttribute", StringComparison.Ordinal) ||
		       string.Equals(name, "global::Hertzole.ScriptableValues.GenerateEventCallbackAttribute", StringComparison.Ordinal) ||
		       string.Equals(name, "global::Hertzole.ScriptableValues.GeneratePoolCallbackAttribute", StringComparison.Ordinal) ||
		       string.Equals(name, "global::Hertzole.ScriptableValues.GenerateCollectionCallbackAttribute", StringComparison.Ordinal);
	}
}