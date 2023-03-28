using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableRectIntValueTests : ScriptableValueTest<ScriptableRectInt, RectInt>
	{
		protected override RectInt MakeDifferentValue(RectInt value)
		{
			return new RectInt(1, 2, 3, 4);
		}
	}
}
