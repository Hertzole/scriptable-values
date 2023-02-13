﻿namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableDoubleValueTests : ScriptableValueTest<ScriptableDouble, double>
	{
		protected override double MakeDifferentValue(double value)
		{
			return value - 1;
		}
	}
}