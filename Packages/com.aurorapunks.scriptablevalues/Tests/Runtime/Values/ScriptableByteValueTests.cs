namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableByteValueTests : ScriptableValueTest<ScriptableByte, byte>
	{
		protected override byte MakeDifferentValue(byte value)
		{
			return (byte) (value - 1);
		}
	}
}