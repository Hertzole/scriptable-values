using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceUlongTests : BaseValueReferenceTest<ScriptableULong, ulong>
	{
		protected override ulong MakeDifferentValue(ulong value)
		{
			return (ulong) (value - 1);
		}
	}
}
