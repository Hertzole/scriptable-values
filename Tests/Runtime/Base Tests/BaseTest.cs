using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hertzole.ScriptableValues.Tests
{
	public abstract partial class BaseTest
	{
		protected readonly List<Object> objects = new List<Object>();

		public static readonly bool[] bools = { true, false };
		public static readonly byte[] bytes = { byte.MinValue, byte.MaxValue, 1 };
		public static readonly sbyte[] sbytes = { sbyte.MinValue, sbyte.MaxValue, 0 };
		public static readonly short[] shorts = { short.MinValue, short.MaxValue, 0 };
		public static readonly ushort[] ushorts = { ushort.MinValue, ushort.MaxValue, 0 };
		public static readonly int[] ints = { int.MinValue, int.MaxValue, 0 };
		public static readonly uint[] uints = { uint.MinValue, uint.MaxValue, 0 };
		public static readonly long[] longs = { long.MinValue, long.MaxValue, 0 };
		public static readonly ulong[] ulongs = { ulong.MinValue, ulong.MaxValue, 0 };
		public static readonly float[] floats = { -69.420f, 69.420f, 0 };
		public static readonly double[] doubles = { -69.420, 69.420, 0 };
		public static readonly decimal[] decimals = { -69.420m, 69.420m, 0 };
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

		protected void DestroyObjects(bool destroyImmediate)
		{
			for (int i = 0; i < objects.Count; i++)
			{
				if (objects[i] == null)
				{
					continue;
				}

				if (objects[i] is GameObject go)
				{
					if (destroyImmediate)
					{
						Object.DestroyImmediate(go);
					}
					else
					{
						Object.Destroy(go);
					}
				}
				else if (objects[i] is Component comp)
				{
					if (destroyImmediate)
					{
						Object.DestroyImmediate(comp.gameObject);
					}
					else
					{
						Object.Destroy(comp.gameObject);
					}
				}
				else
				{
					if (destroyImmediate)
					{
						Object.DestroyImmediate(objects[i]);
					}
					else
					{
						Object.Destroy(objects[i]);
					}
				}
			}

			objects.Clear();
		}

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

		protected T MakeDifferentValue<T>(T value)
		{
			if (typeof(T) == typeof(bool))
			{
				return ConvertAndSetValue<T, bool>(value, oldValue => !oldValue);
			}

			if (typeof(T) == typeof(BoundsInt))
			{
				return ConvertAndSetValue<T, BoundsInt>(value, oldValue => new BoundsInt(oldValue.min + Vector3Int.one, oldValue.size + Vector3Int.back));
			}

			if (typeof(T) == typeof(Bounds))
			{
				return ConvertAndSetValue<T, Bounds>(value, oldValue => new Bounds(oldValue.min + Vector3.one, oldValue.size + Vector3.back));
			}

			if (typeof(T) == typeof(byte))
			{
				return ConvertAndSetValue<T, byte>(value, oldValue => (byte) (oldValue + 1));
			}

			if (typeof(T) == typeof(char))
			{
				return ConvertAndSetValue<T, char>(value, oldValue => (char) (oldValue + 1));
			}

			if (typeof(T) == typeof(Color32))
			{
				return ConvertAndSetValue<T, Color32>(value,
					oldValue => new Color32((byte) (oldValue.r + 1), (byte) (oldValue.g + 1), (byte) (oldValue.b + 1), (byte) (oldValue.a + 1)));
			}

			if (typeof(T) == typeof(Color))
			{
				return ConvertAndSetValue<T, Color>(value, oldValue => new Color(oldValue.r + 0.1f, oldValue.g + 0.25f, oldValue.b + 0.75f, oldValue.a + 1));
			}

			if (typeof(T) == typeof(decimal))
			{
				return ConvertAndSetValue<T, decimal>(value, oldValue => oldValue + 1);
			}

			if (typeof(T) == typeof(double))
			{
				return ConvertAndSetValue<T, double>(value, oldValue => oldValue + 1);
			}

			if (typeof(T) == typeof(float))
			{
				return ConvertAndSetValue<T, float>(value, oldValue => oldValue + 1);
			}

			if (typeof(T) == typeof(GameObject))
			{
				return ConvertAndSetValue<T, GameObject>(value, oldValue => CreateGameObject());
			}

			if (typeof(T) == typeof(int))
			{
				return ConvertAndSetValue<T, int>(value, oldValue => oldValue + 1);
			}

			if (typeof(T) == typeof(long))
			{
				return ConvertAndSetValue<T, long>(value, oldValue => oldValue + 1);
			}

			if (typeof(T) == typeof(Quaternion))
			{
				return ConvertAndSetValue<T, Quaternion>(value, oldValue => Quaternion.Euler(oldValue.eulerAngles + Vector3.one));
			}

			if (typeof(T) == typeof(RectInt))
			{
				return ConvertAndSetValue<T, RectInt>(value, oldValue => new RectInt(oldValue.position + Vector2Int.one, oldValue.size + Vector2Int.one));
			}

			if (typeof(T) == typeof(Rect))
			{
				return ConvertAndSetValue<T, Rect>(value, oldValue => new Rect(oldValue.position + Vector2.one, oldValue.size + Vector2.one));
			}

			if (typeof(T) == typeof(sbyte))
			{
				return ConvertAndSetValue<T, sbyte>(value, oldValue => (sbyte) (oldValue + 1));
			}
			
			if (typeof(T) == typeof(short))
			{
				return ConvertAndSetValue<T, short>(value, oldValue => (short) (oldValue + 1));
			}
			
			if (typeof(T) == typeof(string))
			{
				return ConvertAndSetValue<T, string>(value, oldValue => oldValue + "a");
			}
			
			if (typeof(T) == typeof(uint))
			{
				return ConvertAndSetValue<T, uint>(value, oldValue => oldValue + 1);
			}
			
			if (typeof(T) == typeof(ulong))
			{
				return ConvertAndSetValue<T, ulong>(value, oldValue => oldValue + 1);
			}
			
			if (typeof(T) == typeof(ushort))
			{
				return ConvertAndSetValue<T, ushort>(value, oldValue => (ushort) (oldValue + 1));
			}
			
			if (typeof(T) == typeof(Vector2))
			{
				return ConvertAndSetValue<T, Vector2>(value, oldValue => oldValue + Vector2.one);
			}
			
			if (typeof(T) == typeof(Vector2Int))
			{
				return ConvertAndSetValue<T, Vector2Int>(value, oldValue => oldValue + Vector2Int.one);
			}
			
			if (typeof(T) == typeof(Vector3))
			{
				return ConvertAndSetValue<T, Vector3>(value, oldValue => oldValue + Vector3.one);
			}
			
			if (typeof(T) == typeof(Vector3Int))
			{
				return ConvertAndSetValue<T, Vector3Int>(value, oldValue => oldValue + Vector3Int.one);
			}
			
			if (typeof(T) == typeof(Vector4))
			{
				return ConvertAndSetValue<T, Vector4>(value, oldValue => oldValue + Vector4.one);
			}

			throw new NotSupportedException($"Type {typeof(T)} is not supported.");
		}

		private static TFrom ConvertAndSetValue<TFrom, TTo>(TFrom value, Func<TTo, TTo> setValue)
		{
			TTo castedValue = UnsafeUtility.As<TFrom, TTo>(ref value);
			castedValue = setValue(castedValue);
			return UnsafeUtility.As<TTo, TFrom>(ref castedValue);
		}
	}
}