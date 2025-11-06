namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     What action was performed on the pool.
    /// </summary>
    public enum PoolAction
    {
        /// <summary>
        ///     An object was created.
        /// </summary>
        CreatedObject = 0,
        /// <summary>
        ///     An object was destroyed.
        /// </summary>
        DestroyedObject = 1,
        /// <summary>
        ///     An object was rented.
        /// </summary>
        RentedObject = 2,
        /// <summary>
        ///     An object was returned back to the pool.
        /// </summary>
        ReleasedObject = 3
    }
}