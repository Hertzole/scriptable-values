using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableQuaternionValueTests : ScriptableValueTest<ScriptableQuaternion, Quaternion>
	{
		protected override Quaternion MakeDifferentValue(Quaternion value)
		{
			return (Quaternion) (value * Quaternion.Euler(0, 180, 0));
		}
	}
}
