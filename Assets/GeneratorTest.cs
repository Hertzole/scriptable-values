using Hertzole.ScriptableValues;

public partial class ChangingClass
{
	[GenerateCallback(CallbackType.PreInvoke)]
	public ScriptableBool valueField;
	[GenerateCallback(CallbackType.PreInvoke)]
	public ScriptableString ValueProperty { get; set; }

	private partial void OnValueFieldChanging(bool oldValue, bool newValue)
	{
		throw new System.NotImplementedException();
	}

	private partial void OnValuePropertyChanging(string oldValue, string newValue)
	{
		throw new System.NotImplementedException();
	}
}