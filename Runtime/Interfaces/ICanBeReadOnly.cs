namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Interface for objects that can be read-only.
	/// </summary>
	public interface ICanBeReadOnly
	{
		/// <summary>
		///     Gets or sets if the object is read-only.
		/// </summary>
		bool IsReadOnly { get; set; }
	}
}