using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceVector2IntTests : BaseValueReferenceTest<ScriptableVector2Int, Vector2Int>
	{
		protected override Vector2Int MakeDifferentValue(Vector2Int value)
		{
			return (Vector2Int) (value + new Vector2Int(99, 99));
		}
	}
}
