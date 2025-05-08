#if SCRIPTABLE_VALUES_RUNTIME_BINDING
namespace Hertzole.ScriptableValues
{
	partial class ScriptablePool<T>
	{
		/// <inheritdoc />
		protected override long GetViewHashCode()
		{
			unchecked
			{
				long hashCode = 17;

				hashCode = hashCode * 23 + base.GetViewHashCode();
				hashCode = hashCode * 23 + CountAll.GetHashCode();
				hashCode = hashCode * 23 + CountActive.GetHashCode();
				hashCode = hashCode * 23 + CountInactive.GetHashCode();

				return hashCode;
			}
		}
	}
}
#endif