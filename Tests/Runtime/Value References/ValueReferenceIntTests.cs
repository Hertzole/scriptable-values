using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceIntTests : BaseValueReferenceTest<ScriptableInt, int>
	{
		protected override int MakeDifferentValue(int value)
		{
			return (int) (value - 1);
		}
	}
}
