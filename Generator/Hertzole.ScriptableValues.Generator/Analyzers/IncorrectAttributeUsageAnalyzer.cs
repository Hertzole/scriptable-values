using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hertzole.ScriptableValues.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IncorrectAttributeUsageAnalyzer : DiagnosticAnalyzer
{
	private static readonly ImmutableArray<DiagnosticDescriptor> supportedDescriptors =
		ImmutableArray.Create(DiagnosticRules.IncorrectAttributeUsage);

	private static readonly SyntaxKind[] supportedSyntaxKinds = [SyntaxKind.Attribute];

	/// <inheritdoc />
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
	{
		get { return supportedDescriptors; }
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
		if (context.Node is not AttributeSyntax attributeSyntax)
		{
			return;
		}

		// Get symbol
		INamedTypeSymbol? attributeSymbol = attributeSyntax.GetAttributeSymbol(context.SemanticModel, context.CancellationToken);
		if (attributeSymbol == null || !attributeSymbol.TryGetCallbackAttribute(out CallbackType _))
		{
			// This is not a callback attribute, so we don't need to do anything.
			return;
		}

		// Get member declaration
		MemberDeclarationSyntax? memberDeclaration = attributeSyntax.FirstAncestorOrSelf<MemberDeclarationSyntax>();
		if (memberDeclaration == null)
		{
			return;
		}

		// Get wrapper
		if (!memberDeclaration.TryGetFieldOrPropertyWrapper(context.SemanticModel, out ImmutableArray<FieldOrPropertyWrapper> wrappers) ||
		    wrappers.IsDefaultOrEmpty)
		{
			return;
		}

		FieldOrPropertyWrapper wrapper = wrappers.ItemRef(0);
		if (ScriptableValueHelper.TryGetScriptableType(context.SemanticModel, (INamedTypeSymbol) wrapper.Type, out _, out _))
		{
			// The type is a scriptable type, so we don't need to do anything.
			return;
		}

		context.ReportDiagnostic(Diagnostic.Create(DiagnosticRules.IncorrectAttributeUsage, attributeSyntax.GetLocation(), wrapper.Type.ToDisplayString()));
	}
}