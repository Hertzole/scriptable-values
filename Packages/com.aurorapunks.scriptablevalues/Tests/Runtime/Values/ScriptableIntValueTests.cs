namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableIntValueTests : ScriptableValueTest<ScriptableInt, int>
	{
		protected override int MakeDifferentValue(int value)
		{
			return value - 1;
		}
	}
}