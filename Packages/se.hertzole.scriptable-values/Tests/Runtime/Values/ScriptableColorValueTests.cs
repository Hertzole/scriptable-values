using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableColorValueTests : ScriptableValueTest<ScriptableColor, Color>
	{
		protected override Color MakeDifferentValue(Color value)
		{
			return Color.blue;
		}
	}
}
