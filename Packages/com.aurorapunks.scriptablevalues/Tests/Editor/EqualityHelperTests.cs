using AuroraPunks.ScriptableValues.Helpers;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class EqualityHelperTests
	{
		[Test]
		public void NonUnityObject_Equals()
		{
			Assert.IsTrue(EqualityHelper.Equals(0, 0));
		}
		
		[Test]
		public void NonUnityObject_NotEquals()
		{
			Assert.IsFalse(EqualityHelper.Equals(0, 1));
		}
		[Test]
		public void UnityObject_Equals()
		{
			GameObject go = new GameObject();
			
			Assert.IsTrue(EqualityHelper.Equals(go, go));

			Object.DestroyImmediate(go);
		}
		
		[Test]
		public void UnityObject_NotEquals()
		{
			GameObject go1 = new GameObject();
			GameObject go2 = new GameObject();
			
			Assert.IsFalse(EqualityHelper.Equals(go1, go2));
			
			Object.DestroyImmediate(go1);
			Object.DestroyImmediate(go2);
		}
	}
}