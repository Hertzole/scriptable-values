using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hertzole.ScriptableValues.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class TypeNotSupportedAnalyzer : DiagnosticAnalyzer
{
	private static readonly ImmutableArray<DiagnosticDescriptor> supportedDiagnostics = ImmutableArray.Create(DiagnosticRules.TypeNotSupported);
	private static readonly ImmutableArray<SyntaxKind> supportedSyntaxKinds = ImmutableArray.Create(SyntaxKind.Attribute);

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
		if (attributeSymbol == null || !attributeSymbol.StringEquals(Types.GLOBAL_MARKER_ATTRIBUTE))
		{
			return;
		}

		TypeDeclarationSyntax? containingType = attribute.FirstAncestorOrSelf<TypeDeclarationSyntax>();
		if (containingType == null)
		{
			return;
		}

		if (context.SemanticModel.GetDeclaredSymbol(containingType) is not ITypeSymbol parentTypeSymbol)
		{
			return;
		}

		if (ScriptableValueHelper.DoesTypeSupportCallbacks(in parentTypeSymbol, out NotSupportedReason notSupportedReason))
		{
			// This type is supported, stop here.
			return;
		}

		string type;

		switch (notSupportedReason)
		{
			case NotSupportedReason.ReadOnlyStruct:
				type = "'readonly struct'";
				break;
			default:
				type = "ERROR";
				break;
		}

		context.ReportDiagnostic(Diagnostic.Create(DiagnosticRules.TypeNotSupported, attribute.GetLocation(), type));
	}
}