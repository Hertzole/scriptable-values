using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace AuroraPunks.ScriptableValues.Tests
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

			for (int i = 0; i < objects.Count; i++)
			{
				if (objects[i] == null)
				{
					continue;
				}

				if (objects[i] is GameObject go)
				{
					Object.Destroy(go);
				}
				else if (objects[i] is Component comp)
				{
					Object.Destroy(comp.gameObject);
				}
				else
				{
					Object.Destroy(objects[i]);
				}
			}

			objects.Clear();

			yield return null;
		}

		protected virtual IEnumerator OnTearDownRoutine()
		{
			yield return null;
		}

		protected virtual void OnTearDown() { }
	}
}