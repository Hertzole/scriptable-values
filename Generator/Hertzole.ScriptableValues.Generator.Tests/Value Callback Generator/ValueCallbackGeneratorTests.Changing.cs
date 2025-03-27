using Xunit;

namespace Hertzole.ScriptableValues.Generator.Tests;

partial class ValueCallbackGeneratorTests
{
	private const string CHANGING = /*lang=cs*/@"using System;
using Hertzole.ScriptableValues;

[GenerateScriptableCallbacks]
public partial class ChangingClass
{
	[GenerateValueCallback(ValueCallbackType.Changing)]
	public ScriptableBool valueField;
	[GenerateValueCallback(ValueCallbackType.Changing)]
	public ScriptableString ValueProperty { get; set; }

	private partial void OnValueFieldChanging(bool oldValue, bool newValue)
	{
		throw new System.NotImplementedException();
	}

	private partial void OnValuePropertyChanging(string oldValue, string newValue)
	{
		throw new System.NotImplementedException();
	}
}";

	private const string CHANGING_EXPECTED = /*lang=cs*/@"partial class ChangingClass
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
	private static global::System.Action<bool, bool, global::ChangingClass> __valueFieldScriptableValueCallbackChanging = (oldValue, newValue, context) => { context.OnValueFieldChanging(oldValue, newValue); };
#if UNITY_EDITOR
	[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
#endif // UNITY_EDITOR
	private static global::System.Action<string, string, global::ChangingClass> __ValuePropertyScriptableValueCallbackChanging = (oldValue, newValue, context) => { context.OnValuePropertyChanging(oldValue, newValue); };

	private void SubscribeToCallbacks()
	{
		if ((subscribedCallbacks & SubscribedCallbacksMask.valueField) == 0)
		{
			valueField.RegisterValueChangingListener(__valueFieldScriptableValueCallbackChanging, this);
			subscribedCallbacks |= SubscribedCallbacksMask.valueField;
		}
		if ((subscribedCallbacks & SubscribedCallbacksMask.ValueProperty) == 0)
		{
			ValueProperty.RegisterValueChangingListener(__ValuePropertyScriptableValueCallbackChanging, this);
			subscribedCallbacks |= SubscribedCallbacksMask.ValueProperty;
		}
	}

	private void UnsubscribeFromCallbacks()
	{
		if ((subscribedCallbacks & SubscribedCallbacksMask.valueField) != 0)
		{
			valueField.UnregisterValueChangingListener(__valueFieldScriptableValueCallbackChanging);
			subscribedCallbacks &= ~SubscribedCallbacksMask.valueField;
		}
		if ((subscribedCallbacks & SubscribedCallbacksMask.ValueProperty) != 0)
		{
			ValueProperty.UnregisterValueChangingListener(__ValuePropertyScriptableValueCallbackChanging);
			subscribedCallbacks &= ~SubscribedCallbacksMask.ValueProperty;
		}
	}

	private partial void OnValueFieldChanging(bool oldValue, bool newValue);

	private partial void OnValuePropertyChanging(string oldValue, string newValue);
}";

	[Fact]
	public void Changing()
	{
		GeneratorTest.RunTest<ScriptableCallbackGenerator>("ChangingClass.g.cs", CHANGING, CHANGING_EXPECTED);
	}
}