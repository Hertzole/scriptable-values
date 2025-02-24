using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Hertzole.ScriptableValues.Generator.Tests;

internal static class CompilationExtensions
{
	private static readonly string packagePath = Path.GetFullPath("../../../../../Packages/se.hertzole.scriptable-values");
	private static readonly string runtimePath = Path.Combine(packagePath, "Runtime");
	private static readonly SyntaxTree[] runtimeTrees = GetScriptFiles(runtimePath);

	public static CSharpCompilation AddScriptableValuesRuntime(this CSharpCompilation compilation)
	{
		return compilation.AddSyntaxTrees(runtimeTrees);
	}

	private static SyntaxTree[] GetScriptFiles(string path)
	{
		string[] files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

		// We expect half of the files to be meta files.
		int expectedCapacity = files.Length / 2;
		List<SyntaxTree> trees = new List<SyntaxTree>(expectedCapacity);

		for (int i = 0; i < files.Length; i++)
		{
			if (files[i].EndsWith(".meta"))
			{
				continue;
			}

			trees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(files[i])));
		}

		return trees.ToArray();
	}
}