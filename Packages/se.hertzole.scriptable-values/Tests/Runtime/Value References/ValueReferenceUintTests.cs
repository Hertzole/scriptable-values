using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceUintTests : BaseValueReferenceTest<ScriptableUInt, uint>
	{
		protected override uint MakeDifferentValue(uint value)
		{
			return (uint) (value - 1);
		}
	}
}
