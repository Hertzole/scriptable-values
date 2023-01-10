namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     Interface for receiving callbacks from <see cref="ScriptablePool{T}" />
	/// </summary>
	public interface IPoolable
	{
		/// <summary>
		///     Called when the object is removed from the pool.
		/// </summary>
		void OnUnpooled();

		/// <summary>
		///     Called when the object is added to the pool.
		/// </summary>
		void OnPooled();
	}
}