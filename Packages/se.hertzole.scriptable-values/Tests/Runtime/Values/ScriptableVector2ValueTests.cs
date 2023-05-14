using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableVector2ValueTests : ScriptableValueTest<ScriptableVector2, Vector2>
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
