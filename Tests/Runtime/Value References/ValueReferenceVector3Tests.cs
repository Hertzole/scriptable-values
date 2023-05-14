using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceVector3Tests : BaseValueReferenceTest<ScriptableVector3, Vector3>
	{
		protected override Vector3 MakeDifferentValue(Vector3 value)
		{
			if (value == Vector3.zero)
			{
				return Vector3.one;
			}

			return (Vector3) (value * 1.25f);
		}
	}
}
