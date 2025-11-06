namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Interface for objects that can be notified when a pool changes.
    /// </summary>
    /// <typeparam name="T">The type in the pool.</typeparam>
    public interface IPoolCallbacks<out T>
    {
        event PoolChangedArgs<T> OnPoolChanged;
    }
}