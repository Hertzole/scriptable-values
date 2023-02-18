using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableUIntValueTests : ScriptableValueTest<ScriptableUInt, uint>
	{
		protected override uint MakeDifferentValue(uint value)
		{
			return (uint) (value - 1);
		}
	}
}
