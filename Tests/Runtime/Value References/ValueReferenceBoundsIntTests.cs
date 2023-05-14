using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceBoundsIntTests : BaseValueReferenceTest<ScriptableBoundsInt, BoundsInt>
	{
		protected override BoundsInt MakeDifferentValue(BoundsInt value)
		{
			return new BoundsInt(value.position + new Vector3Int(99, 99, 99), value.size + new Vector3Int(99, 99, 99));
		}
	}
}
