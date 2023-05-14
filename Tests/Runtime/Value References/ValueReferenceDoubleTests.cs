using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceDoubleTests : BaseValueReferenceTest<ScriptableDouble, double>
	{
		protected override double MakeDifferentValue(double value)
		{
			return (double) (value - 1);
		}
	}
}
