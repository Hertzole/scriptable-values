using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Generator.Tests;

public class SubscribedBitMaskTests
{
    private CodeWriter codeWriter;
    private Compilation compilation;

    [SetUp]
    public void SetUp()
    {
        codeWriter = new CodeWriter();
        compilation = CompilationHelper.CreateStandardCompilation([], CSharpParseOptions.Default);
    }

    [TearDown]
    public void TearDown()
    {
        codeWriter.Clear();
    }

    [Test]
    [TestCase(7, "byte")]
    [TestCase(8, "byte")]
    [TestCase(9, "ushort")]
    [TestCase(15, "ushort")]
    [TestCase(16, "ushort")]
    [TestCase(17, "uint")]
    [TestCase(31, "uint")]
    [TestCase(32, "uint")]
    [TestCase(33, "ulong")]
    [TestCase(63, "ulong")]
    [TestCase(64, "ulong")]
    public void EnumType_SupportsAmount(int amount, string type)
    {
        // Arrange
        EquatableArray<CallbackData> data;
        using (ArrayBuilder<CallbackData> builder = new ArrayBuilder<CallbackData>(amount))
        {
            for (int i = 0; i < amount; i++)
            {
                builder.Add(new CallbackData($"Test{i}", CallbackType.Event, CallbackFlags.None, null!, ScriptableType.Event, null!));
            }

            data = new EquatableArray<CallbackData>(builder.ToImmutable());
        }

        CodeWriter expectedWriter = new CodeWriter();
        expectedWriter.Append("private enum SubscribedCallbacksMask : ");
        expectedWriter.AppendLine(type);
        expectedWriter.AppendLine("{");
        expectedWriter.Indent++;
        expectedWriter.AppendLine("None = 0,");
        for (int i = 0; i < amount; i++)
        {
            if (i > 0)
            {
                expectedWriter.AppendLine(",");
            }

            expectedWriter.Append($"Test{i} = 1 << {i}");
        }

        expectedWriter.AppendLine();
        expectedWriter.Indent--;
        expectedWriter.AppendLine("}");

        // Act
        ScriptableCallbackGenerator.WriteSubscribedEnumMask(in codeWriter, in data, default, CancellationToken.None);

        // Assert
        Assert.That(codeWriter.ToString(), Is.EqualTo(expectedWriter.ToString()));
    }
}