using System.ComponentModel;

namespace Hertzole.ScriptableValues
{
    partial class ScriptableEvent
    {
        public static readonly PropertyChangedEventArgs previousArgsChanged = new PropertyChangedEventArgs(nameof(ScriptableEvent<object>.PreviousArgs));
        public static readonly PropertyChangingEventArgs previousArgsChanging = new PropertyChangingEventArgs(nameof(ScriptableEvent<object>.PreviousArgs));
    }
}