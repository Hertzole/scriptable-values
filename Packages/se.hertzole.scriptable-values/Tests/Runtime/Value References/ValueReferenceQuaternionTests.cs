using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceQuaternionTests : BaseValueReferenceTest<ScriptableQuaternion, Quaternion>
	{
		protected override Quaternion MakeDifferentValue(Quaternion value)
		{
			if (value.x == 0f && value.y == 0f && value.z == 0f && value.w == 0f)
			{
				return Quaternion.Euler(0, 180, 0);
			}

			return (Quaternion) (value * Quaternion.Euler(0, 180, 0));
		}
	}
}
