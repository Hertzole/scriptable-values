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

#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool SetEqualityCheck { get; set; }
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool ClearOnStart { get; set; }
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int Count { get; protected set; }
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int Capacity { get; set; }

#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool IsReadOnly { get; set; }
	}
}