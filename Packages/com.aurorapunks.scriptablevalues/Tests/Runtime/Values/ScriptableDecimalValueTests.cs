namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableDecimalValueTests : ScriptableValueTest<ScriptableDecimal, decimal>
	{
		protected override decimal MakeDifferentValue(decimal value)
		{
			return value - 1;
		}
	}
}