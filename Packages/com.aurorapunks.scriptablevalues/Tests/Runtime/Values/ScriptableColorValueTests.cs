using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableColorValueTests : ScriptableValueTest<ScriptableColor, Color>
	{
		protected override Color MakeDifferentValue(Color value)
		{
			return Color.blue;
		}
	}
}
