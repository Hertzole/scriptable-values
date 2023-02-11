namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     Type of change events that can be raised by a <see cref="ScriptableDictionary{TKey, TValue}" />.
	/// </summary>
	public enum DictionaryChangeType
	{
		/// <summary>
		///     Called when an item is added to the dictionary.
		/// </summary>
		Added = 0,
		/// <summary>
		///     Called when an item is removed from the dictionary.
		/// </summary>
		Removed = 1,
		/// <summary>
		///     Called when the dictionary is cleared.
		/// </summary>
		Cleared = 2,
		/// <summary>
		///     Called when an item is set in the dictionary using the indexer.
		/// </summary>
		Set = 3,
		/// <summary>
		///     Called when the dictionary is trimmed.
		/// </summary>
		Trimmed = 4
	}
}