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
}