using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableDecimalValueTests : ScriptableValueTest<ScriptableDecimal, decimal>
	{
		protected override decimal MakeDifferentValue(decimal value)
		{
			return (decimal) (value - 1);
		}
	}
}
