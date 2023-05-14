using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceDecimalTests : BaseValueReferenceTest<ScriptableDecimal, decimal>
	{
		protected override decimal MakeDifferentValue(decimal value)
		{
			return (decimal) (value - 1);
		}
	}
}
