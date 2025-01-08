using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableLongValueTests : ScriptableValueTest<ScriptableLong, long>
	{
		protected override long MakeDifferentValue(long value)
		{
			return value ^ 31;
		}
	}
}
