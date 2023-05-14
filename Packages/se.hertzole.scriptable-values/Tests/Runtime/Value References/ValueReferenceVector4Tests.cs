using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceVector4Tests : BaseValueReferenceTest<ScriptableVector4, Vector4>
	{
		protected override Vector4 MakeDifferentValue(Vector4 value)
		{
			if (value == Vector4.zero)
			{
				return Vector4.one;
			}

			return (Vector4) (value * 1.25f);
		}
	}
}
