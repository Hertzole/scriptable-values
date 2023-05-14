using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceColorTests : BaseValueReferenceTest<ScriptableColor, Color>
	{
		protected override Color MakeDifferentValue(Color value)
		{
			return Color.blue;
		}
	}
}
