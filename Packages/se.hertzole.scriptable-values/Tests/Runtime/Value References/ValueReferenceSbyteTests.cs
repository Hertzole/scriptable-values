using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceSbyteTests : BaseValueReferenceTest<ScriptableSByte, sbyte>
	{
		protected override sbyte MakeDifferentValue(sbyte value)
		{
			return (sbyte) (value - 1);
		}
	}
}
