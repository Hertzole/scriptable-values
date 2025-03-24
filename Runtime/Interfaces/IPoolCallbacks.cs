namespace Hertzole.ScriptableValues
{
	public interface IPoolCallbacks<out T>
	{
		event PoolEventArgs<T> OnPoolChanged;

		void RegisterChangedCallback(PoolEventArgs<T> callback);

		void RegisterChangedCallback<TContext>(PoolEventArgsWithContext<T, TContext> callback, TContext context);

		void UnregisterChangedCallback(PoolEventArgs<T> callback);

		void UnregisterChangedCallback<TContext>(PoolEventArgsWithContext<T, TContext> callback);
	}
}