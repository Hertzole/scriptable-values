using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceBoolTests : BaseValueReferenceTest<ScriptableBool, bool>
	{
		protected override bool MakeDifferentValue(bool value)
		{
			return !value;
		}
	}
}
