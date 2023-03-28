using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableColor32ValueTests : ScriptableValueTest<ScriptableColor32, Color32>
	{
		protected override Color32 MakeDifferentValue(Color32 value)
		{
			return Color.blue;
		}
	}
}
