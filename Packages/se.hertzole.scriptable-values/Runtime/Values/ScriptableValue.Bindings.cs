#if SCRIPTABLE_VALUES_RUNTIME_BINDING
namespace Hertzole.ScriptableValues
{
    partial class ScriptableValue
    {
        /// <inheritdoc />
        protected override long GetViewHashCode()
        {
            unchecked
            {
                long hash = 17;
                hash = hash * 23 + base.GetViewHashCode();
                hash = hash * 23 + isReadOnly.GetHashCode();
                hash = hash * 23 + resetValueOnStart.GetHashCode();
                hash = hash * 23 + setEqualityCheck.GetHashCode();
                return hash;
            }
        }
    }
}
#endif