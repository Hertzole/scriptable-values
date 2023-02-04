using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class BaseTest
	{
		protected readonly List<Object> objects = new List<Object>();

		protected static readonly bool[] bools = { true, false };
		protected static readonly byte[] bytes = { byte.MinValue, byte.MaxValue, 2 };
		protected static readonly sbyte[] sbytes = { sbyte.MinValue, sbyte.MaxValue, 0, 2, -2 };
		protected static readonly short[] shorts = { short.MinValue, short.MaxValue, 0, 2, -2 };
		protected static readonly ushort[] ushorts = { ushort.MinValue, ushort.MaxValue, 1 };
		protected static readonly int[] ints = { int.MinValue, int.MaxValue, 0, 2, -2 };
		protected static readonly uint[] uints = { uint.MinValue, uint.MaxValue, 2 };
		protected static readonly long[] longs = { long.MinValue, long.MaxValue, 0, 2, -2 };
		protected static readonly ulong[] ulongs = { ulong.MinValue, ulong.MaxValue, 2 };
		protected static readonly float[] floats = { -69.420f, 69.420f, 0, 2, -2 };
		protected static readonly double[] doubles = { -69.420, 69.420, 0, 2, -2 };
		protected static readonly decimal[] decimals = { -69.420m, 69.420m, 0, 2, -2, decimal.One, decimal.Zero, decimal.MinusOne };
		protected static readonly string[] strings = { string.Empty, "hello", "WoRld", null };

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
		
		protected T CreateInstance<T>() where T : ScriptableObject
		{
			T instance = ScriptableObject.CreateInstance<T>();
			// Set DontSave so it doesn't get destroyed when exiting play mode.
			instance.hideFlags = HideFlags.DontSave;
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
			if (go == null)
			{
				go = new GameObject();
				objects.Add(go);
			}

			T comp = go.AddComponent<T>();

			return comp;
		}

		protected T Instantiate<T>(T prefab) where T : Object
		{
			T instance = Object.Instantiate(prefab);
			objects.Add(instance);

			return instance;
		}

		protected void Destroy(Object obj)
		{
			objects.Remove(obj);
			Object.Destroy(obj);
		}
	}
}