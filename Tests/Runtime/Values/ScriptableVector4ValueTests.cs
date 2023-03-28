using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableVector4ValueTests : ScriptableValueTest<ScriptableVector4, Vector4>
	{
		protected override Vector4 MakeDifferentValue(Vector4 value)
		{
			return (Vector4) (value * 1.25f);
		}
	}
}
