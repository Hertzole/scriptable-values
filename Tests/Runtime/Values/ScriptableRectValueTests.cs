using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableRectValueTests : ScriptableValueTest<ScriptableRect, Rect>
	{
		protected override Rect MakeDifferentValue(Rect value)
		{
			return new Rect(1, 2, 3, 4);
		}
	}
}
