namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableULongValueTests : ScriptableValueTest<ScriptableULong, ulong>
	{
		protected override ulong MakeDifferentValue(ulong value)
		{
			return (ulong) (value - 1);
		}
	}
}
