using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableIntValueTests : ScriptableValueTest<ScriptableInt, int>
	{
		protected override int MakeDifferentValue(int value)
		{
			return (int) (value - 1);
		}
	}
}
