using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableQuaternionValueTests : ScriptableValueTest<ScriptableQuaternion, Quaternion>
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
