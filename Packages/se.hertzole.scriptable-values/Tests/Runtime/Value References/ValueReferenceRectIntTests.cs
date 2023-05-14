using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceRectIntTests : BaseValueReferenceTest<ScriptableRectInt, RectInt>
	{
		protected override RectInt MakeDifferentValue(RectInt value)
		{
			return new RectInt(1, 2, 3, 4);
		}
	}
}
