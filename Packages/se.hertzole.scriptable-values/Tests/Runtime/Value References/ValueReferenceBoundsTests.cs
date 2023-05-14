using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceBoundsTests : BaseValueReferenceTest<ScriptableBounds, Bounds>
	{
		protected override Bounds MakeDifferentValue(Bounds value)
		{
			return new Bounds(value.center + new Vector3(99, 99, 99), value.size + new Vector3(99, 99, 99));
		}
	}
}
