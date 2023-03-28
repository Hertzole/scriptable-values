using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableVector3IntValueTests : ScriptableValueTest<ScriptableVector3Int, Vector3Int>
	{
		protected override Vector3Int MakeDifferentValue(Vector3Int value)
		{
			return (Vector3Int) (value + new Vector3Int(99, 99, 99));
		}
	}
}
