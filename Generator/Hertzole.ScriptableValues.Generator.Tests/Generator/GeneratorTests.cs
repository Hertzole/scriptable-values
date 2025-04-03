using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Generator.Tests;

public partial class GeneratorTests
{
	private const LanguageVersion DEFAULT_LANGUAGE_VERSION = LanguageVersion.CSharp9; // This is the closest to the version used by Unity.

	private static readonly FrozenDictionary<string, string> constants = new Dictionary<string, string>()
	{
		{ "<SUBSCRIBED_MASK_SUMMARY>", "/// <summary>A bitmask of all the possible subscribed callbacks.</summary>"},
		{ "<SUBSCRIBED_FIELD_SUMMARY>", "/// <summary>The current mask of all subscribed callbacks.</summary>" },
		{ "<SUBSCRIBE_TO_ALL_SUMMARY>", "/// <summary>Subscribes to all scriptable callbacks.</summary>" },
		{ "<UNSUBSCRIBE_TO_ALL_SUMMARY>", "/// <summary>Unsubscribes from all scriptable callbacks.</summary>" },
	}.ToFrozenDictionary();
	
	private static void AssertGeneratedOutput<T>(string source, string filename, string result) where T : IIncrementalGenerator, new()
	{
		AssertGeneratedOutput<T>(source, filename, result, DEFAULT_LANGUAGE_VERSION);
	}

	private static void AssertGeneratedOutput<T>(string source, string filename, string result, LanguageVersion languageVersion)
		where T : IIncrementalGenerator, new()
	{
		string assemblyVersion = typeof(T).Assembly.GetName().Version?.ToString() ?? "UNKNOWN VERSION";
		string generatorName = typeof(T).FullName ?? "UNKNOWN GENERATOR NAME";

		result = constants.Aggregate(result, (current, valuePair) => current.Replace(valuePair.Key, valuePair.Value));

		// Normalize the text and expected strings.
		result = result.Replace("    ", "\t")
		               .Replace("<ASSEMBLY_VERSION>", assemblyVersion)
		               .Replace("<GENERATOR_NAME>", generatorName)
		               .ReplaceLineEndings().Trim();

		List<PortableExecutableReference> assemblyReferences =
		[
			// Include the standard library
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
		];
		
		var parseOptions = CSharpParseOptions.Default
		                                     .WithLanguageVersion(languageVersion)
		                                     .WithPreprocessorSymbols("UNITY_2022_3_OR_NEWER"); // Required to compile attributes

		SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(SourceText.From(source, Encoding.UTF8), parseOptions);

		CSharpCompilation compilation = CSharpCompilation.Create(
			                                                 "Hertzole.ScriptableValues.Tests",
			                                                 [syntaxTree],
			                                                 assemblyReferences,
			                                                 new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, true))
		                                                 .AddScriptableValuesRuntime(parseOptions); // Add the runtime files to the compilation.

		GeneratorDriver driver = CSharpGeneratorDriver.Create(new T()).WithUpdatedParseOptions(parseOptions);

		GeneratorDriver runDriver = driver.RunGeneratorsAndUpdateCompilation(compilation, out Compilation outputCompilation, out ImmutableArray<Diagnostic> diagnostics);

		// Ensure no errors occurred during generation.
		Assert.That(diagnostics, Is.Empty, "Generator produced errors.");

		// Assert.That(outputCompilation.GetDiagnostics().Where(d => d.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning), Is.Empty,
		// 	"Compilation produced errors or warnings.");
		
		GeneratorDriverRunResult runResult = runDriver.GetRunResult();

		// Ensure no errors occurred during generation.
		Assert.That(runResult.Diagnostics, Is.Empty, "Generator produced errors.");

		// Check if the generator threw an exception.
		Assert.That(runResult.Results.Any(r => r.Exception != null), Is.False, "Generator threw an exception");
		// Check if any files were generated.
		Assert.That(runResult.GeneratedTrees.Any(), Is.True, "No files were generated.");

		// Find the generated file.
		SyntaxTree generatedTree = outputCompilation.SyntaxTrees.Single(tree => Path.GetFileName(tree.FilePath) == filename);

		Assert.That(result, Is.EqualTo(generatedTree.ToString()));
	}
}