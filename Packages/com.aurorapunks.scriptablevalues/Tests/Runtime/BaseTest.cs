using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public abstract class BaseTest
	{
		protected readonly List<Object> objects = new List<Object>();

		public static readonly bool[] bools = { true, false };
		public static readonly byte[] bytes = { byte.MinValue, byte.MaxValue, 2 };
		public static readonly sbyte[] sbytes = { sbyte.MinValue, sbyte.MaxValue, 0, 2, -2 };
		public static readonly short[] shorts = { short.MinValue, short.MaxValue, 0, 2, -2 };
		public static readonly ushort[] ushorts = { ushort.MinValue, ushort.MaxValue, 1 };
		public static readonly int[] ints = { int.MinValue, int.MaxValue, 0, 2, -2 };
		public static readonly uint[] uints = { uint.MinValue, uint.MaxValue, 2 };
		public static readonly long[] longs = { long.MinValue, long.MaxValue, 0, 2, -2 };
		public static readonly ulong[] ulongs = { ulong.MinValue, ulong.MaxValue, 2 };
		public static readonly float[] floats = { -69.420f, 69.420f, 0, 2, -2 };
		public static readonly double[] doubles = { -69.420, 69.420, 0, 2, -2 };
		public static readonly decimal[] decimals = { -69.420m, 69.420m, 0, 2, -2, decimal.One, decimal.Zero, decimal.MinusOne };
		public static readonly string[] strings = { string.Empty, "hello", "WoRld", null };
		public static readonly char[] chars = { char.MinValue, char.MaxValue, 'a' };
		public static readonly Color[] colors = { Color.black, Color.white };
		public static readonly Color32[] colors32 = { Color.black, Color.white };
		public static readonly Vector2[] vectors2 = { Vector2.down, Vector2.one };
		public static readonly Vector3[] vectors3 = { Vector3.down, Vector3.one };
		public static readonly Vector4[] vectors4 = { -Vector4.one, Vector4.one };
		public static readonly Vector2Int[] vectors2Int = { Vector2Int.down, Vector2Int.one };
		public static readonly Vector3Int[] vectors3Int = { Vector3Int.down, Vector3Int.one };
		public static readonly Quaternion[] quaternions = { Quaternion.Euler(90, 0, 0), Quaternion.Euler(0, 0, 90) };
		public static readonly Rect[] rects = { Rect.zero, new Rect(-1, -1, 1, 1) };
		public static readonly RectInt[] rectsInt = { new RectInt(0, 0, 0, 0), new RectInt(-1, -1, 1, 1) };
		public static readonly Bounds[] bounds = { new Bounds(Vector3.zero, Vector3.one), new Bounds(Vector3.one, Vector3.one) };
		public static readonly BoundsInt[] boundsInt = { new BoundsInt(Vector3Int.zero, Vector3Int.one), new BoundsInt(Vector3Int.one, Vector3Int.one) };
		
		public static readonly InvokeEvents[] valueInvokes =
		{
			InvokeEvents.Any,
			InvokeEvents.FromValue,
			InvokeEvents.ToValue,
			InvokeEvents.FromValueToValue
		};

		public static readonly InvokeParameters[] invokeParameters =
		{
			InvokeParameters.Single,
			InvokeParameters.Multiple,
			InvokeParameters.Both
		};
		
		public static readonly EventInvokeEvents[] eventInvokes =
		{
			EventInvokeEvents.Any,
			EventInvokeEvents.FromValue,
			EventInvokeEvents.ToValue
		};

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