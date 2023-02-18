using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableBoundsIntValueTests : ScriptableValueTest<ScriptableBoundsInt, BoundsInt>
	{
		protected override BoundsInt MakeDifferentValue(BoundsInt value)
		{
			return new BoundsInt(value.position + new Vector3Int(99, 99, 99), value.size + new Vector3Int(99, 99, 99));
		}
	}
}
