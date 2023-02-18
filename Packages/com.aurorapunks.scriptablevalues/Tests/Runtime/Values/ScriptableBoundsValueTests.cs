using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableBoundsValueTests : ScriptableValueTest<ScriptableBounds, Bounds>
	{
		protected override Bounds MakeDifferentValue(Bounds value)
		{
			return new Bounds(value.center + new Vector3(99, 99, 99), value.size + new Vector3(99, 99, 99));
		}
	}
}
