using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableBoundsIntValueTests : ScriptableValueTest<ScriptableBoundsInt, BoundsInt>
	{
		protected override BoundsInt MakeDifferentValue(BoundsInt value)
		{
			return new BoundsInt(value.position + new Vector3Int(99, 99, 99), value.size + new Vector3Int(99, 99, 99));
		}
	}
}
