using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceStringTests : BaseValueReferenceTest<ScriptableString, string>
	{
		protected override string MakeDifferentValue(string value)
		{
			return value + "1";
		}
	}
}
