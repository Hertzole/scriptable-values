namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     Types of change events that can be raised by a <see cref="ScriptableList{T}" />.
	/// </summary>
	public enum ListChangeType
	{
		/// <summary>
		///     Called when an item is added to the list.
		/// </summary>
		Added = 0,
		/// <summary>
		///     Called when an item is inserted into the list.
		/// </summary>
		Inserted = 1,
		/// <summary>
		///     Called when an item is removed from the list.
		/// </summary>
		Removed = 2,
		/// <summary>
		///     Called when the list is cleared.
		/// </summary>
		Cleared = 3,
		/// <summary>
		///     Called when an item is replaced in the list using the indexer.
		/// </summary>
		Replaced = 4,
		/// <summary>
		///     Called when the list is reversed.
		/// </summary>
		Reversed = 5,
		/// <summary>
		///     Called when the list is sorted.
		/// </summary>
		Sorted = 6,
		/// <summary>
		///     Called when the list is trimmed.
		/// </summary>
		Trimmed = 7
	}
}