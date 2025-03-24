namespace Hertzole.ScriptableValues
{
	public delegate void PoolEventArgs<in T>(PoolAction action, T item);

	public delegate void PoolEventArgsWithContext<in T, in TContext>(PoolAction action, T item, TContext context);
}