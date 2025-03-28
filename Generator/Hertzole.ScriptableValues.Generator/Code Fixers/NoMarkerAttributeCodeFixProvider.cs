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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NoMarkerAttributeCodeFixProvider))]
[Shared]
public sealed class NoMarkerAttributeCodeFixProvider : CodeFixProvider
{
	private static readonly ImmutableArray<string> fixableDiagnosticIds = ImmutableArray.Create(DiagnosticIdentifiers.NoMarkerAttribute);

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

		context.RegisterCodeFix(
			CodeAction.Create(Resources.HSV0002CodeFixTitle, token => CreateChangedDocument(context, token), nameof(Resources.HSV0002CodeFixTitle)),
			diagnostic);

		return Task.CompletedTask;
	}

	private static async Task<Document> CreateChangedDocument(CodeFixContext context, CancellationToken cancellationToken)
	{
		SemanticModel? semanticModel = await context.Document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
		if (semanticModel == null)
		{
			return context.Document;
		}

		DocumentEditor editor = await DocumentEditor.CreateAsync(context.Document, cancellationToken).ConfigureAwait(false);
		SyntaxGenerator generator = editor.Generator;

		INamedTypeSymbol? attributeSymbol = semanticModel.Compilation.GetTypeByMetadataName(Types.MARKER_ATTRIBUTE);
		if (attributeSymbol == null)
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

		// Find the type declaration
		TypeDeclarationSyntax? typeDeclaration = attribute.FirstAncestorOrSelf<TypeDeclarationSyntax>();
		if (typeDeclaration == null)
		{
			return context.Document;
		}

		SyntaxNode attributeDeclaration = generator.Attribute(generator.TypeExpression(attributeSymbol, true));
		editor.AddAttribute(typeDeclaration, attributeDeclaration);

		return editor.GetChangedDocument();
	}
}