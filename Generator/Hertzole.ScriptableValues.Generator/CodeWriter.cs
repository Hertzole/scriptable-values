using System;
using System.Text;

namespace Hertzole.ScriptableValues.Generator;

public sealed class CodeWriter
{
	private readonly StringBuilder sb = new StringBuilder(1024);

	private bool shouldWriteIndent = false;

	public int Indent { get; set; }

	public void Append(string value)
	{
		WriteIndentIfNeeded();

		sb.Append(value);
	}

	public void Append(ReadOnlySpan<char> value)
	{
		WriteIndentIfNeeded();

		sb.Append(value.ToString());
	}

	public void Append(int value)
	{
		WriteIndentIfNeeded();

		sb.Append(value);
	}

	public void AppendLine(string value)
	{
		WriteIndentIfNeeded();

		sb.AppendLine(value);
		shouldWriteIndent = true;
	}
	
	public void AppendLine(ReadOnlySpan<char> value)
	{
		AppendLine(value.ToString());
	}

	public void AppendLine()
	{
		sb.AppendLine();
		shouldWriteIndent = true;
	}

	public void AppendGeneratedCodeAttribute(string generator, string version)
	{
		WriteIndentIfNeeded();

		sb.Append("[global::System.CodeDom.Compiler.GeneratedCode(\"");
		sb.Append(generator);
		sb.Append("\", \"");
		sb.Append(version);
		sb.Append("\")]\n");
		shouldWriteIndent = true;
	}

	public void AppendExcludeFromCodeCoverageAttribute(bool withIfDefines = true)
	{
		AppendWithinIfDefines(withIfDefines, static s => s.Append("[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]\n"));
	}

	private void AppendWithinIfDefines(bool withIfDefines, Action<StringBuilder> writeAction)
	{
		if (withIfDefines)
		{
			using (WithIndent(0))
			{
				sb.AppendLine("#if UNITY_INCLUDE_TESTS");
			}
		}

		WriteIndentIfNeeded();
		writeAction.Invoke(sb);

		if (withIfDefines)
		{
			using (WithIndent(0))
			{
				sb.AppendLine("#endif // UNITY_INCLUDE_TESTS");
			}
		}

		shouldWriteIndent = true;
	}

	public void Clear()
	{
		Indent = 0;
		shouldWriteIndent = false;
		sb.Clear();
	}

	private void WriteIndentIfNeeded()
	{
		if (!shouldWriteIndent)
		{
			return;
		}

		shouldWriteIndent = false;
		sb.Append('\t', Indent);
	}

	public IndentScope WithIndent(int indent)
	{
		return new IndentScope(this, indent);
	}

	public override string ToString()
	{
		return sb.ToString();
	}

	public readonly ref struct IndentScope
	{
		private readonly CodeWriter writer;
		private readonly int originalIndent;

		public IndentScope(CodeWriter writer, int newIndent)
		{
			this.writer = writer;
			originalIndent = writer.Indent;
			writer.Indent = newIndent;
		}

		public void Dispose()
		{
			writer.Indent = originalIndent;
		}
	}
}