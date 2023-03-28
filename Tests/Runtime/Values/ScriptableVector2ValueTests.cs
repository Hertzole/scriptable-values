using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableVector2ValueTests : ScriptableValueTest<ScriptableVector2, Vector2>
	{
		protected override Vector2 MakeDifferentValue(Vector2 value)
		{
			return (Vector2) (value * 1.25f);
		}
	}
}
