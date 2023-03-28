using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableVector2IntValueTests : ScriptableValueTest<ScriptableVector2Int, Vector2Int>
	{
		protected override Vector2Int MakeDifferentValue(Vector2Int value)
		{
			return (Vector2Int) (value + new Vector2Int(99, 99));
		}
	}
}
