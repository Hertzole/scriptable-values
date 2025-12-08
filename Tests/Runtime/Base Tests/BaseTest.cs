#nullable enable

#if SCRIPTABLE_VALUES_RUNTIME_BINDING && SCRIPTABLE_VALUES_UITOOLKIT
#define SCRIPTABLE_VALUES_DO_UI_TOOLKIT
#endif

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#if SCRIPTABLE_VALUES_DO_UI_TOOLKIT
using UnityEngine.UIElements;
#endif

namespace Hertzole.ScriptableValues.Tests
{
    public abstract partial class BaseTest
    {
        protected static readonly List<Object> objects = new List<Object>();

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
        public static readonly string[] strings = { string.Empty, "hello", "WoRld", null! };
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

        protected static void DestroyObjects(bool destroyImmediate)
        {
            List<Object> objectsToDestroy = new List<Object>(objects);
            objects.Clear();

            // Sort by type to ensure components and game objects are destroyed before scriptable objects.
            objectsToDestroy.Sort(static (a, b) =>
            {
                if (a is ScriptableObject && b is not ScriptableObject)
                {
                    return 1;
                }

                if (a is not ScriptableObject && b is ScriptableObject)
                {
                    return -1;
                }

                return 0;
            });

            for (int i = 0; i < objectsToDestroy.Count; i++)
            {
                if (objectsToDestroy[i] == null)
                {
                    continue;
                }

                if (destroyImmediate)
                {
                    Object.DestroyImmediate(objectsToDestroy[i]);
                }
                else
                {
                    Object.Destroy(objectsToDestroy[i]);
                }
            }

            objectsToDestroy.Clear();
        }

        protected static T CreateInstance<T>() where T : ScriptableObject
        {
            T instance = ScriptableObject.CreateInstance<T>();
            if (instance == null)
            {
                throw new NotSupportedException($"Can't create scriptable object from {typeof(T)}.");
            }

            // Set DontSave so it doesn't get destroyed when exiting play mode.
            instance.hideFlags = HideFlags.DontSave;
            instance.name = $"{TestContext.CurrentContext.Test.Name} <{typeof(T).Name}> Instance";
            objects.Add(instance);

            return instance;
        }

        protected static GameObject CreateGameObject(string name = "")
        {
            GameObject go = new GameObject(name);
            objects.Add(go);

            return go;
        }

        protected static T CreateComponent<T>(GameObject? targetObject = null) where T : Component
        {
            if (targetObject == null)
            {
                targetObject = new GameObject($"{TestContext.CurrentContext.Test.Name} <{typeof(T).Name}> Object");
                objects.Add(targetObject);
            }

            T comp = targetObject.AddComponent<T>();

            return comp;
        }

        protected static T Instantiate<T>(T prefab) where T : Object
        {
            T instance = Object.Instantiate(prefab);
            if (instance is Component comp)
            {
                objects.Add(comp.gameObject);
            }
            else
            {
                objects.Add(instance);
            }

            return instance;
        }

        protected static void Destroy(Object obj)
        {
            objects.Remove(obj);
            Object.Destroy(obj);
        }

        protected static T MakeDifferentValue<T>(T value)
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
                return ConvertAndSetValue<T, GameObject>(value, _ => CreateGameObject());
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

        protected static void AssertThrows<T>(TestDelegate action) where T : Exception
        {
            Assert.Throws<T>(action);
        }

