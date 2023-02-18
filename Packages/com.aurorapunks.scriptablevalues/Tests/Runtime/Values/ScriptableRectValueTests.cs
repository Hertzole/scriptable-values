using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableRectValueTests : ScriptableValueTest<ScriptableRect, Rect>
	{
		protected override Rect MakeDifferentValue(Rect value)
		{
			return new Rect(1, 2, 3, 4);
		}
	}
}
