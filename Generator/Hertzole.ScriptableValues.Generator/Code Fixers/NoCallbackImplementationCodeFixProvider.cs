using System;
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

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
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

        return Task.CompletedTask;
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

        SyntaxNode? newMethod = GetImplementationMethod(in editor, in semanticModel, in callbackName!, in scriptableType, in genericType!);

        editor.InsertAfter(memberDeclaration, newMethod);

        return editor.GetChangedDocument();
    }

    private static SyntaxNode GetImplementationMethod(in DocumentEditor editor,
        in SemanticModel semanticModel,
        in string callbackName,
        in ScriptableType scriptableType,
        in ITypeSymbol? genericType)
    {
        SyntaxGenerator generator = editor.Generator;
        using ArrayBuilder<SyntaxNode> parametersBuilder = new ArrayBuilder<SyntaxNode>();

        switch (scriptableType)
        {
            case ScriptableType.Value:
                SyntaxNode genericTypeExpression = generator.TypeExpression(genericType!, true);

                parametersBuilder.Add(generator.ParameterDeclaration("oldValue", genericTypeExpression));
                parametersBuilder.Add(generator.ParameterDeclaration("newValue", genericTypeExpression));
                break;
            case ScriptableType.GenericEvent:
            case ScriptableType.Event:
                SyntaxNode senderExpression = generator.TypeExpression(semanticModel.Compilation.GetSpecialType(SpecialType.System_Object));
                SyntaxNode argsExpression = generator.TypeExpression(genericType ?? semanticModel.Compilation.GetTypeByMetadataName("System.EventArgs")!, true);

                parametersBuilder.Add(generator.ParameterDeclaration("sender", senderExpression));
                parametersBuilder.Add(generator.ParameterDeclaration("args", argsExpression));
                break;
            case ScriptableType.Pool:
                SyntaxNode poolActionExpression =
                    generator.TypeExpression(semanticModel.Compilation.GetTypeByMetadataName("Hertzole.ScriptableValues.PoolAction")!, true);

                SyntaxNode poolItemExpression = generator.TypeExpression(genericType!, true);

                parametersBuilder.Add(generator.ParameterDeclaration("action", poolActionExpression));
                parametersBuilder.Add(generator.ParameterDeclaration("item", poolItemExpression));
                break;
            case ScriptableType.List:
            case ScriptableType.Dictionary:
                SyntaxNode collectionArgsExpression = generator.TypeExpression(semanticModel.Compilation.GetCollectionChangedArgs(in genericType!), true);

                parametersBuilder.Add(generator.ParameterDeclaration("args", collectionArgsExpression));
                break;
            default:
                throw new NotSupportedException("Unsupported scriptable type: " + scriptableType);
        }

        SyntaxNode newMethod = generator.MethodDeclaration(
            callbackName,
            parametersBuilder.ToImmutable(),
            accessibility: Accessibility.Private,
            modifiers: DeclarationModifiers.Partial);

        ThrowStatementSyntax throwStatement = SyntaxFactory.ThrowStatement(
            SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.ParseTypeName("System.NotImplementedException"),
                SyntaxFactory.ArgumentList(),
                null));

        newMethod = generator.WithStatements(newMethod, [throwStatement]);

        return newMethod;
    }
}