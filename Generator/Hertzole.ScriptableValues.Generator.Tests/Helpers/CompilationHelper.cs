using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Hertzole.ScriptableValues.Generator.Tests;

internal static class CompilationHelper
{
	public static CSharpCompilation CreateStandardCompilation(SyntaxTree[] trees, CSharpParseOptions parseOptions)
	{
		List<PortableExecutableReference> assemblyReferences =
		[
			// Include the standard library
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
		];

		List<SyntaxTree> totalTrees = new List<SyntaxTree>(trees.Length + 2)
		{
			CSharpSyntaxTree.ParseText("namespace TestNamespace { public class TestClass {} }", parseOptions),
			CSharpSyntaxTree.ParseText("namespace TestNamespace { public struct TestStruct {} }", parseOptions)
		};

		totalTrees.AddRange(trees);

		CSharpCompilation compilation = CSharpCompilation.Create(
			                                                 "Hertzole.ScriptableValues.Tests",
			                                                 totalTrees,
			                                                 assemblyReferences,
			                                                 new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, true))
		                                                 .AddScriptableValuesRuntime(parseOptions); // Add the runtime files to the compilation.

		return compilation;
	}

	public static CSharpParseOptions CreateStandardParseOptions(LanguageVersion languageVersion)
	{
		CSharpParseOptions parseOptions = CSharpParseOptions.Default
		                                                    .WithLanguageVersion(languageVersion)
		                                                    .WithPreprocessorSymbols("UNITY_2022_3_OR_NEWER"); // Required to compile attributes

		return parseOptions;
	}
}