namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableULongValueTests : ScriptableValueTest<ScriptableULong, ulong>
	{
		protected override ulong MakeDifferentValue(ulong value)
		{
			return value - 1;
		}
	}
}