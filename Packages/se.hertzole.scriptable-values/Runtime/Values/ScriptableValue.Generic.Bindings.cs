#if SCRIPTABLE_VALUES_RUNTIME_BINDING && SCRIPTABLE_VALUES_UITOOLKIT
using System.Collections.Generic;

namespace Hertzole.ScriptableValues
{
    partial class ScriptableValue<T>
    {
        /// <inheritdoc />
        protected override long GetViewHashCode()
        {
            unchecked
            {
                EqualityComparer<T> comparer = EqualityComparer<T>.Default;

                long hash = 17;
                hash = hash * 23 + base.GetViewHashCode();
                hash = hash * 23 + comparer.GetHashCode(Value);
                hash = hash * 23 + comparer.GetHashCode(PreviousValue);
                hash = hash * 23 + comparer.GetHashCode(DefaultValue);
                return hash;
            }
        }
    }
}
#endif