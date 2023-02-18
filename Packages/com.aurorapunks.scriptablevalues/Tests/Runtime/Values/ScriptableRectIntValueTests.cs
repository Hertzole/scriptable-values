using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableRectIntValueTests : ScriptableValueTest<ScriptableRectInt, RectInt>
	{
		protected override RectInt MakeDifferentValue(RectInt value)
		{
			return new RectInt(1, 2, 3, 4);
		}
	}
}
