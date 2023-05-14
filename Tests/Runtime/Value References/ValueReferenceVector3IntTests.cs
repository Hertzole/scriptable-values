using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceVector3IntTests : BaseValueReferenceTest<ScriptableVector3Int, Vector3Int>
	{
		protected override Vector3Int MakeDifferentValue(Vector3Int value)
		{
			return (Vector3Int) (value + new Vector3Int(99, 99, 99));
		}
	}
}
