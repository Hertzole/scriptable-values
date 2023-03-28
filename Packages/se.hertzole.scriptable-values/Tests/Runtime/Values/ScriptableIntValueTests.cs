using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableIntValueTests : ScriptableValueTest<ScriptableInt, int>
	{
		protected override int MakeDifferentValue(int value)
		{
			return (int) (value - 1);
		}
	}
}
