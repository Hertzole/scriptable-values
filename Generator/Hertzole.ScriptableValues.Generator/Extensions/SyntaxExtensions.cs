using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.ScriptableValues.Generator;

internal static class SyntaxExtensions
{
    public static bool TryGetFieldDeclaration(this SyntaxNode node, out FieldDeclarationSyntax? fieldDeclaration)
    {
        if (node.IsKind(SyntaxKind.FieldDeclaration))
        {
            fieldDeclaration = (FieldDeclarationSyntax) node;
            return true;
        }

        SyntaxNode? parent = node.Parent;
        while (parent != null)
        {
            if (parent.IsKind(SyntaxKind.FieldDeclaration))
            {
                fieldDeclaration = (FieldDeclarationSyntax) parent;
                return true;
            }

            parent = parent.Parent;
        }

        fieldDeclaration = null;
        return false;
    }

    public static bool TryGetFieldOrPropertyWrapper(this SyntaxNode node, in SemanticModel semanticModel, out ImmutableArray<FieldOrPropertyWrapper> wrapper)
    {
        using ArrayBuilder<FieldOrPropertyWrapper> builder = new ArrayBuilder<FieldOrPropertyWrapper>();

        if (node is FieldDeclarationSyntax fieldSyntax)
        {
            foreach (VariableDeclaratorSyntax variable in fieldSyntax.Declaration.Variables)
            {
                ISymbol? declareSymbol = semanticModel.GetDeclaredSymbol(variable);
                if (declareSymbol is not IFieldSymbol fieldSymbol)
                {
                    continue;
                }

                builder.Add(new FieldOrPropertyWrapper(fieldSymbol));
            }
        }
        else if (node is PropertyDeclarationSyntax propertySyntax)
        {
            IPropertySymbol? symbol = semanticModel.GetDeclaredSymbol(propertySyntax);
            if (symbol is not IPropertySymbol propertySymbol)
            {
                return false;
            }

            builder.Add(new FieldOrPropertyWrapper(propertySymbol));
        }

        wrapper = builder.ToImmutable();
        return wrapper.Length > 0;
    }

    public static INamedTypeSymbol? GetAttributeSymbol(this AttributeSyntax syntax, SemanticModel semanticModel, CancellationToken cancellation)
    {
        if (semanticModel.GetSymbolInfo(syntax).Symbol is not IMethodSymbol methodSymbol)
        {
            return null;
        }

        cancellation.ThrowIfCancellationRequested();

        if (methodSymbol.ContainingType is INamedTypeSymbol attributeSymbol)
        {
            return attributeSymbol;
        }

        return null;
    }
}