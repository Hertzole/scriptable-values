using System.ComponentModel;

namespace Hertzole.ScriptableValues
{
	partial class ScriptableValue
	{
		public static readonly PropertyChangingEventArgs isReadOnlyChangingArgs = new PropertyChangingEventArgs(nameof(IsReadOnly));
		public static readonly PropertyChangedEventArgs isReadOnlyChangedArgs = new PropertyChangedEventArgs(nameof(IsReadOnly));
		
		public static readonly PropertyChangingEventArgs resetValueOnStartChangingArgs = new PropertyChangingEventArgs(nameof(ResetValueOnStart));
		public static readonly PropertyChangedEventArgs resetValueOnStartChangedArgs = new PropertyChangedEventArgs(nameof(ResetValueOnStart));
		
		public static readonly PropertyChangingEventArgs setEqualityCheckChangingArgs = new PropertyChangingEventArgs(nameof(SetEqualityCheck));
		public static readonly PropertyChangedEventArgs setEqualityCheckChangedArgs = new PropertyChangedEventArgs(nameof(SetEqualityCheck));
		
		public static readonly PropertyChangingEventArgs valueChangingArgs = new PropertyChangingEventArgs(nameof(ScriptableValue<object>.Value));
		public static readonly PropertyChangedEventArgs valueChangedArgs = new PropertyChangedEventArgs(nameof(ScriptableValue<object>.Value));
		
		public static readonly PropertyChangingEventArgs previousValueChangingArgs = new PropertyChangingEventArgs(nameof(ScriptableValue<object>.PreviousValue));
		public static readonly PropertyChangedEventArgs previousValueChangedArgs = new PropertyChangedEventArgs(nameof(ScriptableValue<object>.PreviousValue));
		
		public static readonly PropertyChangingEventArgs defaultValueChangingArgs = new PropertyChangingEventArgs(nameof(ScriptableValue<object>.DefaultValue));
		public static readonly PropertyChangedEventArgs defaultValueChangedArgs = new PropertyChangedEventArgs(nameof(ScriptableValue<object>.DefaultValue));
	}
}