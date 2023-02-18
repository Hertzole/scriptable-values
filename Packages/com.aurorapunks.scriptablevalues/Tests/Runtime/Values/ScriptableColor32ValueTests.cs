using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableColor32ValueTests : ScriptableValueTest<ScriptableColor32, Color32>
	{
		protected override Color32 MakeDifferentValue(Color32 value)
		{
			return Color.blue;
		}
	}
}
