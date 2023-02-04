using AuroraPunks.ScriptableValues.Helpers;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class EqualityHelperTests : BaseEditorTest
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
			GameObject go = CreateGameObject();

			Assert.IsTrue(EqualityHelper.Equals(go, go));
		}

		[Test]
		public void UnityObject_Equals_Null()
		{
			Assert.IsTrue(EqualityHelper.Equals((Object) null, null));
		}

		[Test]
		public void UnityObject_NotEquals()
		{
			GameObject go1 = CreateGameObject();
			GameObject go2 = CreateGameObject();

			Assert.IsFalse(EqualityHelper.Equals(go1, go2));
		}

		[Test]
		public void UnityObject_NotEquals_Null()
		{
			GameObject go1 = CreateGameObject();

			Assert.IsFalse(EqualityHelper.Equals(go1, null));
		}

		[Test]
		public void NonUnityObject_IsNull()
		{
			string value = null;

			Assert.IsTrue(EqualityHelper.IsNull(value));
		}

		[Test]
		public void NonUnityObject_IsNotNull()
		{
			string value = "test";

			Assert.IsFalse(EqualityHelper.IsNull(value));
		}

		[Test]
		public void UnityObject_IsNull()
		{
			GameObject go = null;

			Assert.IsTrue(EqualityHelper.IsNull(go));
		}

		[Test]
		public void UnityObject_IsNotNull()
		{
			GameObject go = CreateGameObject();

			Assert.IsFalse(EqualityHelper.IsNull(go));
		}
	}
}