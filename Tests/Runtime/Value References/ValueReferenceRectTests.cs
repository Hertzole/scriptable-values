using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceRectTests : BaseValueReferenceTest<ScriptableRect, Rect>
	{
		protected override Rect MakeDifferentValue(Rect value)
		{
			return new Rect(1, 2, 3, 4);
		}
	}
}
