namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableStringValueTests : ScriptableValueTest<ScriptableString, string>
	{
		protected override string MakeDifferentValue(string value)
		{
			return value + "1";
		}
	}
}
