using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceVector2Tests : BaseValueReferenceTest<ScriptableVector2, Vector2>
	{
		protected override Vector2 MakeDifferentValue(Vector2 value)
		{
			if (value == Vector2.zero)
			{
				return Vector2.one;
			}

			return (Vector2) (value * 1.25f);
		}
	}
}
