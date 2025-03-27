using System;
using SymbolDisplayFormat = Microsoft.CodeAnalysis.SymbolDisplayFormat;

namespace Hertzole.ScriptableValues.Generator;

partial class ScriptableCallbackGenerator
{
	private static void WriteValueField(in CodeWriter writer, in HierarchyInfo hierarchy, in CallbackData data)
	{
		string genericTypeName = data.GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		using ArrayBuilder<string> genericParameters = new ArrayBuilder<string>(3);
		genericParameters.Add(genericTypeName);
		genericParameters.Add(genericTypeName);
		genericParameters.Add(hierarchy.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

		using ArrayBuilder<string> callbackParameters = new ArrayBuilder<string>(3);
		callbackParameters.Add("oldValue");
		callbackParameters.Add("newValue");
		callbackParameters.Add("context");

		if ((data.Flags & CallbackFlags.PreAndPostInvoke) == CallbackFlags.PreAndPostInvoke)
		{
			ReadOnlySpan<char> changingCallbackName = ScriptableValueChanging;
			ReadOnlySpan<char> changedCallbackName = ScriptableValueChanged;

			ArrayBuilder<char> changingName = new ArrayBuilder<char>(data.Name.Length + changingCallbackName.Length);
			ArrayBuilder<char> changedName = new ArrayBuilder<char>(data.Name.Length + changedCallbackName.Length);

			ArrayBuilder<char> changingCallback = new ArrayBuilder<char>("context.On".AsSpan());
			ArrayBuilder<char> changedCallback = new ArrayBuilder<char>("context.On".AsSpan());
			try
			{
				changingName.AddRange(data.Name);
				changingName.AddRange(changingCallbackName);

				changedName.AddRange(data.Name);
				changedName.AddRange(changedCallbackName);

				ReadOnlySpan<char> prettyName = Naming.FormatVariableName(data.Name.AsSpan());
				changingCallback.AddRange(prettyName);
				changingCallback.AddRange("Changing(oldValue, newValue);");

				changedCallback.AddRange(prettyName);
				changedCallback.AddRange("Changed(oldValue, newValue);");

				WriteCachedField(in writer, genericParameters.AsSpan(), changingName.AsSpan(), callbackParameters.AsSpan(), changingCallback.AsSpan());
				WriteCachedField(in writer, genericParameters.AsSpan(), changedName.AsSpan(), callbackParameters.AsSpan(), changedCallback.AsSpan());
			}
			finally
			{
				// Dispose manually instead of using 'using' to avoid creating a bunch of try blocks.
				changingName.Dispose();
				changedName.Dispose();
				changingCallback.Dispose();
				changedCallback.Dispose();
			}
		}
		else
		{
			// If we are only generating one callback, we need to use the correct name.
			// If it has the flag PostInvoke, we use the 'Changed' name, otherwise we use 'Changing'.
			ReadOnlySpan<char> callbackName = (data.Flags & CallbackFlags.PostInvoke) != 0 ? ScriptableValueChanged : ScriptableValueChanging;

			ArrayBuilder<char> name = new ArrayBuilder<char>(data.Name.Length + callbackName.Length);
			ArrayBuilder<char> callback = new ArrayBuilder<char>("context.On".AsSpan());
			try
			{
				name.AddRange(data.Name);
				name.AddRange(callbackName);

				ReadOnlySpan<char> prettyName = Naming.FormatVariableName(data.Name.AsSpan());
				callback.AddRange(prettyName);

				callback.AddRange((data.Flags & CallbackFlags.PostInvoke) != 0 ? "Changed" : "Changing");

				callback.AddRange("(oldValue, newValue);");

				WriteCachedField(in writer, genericParameters.AsSpan(), name.AsSpan(), callbackParameters.AsSpan(), callback.AsSpan());
			}
			finally
			{
				// Dispose manually instead of using 'using' to avoid creating a bunch of try blocks.
				name.Dispose();
				callback.Dispose();
			}
		}
	}

	private static void WriteCachedField(in CodeWriter writer,
		in ReadOnlySpan<string> genericParameters,
		in ReadOnlySpan<char> name,
		in ReadOnlySpan<string> callbackParameters,
		in ReadOnlySpan<char> callback)
	{
		using (writer.WithIndent(0))
		{
			writer.AppendLine("#if UNITY_EDITOR");
		}

		writer.AppendLine("[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
		WriteGeneratedCodeAttribute(writer);
		using (writer.WithIndent(0))
		{
			writer.AppendLine("#endif // UNITY_EDITOR");
		}

		writer.Append("private static global::System.Action<");
		for (int i = 0; i < genericParameters.Length; i++)
		{
			if (i > 0)
			{
				writer.Append(", ");
			}

			writer.Append(genericParameters[i]);
		}

		writer.Append("> ");
		writer.Append(name);
		writer.Append(" = (");

		for (int i = 0; i < callbackParameters.Length; i++)
		{
			if (i > 0)
			{
				writer.Append(", ");
			}

			writer.Append(callbackParameters[i]);
		}

		writer.Append(") => { ");
		writer.Append(callback);
		writer.AppendLine(" };");
	}

	private static void WriteGeneratedCodeAttribute(in CodeWriter writer, bool withIfDefines = false)
	{
		if (withIfDefines)
		{
			using (writer.WithIndent(0))
			{
				writer.AppendLine("#if UNITY_EDITOR");
			}
		}

		writer.AppendGeneratedCodeAttribute(generatorName, generatorVersion);

		if (withIfDefines)
		{
			using (writer.WithIndent(0))
			{
				writer.AppendLine("#endif // UNITY_EDITOR");
			}
		}
	}
}