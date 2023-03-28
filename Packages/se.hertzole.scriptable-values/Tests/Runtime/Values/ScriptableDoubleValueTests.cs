using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableDoubleValueTests : ScriptableValueTest<ScriptableDouble, double>
	{
		protected override double MakeDifferentValue(double value)
		{
			return (double) (value - 1);
		}
	}
}
