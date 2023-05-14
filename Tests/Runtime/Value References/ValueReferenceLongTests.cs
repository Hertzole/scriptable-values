using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceLongTests : BaseValueReferenceTest<ScriptableLong, long>
	{
		protected override long MakeDifferentValue(long value)
		{
			return (long) (value - 1);
		}
	}
}
