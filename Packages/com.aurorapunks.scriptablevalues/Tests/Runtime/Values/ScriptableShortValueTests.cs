namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableShortValueTests : ScriptableValueTest<ScriptableShort, short>
	{
		protected override short MakeDifferentValue(short value)
		{
			return (short) (value - 1);
		}
	}
}
