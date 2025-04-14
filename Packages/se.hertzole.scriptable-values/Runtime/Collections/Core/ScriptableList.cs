using System.ComponentModel;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

namespace Hertzole.ScriptableValues
{
	public abstract class ScriptableList : RuntimeScriptableObject, ICanBeReadOnly
	{
		public static readonly PropertyChangingEventArgs clearOnStartChangingArgs = new PropertyChangingEventArgs(nameof(ClearOnStart));
		public static readonly PropertyChangedEventArgs clearOnStartChangedArgs = new PropertyChangedEventArgs(nameof(ClearOnStart));

		public static readonly PropertyChangingEventArgs isReadOnlyChangingArgs = new PropertyChangingEventArgs(nameof(IsReadOnly));
		public static readonly PropertyChangedEventArgs isReadOnlyChangedArgs = new PropertyChangedEventArgs(nameof(IsReadOnly));

		public static readonly PropertyChangingEventArgs setEqualityCheckChangingArgs = new PropertyChangingEventArgs(nameof(SetEqualityCheck));
		public static readonly PropertyChangedEventArgs setEqualityCheckChangedArgs = new PropertyChangedEventArgs(nameof(SetEqualityCheck));

		public static readonly PropertyChangingEventArgs countChangingArgs = new PropertyChangingEventArgs(nameof(Count));
		public static readonly PropertyChangedEventArgs countChangedArgs = new PropertyChangedEventArgs(nameof(Count));

		public static readonly PropertyChangingEventArgs capacityChangingArgs = new PropertyChangingEventArgs(nameof(Capacity));
		public static readonly PropertyChangedEventArgs capacityChangedArgs = new PropertyChangedEventArgs(nameof(Capacity));

		/// <summary>
		///     If <c>true</c>, an equality check will be run before setting an item through the indexer to make sure the new
		///     object is
		///     not the same as the old one.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool SetEqualityCheck { get; set; }

		/// <summary>
		///     If <c>true</c>, the <see cref="ScriptableList{T}" /> will be cleared on play mode start/game boot.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool ClearOnStart { get; set; }

		/// <summary>
		///     Gets the number of elements contained in the <see cref="ScriptableList{T}" />.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int Count { get; protected set; }

		/// <summary>
		///     Gets the total number of elements the internal data structure can hold without resizing.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int Capacity { get; set; }

		/// <summary>
		///     If read only, the <see cref="ScriptableList{T}" /> cannot be changed at runtime and won't be cleared on start.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool IsReadOnly { get; set; }
	}
}