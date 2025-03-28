using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

internal static class CompilationExtensions
{
	public static INamedTypeSymbol GetCollectionChangedArgs(this Compilation compilation, in ITypeSymbol genericType)
	{
		INamedTypeSymbol collectionArgs = compilation.GetTypeByMetadataName("Hertzole.ScriptableValues.CollectionChangedArgs`1")!;
		collectionArgs = collectionArgs.ConstructedFrom.Construct(genericType!);

		return collectionArgs;
	}
}