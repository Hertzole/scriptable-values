namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Delegate for pool changed events.
    /// </summary>
    /// <typeparam name="T">The type of the item in the pool.</typeparam>
    public delegate void PoolChangedArgs<in T>(PoolAction action, T item);
}