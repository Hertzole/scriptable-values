namespace Hertzole.ScriptableValues
{
	public delegate void ValueEventHandler<in T>(T oldValue, T newValue);
}