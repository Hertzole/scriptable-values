using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceShortTests : BaseValueReferenceTest<ScriptableShort, short>
	{
		protected override short MakeDifferentValue(short value)
		{
			return (short) (value - 1);
		}
	}
}
