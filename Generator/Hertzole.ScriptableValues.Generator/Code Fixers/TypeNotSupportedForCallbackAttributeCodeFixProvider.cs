using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;

namespace Hertzole.ScriptableValues.Generator;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TypeNotSupportedForCallbackAttributeCodeFixProvider))]
[Shared]
public sealed class TypeNotSupportedForCallbackAttributeCodeFixProvider : CodeFixProvider
{
	private static readonly ImmutableArray<string> fixableDiagnosticIds = ImmutableArray.Create(DiagnosticIdentifiers.TypeNotSupportedForCallbackAttribute);

	/// <inheritdoc />
	public override ImmutableArray<string> FixableDiagnosticIds
	{
		get { return fixableDiagnosticIds; }
	}

	/// <inheritdoc />
	public override FixAllProvider? GetFixAllProvider()
	{
		return WellKnownFixAllProviders.BatchFixer;
	}

	/// <inheritdoc />
	public override Task RegisterCodeFixesAsync(CodeFixContext context)
	{
		Diagnostic diagnostic = context.Diagnostics[0];
		if (!diagnostic.Properties.TryGetValue("correctAttributeName", out string? attributeName) ||
		    string.IsNullOrEmpty(attributeName))
		{
			return Task.CompletedTask;
		}

		context.RegisterCodeFix(CodeAction.Create(
			string.Format(Resources.HSV0100CodeFixTitle, attributeName),
			token => CreateChangedDocument(context, token),
			nameof(Resources.HSV0100CodeFixTitle)), diagnostic);

		return Task.CompletedTask;
	}

	private static async Task<Document> CreateChangedDocument(CodeFixContext context, CancellationToken cancellationToken)
	{
		Diagnostic diagnostic = context.Diagnostics[0];
		if (!diagnostic.Properties.TryGetValue("correctAttributeName", out string? attributeName) ||
		    string.IsNullOrEmpty(attributeName))
		{
			return context.Document;
		}

		SyntaxNode? root = await context.Document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
		if (root == null)
		{
			return context.Document;
		}

		TextSpan diagnosticSpan = context.Diagnostics[0].Location.SourceSpan;
		SyntaxNode node = root.FindNode(diagnosticSpan);

		if (node is not AttributeSyntax attribute)
		{
			return context.Document;
		}

		SemanticModel? semanticModel = await context.Document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
		if (semanticModel == null)
		{
			return context.Document;
		}

		// Get symbol for the new attribute
		string fullName = "Hertzole.ScriptableValues." + attributeName;
		INamedTypeSymbol? newAttributeSymbol = semanticModel.Compilation.GetTypeByMetadataName(fullName);
		if (newAttributeSymbol == null)
		{
			Log.Info("Could not find symbol for new attribute: " + fullName);
			return context.Document;
		}

		DocumentEditor? editor = await DocumentEditor.CreateAsync(context.Document, cancellationToken).ConfigureAwait(false);
		SyntaxNode newAttribute = editor.Generator.Attribute(editor.Generator.TypeExpression(newAttributeSymbol));

		editor.ReplaceNode(attribute, newAttribute);

		return editor.GetChangedDocument();
	}
}