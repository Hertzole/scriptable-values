namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableUShortValueTests : ScriptableValueTest<ScriptableUShort, ushort>
	{
		protected override ushort MakeDifferentValue(ushort value)
		{
			return (ushort) (value - 1);
		}
	}
}
