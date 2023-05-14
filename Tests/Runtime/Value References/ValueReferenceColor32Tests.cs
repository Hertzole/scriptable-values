using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceColor32Tests : BaseValueReferenceTest<ScriptableColor32, Color32>
	{
		protected override Color32 MakeDifferentValue(Color32 value)
		{
			return Color.blue;
		}
	}
}
