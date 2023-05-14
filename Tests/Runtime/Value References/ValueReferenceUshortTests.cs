using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceUshortTests : BaseValueReferenceTest<ScriptableUShort, ushort>
	{
		protected override ushort MakeDifferentValue(ushort value)
		{
			return (ushort) (value - 1);
		}
	}
}
