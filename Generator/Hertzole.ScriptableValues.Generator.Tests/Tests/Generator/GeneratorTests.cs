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
    private static readonly FrozenDictionary<string, string> constants = new Dictionary<string, string>
    {
        { "<SUBSCRIBED_MASK_SUMMARY>", "/// <summary>A bitmask of all the possible subscribed callbacks.</summary>" },
        { "<SUBSCRIBED_FIELD_SUMMARY>", "/// <summary>The current mask of all subscribed callbacks.</summary>" },
        { "<SUBSCRIBE_TO_ALL_SUMMARY>", "/// <summary>Subscribes to all scriptable callbacks.</summary>" },
        { "<UNSUBSCRIBE_TO_ALL_SUMMARY>", "/// <summary>Unsubscribes from all scriptable callbacks.</summary>" }
    }.ToFrozenDictionary();
    private const LanguageVersion DEFAULT_LANGUAGE_VERSION = LanguageVersion.CSharp9; // This is the closest to the version used by Unity.

    private static void AssertGeneratedOutput<T>(string source, string filename, string result, LanguageVersion languageVersion = DEFAULT_LANGUAGE_VERSION)
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

        GeneratorDriverRunResult runResult = RunDriver<T>([source], languageVersion, out Compilation outputCompilation, out _);

        // Ensure no errors occurred during generation.
        Assert.That(runResult.Diagnostics, Is.Empty, "Generator produced errors.");

        // Check if the generator threw an exception.
        Assert.That(runResult.Results.Any(r => r.Exception != null), Is.False, "Generator threw an exception");
        // Check if any files were generated.
        Assert.That(runResult.GeneratedTrees.Any(), Is.True, "No files were generated.");

        // Find the generated file.
        SyntaxTree generatedTree = outputCompilation.SyntaxTrees.Single(tree => Path.GetFileName(tree.FilePath) == filename);

        Assert.That(generatedTree.ToString(), Is.EqualTo(result));
    }

    private static void AssertDoesNotGenerate<T>(string source, LanguageVersion languageVersion = DEFAULT_LANGUAGE_VERSION)
        where T : IIncrementalGenerator, new()
    {
        // Act
        GeneratorDriverRunResult runResult = RunDriver<T>([source], languageVersion, out Compilation _, out _);

        // Assert
        Assert.That(runResult.Diagnostics, Is.Empty, "Generator produced errors.");

        // Check if the generator threw an exception.
        Assert.That(runResult.Results.Any(r => r.Exception != null), Is.False, "Generator threw an exception");
        // Check if any files were generated.
        Assert.That(runResult.GeneratedTrees.Any(), Is.False, "Files were generated.");
    }

    private static GeneratorDriverRunResult RunDriver<T>(string[] sources,
        LanguageVersion languageVersion,
        out Compilation outputCompilation,
        out ImmutableArray<Diagnostic> diagnostics)
        where T : IIncrementalGenerator, new()
    {
        CSharpParseOptions parseOptions = CompilationHelper.CreateStandardParseOptions(languageVersion);

        SyntaxTree[] trees = new SyntaxTree[sources.Length];

        for (int i = 0; i < sources.Length; i++)
        {
            trees[i] = CSharpSyntaxTree.ParseText(SourceText.From(sources[i], Encoding.UTF8), parseOptions);
        }

        CSharpCompilation compilation = CompilationHelper.CreateStandardCompilation(trees, parseOptions);

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new T()).WithUpdatedParseOptions(parseOptions);

        GeneratorDriver runDriver = driver.RunGeneratorsAndUpdateCompilation(compilation, out outputCompilation, out diagnostics);

        // Ensure no errors occurred during generation.
        Assert.That(diagnostics, Is.Empty, "Generator produced errors.");

        return runDriver.GetRunResult();
    }
}