        protected static void ShuffleArray<T>(T[] array)
        {
            System.Random random = new System.Random();

            int n = array.Length;
            while (n > 1)
            {
                int k = random.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        protected static T[] GetShuffledArray<T>(int count = 1000)
        {
            T[] items = Enumerable.Range(0, count).Select(x => (T) Convert.ChangeType(x, typeof(T))).ToArray();
            ShuffleArray(items);
            return items;
        }

        protected static int GetRandomNumber(int tolerance = 10)
        {
            int result = 0;
            int min = -tolerance;
            int max = tolerance;
            int tries = 0;

            while ((result > min || result < max) && tries < 100)
            {
                result = Random.Range(int.MinValue, int.MaxValue);
                tries++;
            }

            return result;
        }

        protected static void AssertIsSorted<T>(IReadOnlyList<T> actual, Comparison<T> comparison)
        {
            AssertIsSorted(actual, 0, actual.Count, new ComparisonComparer<T>(comparison));
        }

        protected static void AssertIsSorted<T>(IReadOnlyList<T> actual, int index, int count, IComparer<T>? comparer = null)
        {
            T[] expected = new T[actual.Count];
            for (int i = 0; i < actual.Count; i++)
            {
                expected[i] = actual[i];
            }

            Array.Sort(expected, index, count, comparer);

            AssertArraysAreEqual(expected, actual);
        }

        protected static void AssertArraysAreEqual<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual)
        {
            UnityEngine.Assertions.Assert.AreEqual(expected.Count, actual.Count, "The lists are not the same length.");

            for (int i = 0; i < expected.Count; i++)
            {
                UnityEngine.Assertions.Assert.AreEqual(expected[i], actual[i], $"The item at index {i} is not the same.");
            }
        }

        protected static void AssertThrowsReadOnlyException<T>(T obj, Action<T> action) where T : RuntimeScriptableObject, ICanBeReadOnly
        {
            // Arrange
            obj.IsReadOnly = true;

            // Act & Assert
            AssertThrows<ReadOnlyException>(() => action.Invoke(obj));
        }

        protected void AssertPropertyChangesAreInvoked<T>(PropertyChangingEventArgs changingArgs,
            PropertyChangedEventArgs changedArgs,
            Action<T> action,
            T? instance = null) where T : RuntimeScriptableObject
        {
            // Arrange
            if (instance == null)
            {
                instance = CreateInstance<T>();
            }

            // Use lists here because there may be multiple events fired.
            List<PropertyChangingEventArgs> changingEventArgs = new List<PropertyChangingEventArgs>();
            List<PropertyChangedEventArgs> changedEventArgs = new List<PropertyChangedEventArgs>();
#if SCRIPTABLE_VALUES_DO_UI_TOOLKIT
            List<BindablePropertyChangedEventArgs> bindableEventArgs = new List<BindablePropertyChangedEventArgs>();
            long originalHashCode = ((IDataSourceViewHashProvider) instance).GetViewHashCode();
#endif

            ((INotifyPropertyChanged) instance).PropertyChanged += OnPropertyChanged;
            ((INotifyPropertyChanging) instance).PropertyChanging += OnPropertyChanging;

#if SCRIPTABLE_VALUES_DO_UI_TOOLKIT
            ((INotifyBindablePropertyChanged) instance).propertyChanged += OnBindablePropertyChanged;
#endif

            // Act
            action.Invoke(instance);

            // Assert
            Assert.IsTrue(changingEventArgs.Count != 0, "There should be at least one changing event.");
            Assert.IsTrue(changedEventArgs.Count != 0, "There should be at least one changed event.");
            Assert.IsTrue(changingEventArgs.Count == changedEventArgs.Count, "The number of changing and changed events should be the same.");
#if SCRIPTABLE_VALUES_DO_UI_TOOLKIT
            Assert.IsTrue(bindableEventArgs.Count != 0, "There should be at least one bindable event.");
            Assert.IsTrue(changingEventArgs.Count == bindableEventArgs.Count, "The number of changing and bindable events should be the same.");
#endif

            Assert.IsTrue(changingEventArgs.Any(x => x.PropertyName == changingArgs.PropertyName));
            Assert.IsTrue(changedEventArgs.Any(x => x.PropertyName == changedArgs.PropertyName));
#if SCRIPTABLE_VALUES_DO_UI_TOOLKIT
            Assert.IsTrue(bindableEventArgs.Any(x => x.propertyName == changedArgs.PropertyName));
            Assert.AreNotEqual(originalHashCode, ((IDataSourceViewHashProvider) instance).GetViewHashCode(), "Hashcode should've changed between the events.");
#endif

            // Cleanup
            ((INotifyPropertyChanged) instance).PropertyChanged -= OnPropertyChanged;
            ((INotifyPropertyChanging) instance).PropertyChanging -= OnPropertyChanging;
#if SCRIPTABLE_VALUES_DO_UI_TOOLKIT
            ((INotifyBindablePropertyChanged) instance).propertyChanged -= OnBindablePropertyChanged;
#endif

            void OnPropertyChanged(object _, PropertyChangedEventArgs args)
            {
                changedEventArgs.Add(args);
            }

            void OnPropertyChanging(object _, PropertyChangingEventArgs args)
            {
                changingEventArgs.Add(args);
            }

#if SCRIPTABLE_VALUES_DO_UI_TOOLKIT
            void OnBindablePropertyChanged(object _, BindablePropertyChangedEventArgs args)
            {
                bindableEventArgs.Add(args);
            }
#endif
        }

        protected static TestCaseData MakePropertyChangeTestCase<TType>(PropertyChangingEventArgs changingArgs,
            PropertyChangedEventArgs changedArgs,
            Action<TType> setValue,
            string? name = null)
        {
            return new TestCaseData(changingArgs, changedArgs, setValue).SetName(string.IsNullOrEmpty(name) ? changedArgs.PropertyName : name);
        }

        private sealed class ComparisonComparer<T> : IComparer<T>
        {
            private readonly Comparison<T> comparison;

            public ComparisonComparer(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }

            public int Compare(T x, T y)
            {
                return comparison.Invoke(x, y);
            }
        }
    }
}