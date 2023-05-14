using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableVector3ValueTests : ScriptableValueTest<ScriptableVector3, Vector3>
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
