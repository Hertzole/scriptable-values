#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
    [TestFixture(typeof(TestClassScriptablePool), typeof(TestClass))]
    [TestFixture(typeof(ScriptableGameObjectPool), typeof(GameObject))]
    [TestFixture(typeof(CameraScriptablePool), typeof(Camera))]
    [TestFixture(typeof(TestScriptableObjectPool), typeof(PoolableScriptableObject))]
    public partial class ScriptablePoolTests<TType, TValue> : BaseRuntimeTest where TType : ScriptablePool<TValue> where TValue : class
    {
        private TType pool = null!;

        /// <inheritdoc />
        protected override void OnSetup()
        {
            pool = CreatePoolInstance<TType>();
        }

        /// <inheritdoc />
        protected override void OnTearDown()
        {
            pool.Clear();
        }

        private T CreatePoolInstance<T>() where T : ScriptablePool
        {
            if (typeof(T) == typeof(CameraScriptablePool))
            {
                Camera cam = CreateComponent<Camera>();
                CreateComponent<PoolableScript>(cam.gameObject);
                CameraScriptablePool pool = CreateInstance<CameraScriptablePool>();

                pool.Prefab = cam;

                return UnsafeUtility.As<CameraScriptablePool, T>(ref pool);
            }

            if (typeof(T) == typeof(ScriptableGameObjectPool))
            {
                PoolableScript go = CreateComponent<PoolableScript>();
                ScriptableGameObjectPool pool = CreateInstance<ScriptableGameObjectPool>();

                pool.Prefab = go.gameObject;

                return UnsafeUtility.As<ScriptableGameObjectPool, T>(ref pool);
            }

            return CreateInstance<T>();
        }

        private static bool IsType<T>()
        {
            return typeof(TValue) == typeof(T);
        }

        private static bool TryGetType<T>(TValue obj,
#if NETSTANDARD2_1
            [NotNullWhen(true)]
#endif
            out T? value) where T : class
        {
            if (typeof(TValue) == typeof(T))
            {
                value = UnsafeUtility.As<TValue, T>(ref obj);
                Assert.IsNotNull(value);
                return true;
            }

            value = null;
            return false;
        }

        private static bool GetIsActive(TValue obj)
        {
            if (TryGetType(obj, out GameObject? go))
            {
                return go.activeSelf;
            }

            if (TryGetType(obj, out Camera? camera))
            {
                return camera.gameObject.activeSelf;
            }

            throw new InvalidOperationException($"Cannot get active state of {typeof(TValue)}.");
        }

        private static bool GetIsPooled(TValue value)
        {
            if (TryGetType(value, out TestClass? testClass))
            {
                return testClass.IsPooled;
            }

            if (TryGetType(value, out PoolableScriptableObject? scriptableObject))
            {
                return scriptableObject.IsPooled;
            }

            if (TryGetType(value, out GameObject? gameObject))
            {
                return gameObject.GetComponent<PoolableScript>().IsPooled;
            }

            if (TryGetType(value, out Camera? camera))
            {
                return camera.GetComponent<PoolableScript>().IsPooled;
            }

            throw new InvalidOperationException($"Cannot get pooled state of {typeof(TValue)}.");
        }
    }
}