using Xunit;

namespace Hertzole.ScriptableValues.Generator.Tests;

partial class ValueCallbackGeneratorTests
{
	private const string CHANGED = /*lang=cs*/@"using System;
using Hertzole.ScriptableValues;

[GenerateScriptableCallbacks]
public partial class ChangedClass
{
	[GenerateValueCallback(ValueCallbackType.Changed)]
	public ScriptableBool valueField;
	[GenerateValueCallback(ValueCallbackType.Changed)]
	public ScriptableString ValueProperty { get; set; }

	private partial void OnValueFieldChanged(bool oldValue, bool newValue)
	{
		throw new System.NotImplementedException();
	}

	private partial void OnValuePropertyChanged(string oldValue, string newValue)
	{
		throw new System.NotImplementedException();
	}
}";

	private const string CHANGED_EXPECTED = /*lang=cs*/@"partial class ChangedClass
{
	private enum SubscribedCallbacksMask : byte
	{
		None = 0,
		valueField = 1 << 0,
		ValueProperty = 1 << 1
	}

#if UNITY_EDITOR
	[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
#endif // UNITY_EDITOR
	private SubscribedCallbacksMask subscribedCallbacks = SubscribedCallbacksMask.None;

#if UNITY_EDITOR
	[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
#endif // UNITY_EDITOR
	private static global::System.Action<bool, bool, global::ChangedClass> __valueFieldScriptableValueCallbackChanged = (oldValue, newValue, context) => { context.OnValueFieldChanged(oldValue, newValue); };
#if UNITY_EDITOR
	[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
#endif // UNITY_EDITOR
	private static global::System.Action<string, string, global::ChangedClass> __ValuePropertyScriptableValueCallbackChanged = (oldValue, newValue, context) => { context.OnValuePropertyChanged(oldValue, newValue); };

	private void SubscribeToCallbacks()
	{
		if ((subscribedCallbacks & SubscribedCallbacksMask.valueField) == 0)
		{
			valueField.RegisterValueChangedListener(__valueFieldScriptableValueCallbackChanged, this);
			subscribedCallbacks |= SubscribedCallbacksMask.valueField;
		}
		if ((subscribedCallbacks & SubscribedCallbacksMask.ValueProperty) == 0)
		{
			ValueProperty.RegisterValueChangedListener(__ValuePropertyScriptableValueCallbackChanged, this);
			subscribedCallbacks |= SubscribedCallbacksMask.ValueProperty;
		}
	}

	private void UnsubscribeFromCallbacks()
	{
		if ((subscribedCallbacks & SubscribedCallbacksMask.valueField) != 0)
		{
			valueField.UnregisterValueChangedListener(__valueFieldScriptableValueCallbackChanged);
			subscribedCallbacks &= ~SubscribedCallbacksMask.valueField;
		}
		if ((subscribedCallbacks & SubscribedCallbacksMask.ValueProperty) != 0)
		{
			ValueProperty.UnregisterValueChangedListener(__ValuePropertyScriptableValueCallbackChanged);
			subscribedCallbacks &= ~SubscribedCallbacksMask.ValueProperty;
		}
	}

	private partial void OnValueFieldChanged(bool oldValue, bool newValue);

	private partial void OnValuePropertyChanged(string oldValue, string newValue);
}";

	[Fact]
	public void Changed()
	{
		GeneratorTest.RunTest<ScriptableCallbackGenerator>("ChangedClass.g.cs", CHANGED, CHANGED_EXPECTED);
	}
}