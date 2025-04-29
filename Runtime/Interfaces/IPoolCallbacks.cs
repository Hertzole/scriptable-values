namespace Hertzole.ScriptableValues
{
	public interface IPoolCallbacks<out T>
	{
		event PoolEventArgs<T> OnPoolChanged;
	}
}