﻿using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues.Tests
{
	public class BaseRuntimeTest : BaseTest
	{
		[UnitySetUp]
		public IEnumerator Setup()
		{
			Assert.AreEqual(0, objects.Count);

			OnSetup();

			yield return OnSetupRoutine();
		}

		protected virtual IEnumerator OnSetupRoutine()
		{
			yield return null;
		}

		protected virtual void OnSetup() { }

		[UnityTearDown]
		public IEnumerator TearDown()
		{
			OnTearDown();

			yield return OnTearDownRoutine();

			DestroyObjects(false);

			yield return null;
		}

		protected virtual IEnumerator OnTearDownRoutine()
		{
			yield return null;
		}

		protected virtual void OnTearDown() { }
	}
}