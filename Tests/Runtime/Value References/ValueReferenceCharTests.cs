using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceCharTests : BaseValueReferenceTest<ScriptableChar, char>
	{
		protected override char MakeDifferentValue(char value)
		{
			return (char) (value - 1);
		}
	}
}
