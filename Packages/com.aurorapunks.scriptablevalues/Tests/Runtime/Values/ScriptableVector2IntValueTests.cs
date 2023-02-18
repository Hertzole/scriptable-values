using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableVector2IntValueTests : ScriptableValueTest<ScriptableVector2Int, Vector2Int>
	{
		protected override Vector2Int MakeDifferentValue(Vector2Int value)
		{
			return (Vector2Int) (value + new Vector2Int(99, 99));
		}
	}
}
