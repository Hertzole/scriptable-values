using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;

namespace Hertzole.ScriptableValues.Generator;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NoCallbackImplementationCodeFixProvider))]
[Shared]
public sealed class NoCallbackImplementationCodeFixProvider : CodeFixProvider
{
	private static readonly ImmutableArray<string> fixableDiagnosticIds = ImmutableArray.Create(DiagnosticIdentifiers.NoCallbackImplementation);

	public override ImmutableArray<string> FixableDiagnosticIds
	{
		get { return fixableDiagnosticIds; }
	}

	public override FixAllProvider GetFixAllProvider()
	{
		return WellKnownFixAllProviders.BatchFixer;
	}

	public override async Task RegisterCodeFixesAsync(CodeFixContext context)
	{
		Diagnostic diagnostic = context.Diagnostics[0];
		if (!diagnostic.Properties.TryGetValue("callbackName", out string? callbackName))
		{
			callbackName = string.Empty;
		}

		context.RegisterCodeFix(CodeAction.Create(
			string.Format(Resources.HSV0001CodeFixTitle, callbackName),
			token => CreateChangedDocument(context, token),
			nameof(Resources.HSV0001CodeFixTitle)), diagnostic);
	}

	private static async Task<Document> CreateChangedDocument(CodeFixContext context, CancellationToken cancellationToken)
	{
		SyntaxNode? root = await context.Document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
		if (root == null)
		{
			return context.Document;
		}

		// Find the declaration
		TextSpan diagnosticSpan = context.Diagnostics[0].Location.SourceSpan;
		SyntaxNode node = root.FindNode(diagnosticSpan);

		if (node is not AttributeSyntax attribute)
		{
			return context.Document;
		}

		// Get the member declaration
		MemberDeclarationSyntax? memberDeclaration = attribute.FirstAncestorOrSelf<MemberDeclarationSyntax>();
		if (memberDeclaration == null)
		{
			return context.Document;
		}

		SemanticModel? semanticModel = await context.Document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
		if (semanticModel == null)
		{
			return context.Document;
		}

		// Get the field/property wrapper
		if (!memberDeclaration.TryGetFieldOrPropertyWrapper(in semanticModel, out ImmutableArray<FieldOrPropertyWrapper> wrappers) || wrappers.IsDefaultOrEmpty)
		{
			return context.Document;
		}

		Diagnostic diagnostic = context.Diagnostics[0];
		if (!diagnostic.Properties.TryGetValue("fieldName", out string? fieldName))
		{
			return context.Document;
		}

		FieldOrPropertyWrapper? targetWrapper = null;
		for (int i = 0; i < wrappers.Length; i++)
		{
			FieldOrPropertyWrapper wrapper = wrappers.ItemRef(i);
			if (wrapper.Name == fieldName)
			{
				targetWrapper = wrapper;
				break;
			}
		}

		if (targetWrapper == null)
		{
			return context.Document;
		}

		if (!ScriptableValueHelper.TryGetScriptableType(in semanticModel, (INamedTypeSymbol?) targetWrapper.Value.Type, out ScriptableType scriptableType,
			    out ITypeSymbol? genericType) ||
		    !diagnostic.Properties.TryGetValue("callbackName", out string? callbackName))
		{
			return context.Document;
		}

		DocumentEditor editor = await DocumentEditor.CreateAsync(context.Document, cancellationToken).ConfigureAwait(false);
		SyntaxGenerator generator = editor.Generator;

		SyntaxNode newMethod = null;

		using ArrayBuilder<SyntaxNode> parametersBuilder = new ArrayBuilder<SyntaxNode>();

		if (scriptableType == ScriptableType.Value)
		{
			SyntaxNode typeExpression = generator.TypeExpression(genericType!, true);

			parametersBuilder.Add(generator.ParameterDeclaration("oldValue", typeExpression));
			parametersBuilder.Add(generator.ParameterDeclaration("newValue", typeExpression));

			newMethod = generator.MethodDeclaration(
				callbackName!,
				parametersBuilder.ToImmutable(),
				accessibility: Accessibility.Private,
				modifiers: DeclarationModifiers.Partial);
		}

		//TODO: Remove when all types are supported
		if (newMethod == null)
		{
			return context.Document;
		}

		ThrowStatementSyntax throwStatement = SyntaxFactory.ThrowStatement(
			SyntaxFactory.ObjectCreationExpression(
				SyntaxFactory.ParseTypeName("System.NotImplementedException"),
				SyntaxFactory.ArgumentList(),
				null));

		newMethod = generator.WithStatements(newMethod, [throwStatement]);
		editor.InsertAfter(memberDeclaration, newMethod);

		return editor.GetChangedDocument();
	}
}