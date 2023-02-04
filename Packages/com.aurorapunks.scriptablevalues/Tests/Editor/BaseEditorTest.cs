using NUnit.Framework;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class BaseEditorTest : BaseTest
	{
		[SetUp]
		public void Setup()
		{
			OnSetup();
		}

		protected virtual void OnSetup() { }

		[TearDown]
		public void TearDown()
		{
			OnTearDown();

			for (int i = 0; i < objects.Count; i++)
			{
				if (objects[i] == null)
				{
					continue;
				}

				if (objects[i] is GameObject go)
				{
					Object.DestroyImmediate(go);
				}
				else if (objects[i] is Component comp)
				{
					Object.DestroyImmediate(comp.gameObject);
				}
				else
				{
					Object.DestroyImmediate(objects[i]);
				}
			}

			objects.Clear();
		}

		protected virtual void OnTearDown() { }
	}
}