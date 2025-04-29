namespace Hertzole.ScriptableValues
{
	public delegate void PoolEventArgs<in T>(PoolAction action, T item);
}