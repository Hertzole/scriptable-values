using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceFloatTests : BaseValueReferenceTest<ScriptableFloat, float>
	{
		protected override float MakeDifferentValue(float value)
		{
			return (float) (value - 1);
		}
	}
}
