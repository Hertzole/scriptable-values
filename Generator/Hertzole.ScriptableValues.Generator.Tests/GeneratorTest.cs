using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace Hertzole.ScriptableValues.Generator.Tests;

public static class GeneratorTest
{
	public static void RunTest<T>(string fileName, string text, string expected) where T : class, IIncrementalGenerator, new()
	{
		// Create an instance of the generator.
		T generator = new T();

		// Normalize the text and expected strings.
		expected = expected.Replace("    ", "\t").Replace("\r\n", "\n").Trim();

		CSharpGeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

		List<PortableExecutableReference> refs =
		[
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
		];

		CSharpCompilation compilation =
			CSharpCompilation.Create("Hertzole.ScriptableValues.GeneratorTests", new[] { CSharpSyntaxTree.ParseText(text) },
				                 refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, true))
			                 .AddScriptableValuesRuntime(); // Add the runtime files to the compilation.
		
		
		GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();

		// Check if the generator threw an exception.
		Assert.True(runResult.Results.Any(r => r.Exception is null), "Generator threw an exception.");
		// Check if any files were generated.
		Assert.True(runResult.GeneratedTrees.Any(), "No files were generated.");

		// Find the generated file.
		SyntaxTree? generatedFileSyntax = runResult.GeneratedTrees.FirstOrDefault(t => t.FilePath.EndsWith(fileName));

		// Check if the file was found.
		Assert.NotNull(generatedFileSyntax);

		// Get the text of the generated file.
		// Replace \r\n with \n and trim the text.
		string generatedText = generatedFileSyntax.GetText().ToString().Replace("\r\n", "\n").Trim();

		Assert.Equal(expected, generatedText);
	}
}