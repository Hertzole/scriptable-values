using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class BaseTest
	{
		private readonly List<Object> objects = new List<Object>();

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

		protected T CreateInstance<T>() where T : ScriptableObject
		{
			T instance = ScriptableObject.CreateInstance<T>();
			objects.Add(instance);

			return instance;
		}

		protected GameObject CreateGameObject(string name = "")
		{
			GameObject go = new GameObject(name);
			objects.Add(go);

			return go;
		}

		protected T CreateComponent<T>(GameObject go = null) where T : Component
		{
			go ??= new GameObject();
			T comp = go.AddComponent<T>();
			objects.Add(comp);

			return comp;
		}

		protected T Instantiate<T>(T prefab) where T : Object
		{
			var instance = Object.Instantiate(prefab);
			objects.Add(instance);
			
			return instance;
		}

		protected void Destroy(Object obj)
		{
			Object.Destroy(obj);
			objects.Remove(obj);
		}
	}
}