namespace Hertzole.ScriptableValues
{
	public interface IPoolCallbacks<out T>
	{
		event PoolChangedArgs<T> OnPoolChanged;
	}
}