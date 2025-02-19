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

	public void AppendLine()
	{
		sb.AppendLine();
		shouldWriteIndent = true;
	}

	public void Clear()
	{
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