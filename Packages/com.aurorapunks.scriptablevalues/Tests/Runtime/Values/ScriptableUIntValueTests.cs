namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableUIntValueTests : ScriptableValueTest<ScriptableUInt, uint>
	{
		protected override uint MakeDifferentValue(uint value)
		{
			return value - 1;
		}
	}
}