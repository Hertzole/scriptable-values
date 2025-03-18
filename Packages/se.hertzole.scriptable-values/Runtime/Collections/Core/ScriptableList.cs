using System.ComponentModel;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

namespace Hertzole.ScriptableValues
{
	public abstract class ScriptableList : RuntimeScriptableObject
	{
		public static readonly PropertyChangedEventArgs clearOnStartChangedArgs = new PropertyChangedEventArgs(nameof(ScriptableList<object>.ClearOnStart));
		public static readonly PropertyChangedEventArgs isReadOnlyChangedArgs = new PropertyChangedEventArgs(nameof(IsReadOnly));
		public static readonly PropertyChangedEventArgs setEqualityCheckChangedArgs = new PropertyChangedEventArgs(nameof(SetEqualityCheck));

		public static readonly PropertyChangedEventArgs countChangedArgs = new PropertyChangedEventArgs(nameof(Count));
		public static readonly PropertyChangedEventArgs capacityChangedArgs = new PropertyChangedEventArgs(nameof(Capacity));
		public static readonly PropertyChangingEventArgs setEqualityCheckChangingArgs = new PropertyChangingEventArgs(nameof(SetEqualityCheck));

		public static readonly PropertyChangingEventArgs clearOnStartChangingArgs = new PropertyChangingEventArgs(nameof(ScriptableList<object>.ClearOnStart));

		public static readonly PropertyChangingEventArgs isReadOnlyChangingArgs = new PropertyChangingEventArgs(nameof(IsReadOnly));

#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool IsReadOnly { get; set; }
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
		public abstract int Count { get; }
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int Capacity { get; }
	}
}