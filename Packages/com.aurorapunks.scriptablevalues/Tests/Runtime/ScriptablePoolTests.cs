using System;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using Object = UnityEngine.Object;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class ScriptablePoolTests : BaseRuntimeTest
	{
		// Get

		[Test]
		public void Get_Class()
		{
			Get_Test<TestClassScriptablePool, TestClass>();
		}

		[Test]
		public void Get_GameObject()
		{
			Get_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Get_Component()
		{
			Get_Test<CameraScriptablePool, Camera>();
		}

		[Test]
		public void Get_ScriptableObject()
		{
			Get_Test<TestScriptableObjectPool, PoolableScriptableObject>();
		}

		// Return

		[Test]
		public void Return_Class()
		{
			Return_Test<TestClassScriptablePool, TestClass>();
		}

		[Test]
		public void Return_GameObject()
		{
			Return_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Return_Component()
		{
			Return_Test<CameraScriptablePool, Camera>();
		}

		[Test]
		public void Return_ScriptableObject()
		{
			Return_Test<TestScriptableObjectPool, PoolableScriptableObject>();
		}

		// Return get same back

		[Test]
		public void Get_Class_ReturnSame()
		{
			Get_ReturnSame_Test<TestClassScriptablePool, TestClass>();
		}

		[Test]
		public void Get_GameObject_ReturnSame()
		{
			Get_ReturnSame_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Get_Component_ReturnSame()
		{
			Get_ReturnSame_Test<CameraScriptablePool, Camera>();
		}

		[Test]
		public void Get_ScriptableObject_ReturnSame()
		{
			Get_ReturnSame_Test<TestScriptableObjectPool, PoolableScriptableObject>();
		}

		// Get invokes event

		[Test]
		public void Get_Class_InvokesEvent()
		{
			Get_InvokesEvent_Test<TestClassScriptablePool, TestClass>();
		}

		[Test]
		public void Get_GameObject_InvokesEvent()
		{
			Get_InvokesEvent_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Get_Component_InvokesEvent()
		{
			Get_InvokesEvent_Test<CameraScriptablePool, Camera>();
		}

		[Test]
		public void Get_ScriptableObject_InvokesEvent()
		{
			Get_InvokesEvent_Test<TestScriptableObjectPool, PoolableScriptableObject>();
		}

		// Return invokes event

		[Test]
		public void Return_Class_InvokesEvent()
		{
			Return_InvokesEvent_Test<TestClassScriptablePool, TestClass>();
		}

		[Test]
		public void Return_GameObject_InvokesEvent()
		{
			Return_InvokesEvent_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Return_Component_InvokesEvent()
		{
			Return_InvokesEvent_Test<CameraScriptablePool, Camera>();
		}

		[Test]
		public void Return_ScriptableObject_InvokesEvent()
		{
			Return_InvokesEvent_Test<TestScriptableObjectPool, PoolableScriptableObject>();
		}

		// Create invokes event

		[Test]
		public void Create_Class_InvokesEvent()
		{
			Create_InvokesEvent_Test<TestClassScriptablePool, TestClass>();
		}

		[Test]
		public void Create_GameObject_InvokesEvent()
		{
			Create_InvokesEvent_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Create_Component_InvokesEvent()
		{
			Create_InvokesEvent_Test<CameraScriptablePool, Camera>();
		}

		[Test]
		public void Create_ScriptableObject_InvokesEvent()
		{
			Create_InvokesEvent_Test<TestScriptableObjectPool, PoolableScriptableObject>();
		}

		// Clear

		[Test]
		public void Clear_Class()
		{
			Clear_Test<TestClassScriptablePool, TestClass>();
		}

		[Test]
		public void Clear_GameObject()
		{
			Clear_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Clear_Component()
		{
			Clear_Test<CameraScriptablePool, Camera>();
		}

		[Test]
		public void Clear_ScriptableObject()
		{
			Clear_Test<TestScriptableObjectPool, PoolableScriptableObject>();
		}

		// Get poolable

		[Test]
		public void Get_Class_Poolable()
		{
			Get_Poolable_Test<TestClassScriptablePool, TestClass>(testClass => testClass.IsPooled);
		}

		[Test]
		public void Get_GameObject_Poolable()
		{
			Get_Poolable_Test<ScriptableGameObjectPool, GameObject>(gameObject => gameObject.GetComponent<PoolableScript>().IsPooled);
		}

		[Test]
		public void Get_Component_Poolable()
		{
			Get_Poolable_Test<CameraScriptablePool, Camera>(camera => camera.GetComponent<PoolableScript>().IsPooled);
		}

		[Test]
		public void Get_ScriptableObject_Poolable()
		{
			Get_Poolable_Test<TestScriptableObjectPool, PoolableScriptableObject>(scriptableObject => scriptableObject.IsPooled);
		}

		// Return poolable

		[Test]
		public void Return_Class_Poolable()
		{
			Return_Poolable_Test<TestClassScriptablePool, TestClass>(testClass => testClass.IsPooled);
		}

		[Test]
		public void Return_GameObject_Poolable()
		{
			Return_Poolable_Test<ScriptableGameObjectPool, GameObject>(gameObject => gameObject.GetComponent<PoolableScript>().IsPooled);
		}

		[Test]
		public void Return_Component_Poolable()
		{
			Return_Poolable_Test<CameraScriptablePool, Camera>(camera => camera.GetComponent<PoolableScript>().IsPooled);
		}

		[Test]
		public void Return_ScriptableObject_Poolable()
		{
			Return_Poolable_Test<TestScriptableObjectPool, PoolableScriptableObject>(scriptableObject => scriptableObject.IsPooled);
		}

		// Return null

		[Test]
		public void Return_Class_Null()
		{
			Return_Null_Test<TestClassScriptablePool, TestClass>();
		}

		[Test]
		public void Return_GameObject_Null()
		{
			Return_Null_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Return_Component_Null()
		{
			Return_Null_Test<CameraScriptablePool, Camera>();
		}

		[Test]
		public void Return_ScriptableObject_Null()
		{
			Return_Null_Test<TestScriptableObjectPool, PoolableScriptableObject>();
		}

		// Get object is active

		[Test]
		public void Get_GameObject_IsActive()
		{
			Get_IsActive_Test<ScriptableGameObjectPool, GameObject>(gameObject => gameObject.activeSelf);
		}

		[Test]
		public void Get_Component_IsActive()
		{
			Get_IsActive_Test<CameraScriptablePool, Camera>(camera => camera.gameObject.activeSelf);
		}

		// Return object is inactive

		[Test]
		public void Return_GameObject_IsInactive()
		{
			Return_IsInactive_Test<ScriptableGameObjectPool, GameObject>(gameObject => gameObject.activeSelf);
		}

		[Test]
		public void Return_Component_IsInactive()
		{
			Return_IsInactive_Test<CameraScriptablePool, Camera>(camera => camera.gameObject.activeSelf);
		}

		// Get destroyed objects

		[Test]
		public void Get_DestroyedObjects_GameObject()
		{
			Get_DestroyedObjects_Test<ScriptableGameObjectPool, GameObject>();
		}

		[Test]
		public void Get_DestroyedObjects_Component()
		{
			Get_DestroyedObjects_Test<CameraScriptablePool, Camera>();
		}

		private void Get_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();
			Assert.IsNotNull(instance.Get());
			Assert.AreEqual(1, instance.CountAll);
			Assert.AreEqual(1, instance.CountActive);
			Assert.AreEqual(0, instance.CountInactive);
		}

		private void Return_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();
			TValue value = instance.Get();

			instance.Return(value);
			Assert.AreEqual(1, instance.CountAll);
			Assert.AreEqual(0, instance.CountActive);
			Assert.AreEqual(1, instance.CountInactive);
		}

		private void Get_ReturnSame_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			TValue value = instance.Get();

			Assert.IsNotNull(value);

			Assert.AreEqual(1, instance.CountAll);
			Assert.AreEqual(1, instance.CountActive);
			Assert.AreEqual(0, instance.CountInactive);

			instance.Return(value);

			Assert.AreEqual(1, instance.CountAll);
			Assert.AreEqual(0, instance.CountActive);
			Assert.AreEqual(1, instance.CountInactive);

			TValue value2 = instance.Get();

			Assert.AreEqual(1, instance.CountAll);
			Assert.AreEqual(1, instance.CountActive);
			Assert.AreEqual(0, instance.CountInactive);

			Assert.AreEqual(value, value2);
		}

		private void Get_InvokesEvent_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			bool eventInvoked = false;
			TValue eventObject = null;

			instance.OnGetObject += testObj =>
			{
				eventInvoked = true;
				eventObject = testObj;
			};

			TValue testClass = instance.Get();

			Assert.IsTrue(eventInvoked);
			Assert.AreEqual(testClass, eventObject);
		}

		private void Return_InvokesEvent_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			bool eventInvoked = false;
			TValue eventObject = null;

			instance.OnReturnObject += testObj =>
			{
				eventInvoked = true;
				eventObject = testObj;
			};

			TValue testClass = instance.Get();

			instance.Return(testClass);

			Assert.IsTrue(eventInvoked);
			Assert.AreEqual(testClass, eventObject);
		}

		private void Create_InvokesEvent_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			bool eventInvoked = false;
			TValue eventObject = null;

			instance.OnCreateObject += testObj =>
			{
				eventInvoked = true;
				eventObject = testObj;
			};

			TValue testClass = instance.Get();

			Assert.IsTrue(eventInvoked);
			Assert.AreEqual(testClass, eventObject);

			eventInvoked = false;

			instance.Return(testClass);

			Assert.IsFalse(eventInvoked);

			instance.Get();

			Assert.IsFalse(eventInvoked);
		}

		private void Clear_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			TValue[] testValues = new TValue[10];

			bool destroyEventInvoked = false;
			int destroyObjectIndex = 0;

			instance.OnDestroyObject += destroyedObject =>
			{
				destroyEventInvoked = true;

				Assert.AreEqual(testValues[destroyObjectIndex], destroyedObject);

				destroyObjectIndex--;
			};

			for (int i = 0; i < 10; i++)
			{
				testValues[i] = instance.Get();
			}

			Assert.IsFalse(destroyEventInvoked);
			Assert.AreEqual(10, instance.CountAll);
			Assert.AreEqual(10, instance.CountActive);
			Assert.AreEqual(0, instance.CountInactive);

			for (int i = 0; i < 5; i++)
			{
				instance.Return(testValues[i]);
			}

			Assert.IsFalse(destroyEventInvoked);
			Assert.AreEqual(10, instance.CountAll);
			Assert.AreEqual(5, instance.CountActive);
			Assert.AreEqual(5, instance.CountInactive);

			destroyObjectIndex = 9;

			instance.Clear();

			Assert.IsTrue(destroyEventInvoked);

			Assert.AreEqual(0, instance.CountAll);
			Assert.AreEqual(0, instance.CountActive);
			Assert.AreEqual(0, instance.CountInactive);
		}

		private void Get_Poolable_Test<TType, TValue>(Func<TValue, bool> checkPooledState) where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			TValue value = instance.Get();

			Assert.IsFalse(checkPooledState.Invoke(value));
		}

		private void Return_Poolable_Test<TType, TValue>(Func<TValue, bool> checkPooledState) where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			TValue value = instance.Get();

			instance.Return(value);

			Assert.IsTrue(checkPooledState.Invoke(value));
		}

		private void Return_Null_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			instance.Get();

			Assert.AreEqual(1, instance.CountAll);
			Assert.AreEqual(1, instance.CountActive);
			Assert.AreEqual(0, instance.CountInactive);

			bool createEventInvoked = false;

			instance.OnCreateObject += testObj => { createEventInvoked = true; };

			instance.Return(null);

			Assert.AreEqual(2, instance.CountAll);
			Assert.AreEqual(1, instance.CountActive);
			Assert.AreEqual(1, instance.CountInactive);

			instance.Get();

			Assert.AreEqual(2, instance.CountAll);
			Assert.AreEqual(2, instance.CountActive);
			Assert.AreEqual(0, instance.CountInactive);

			Assert.IsTrue(createEventInvoked);
		}

		private void Get_IsActive_Test<TType, TValue>(Func<TValue, bool> checkActiveState) where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			TValue value = instance.Get();

			Assert.IsTrue(checkActiveState.Invoke(value));

			instance.Return(value);

			Assert.IsFalse(checkActiveState.Invoke(value));

			// Check multiple times as objects always start active.
			instance.Get();

			Assert.IsTrue(checkActiveState.Invoke(value));
		}

		private void Return_IsInactive_Test<TType, TValue>(Func<TValue, bool> checkActiveState) where TType : ScriptablePool<TValue> where TValue : class
		{
			TType instance = CreatePoolInstance<TType>();

			TValue value = instance.Get();

			instance.Return(value);

			Assert.IsFalse(checkActiveState.Invoke(value));

			instance.Get();

			Assert.IsTrue(checkActiveState.Invoke(value));

			instance.Return(value);

			Assert.IsFalse(checkActiveState.Invoke(value));
		}

		private void Get_DestroyedObjects_Test<TType, TValue>() where TType : ScriptablePool<TValue> where TValue : Object
		{
			TType instance = CreatePoolInstance<TType>();

			TValue value = instance.Get();

			Assert.IsNotNull(value);

			Object.DestroyImmediate(value);

			Assert.IsNull(value);

			// Your IDE may say that the code underneath is unreachable, but it is most certainly reachable.

			value = instance.Get();

			Assert.IsNotNull(value);
		}

		private T CreatePoolInstance<T>() where T : ScriptableObject
		{
			if (typeof(T) == typeof(CameraScriptablePool))
			{
				Camera cam = CreateComponent<Camera>();
				CreateComponent<PoolableScript>(cam.gameObject);
				CameraScriptablePool pool = CreateInstance<CameraScriptablePool>();

				pool.Prefab = cam;

				return pool as T;
			}

			if (typeof(T) == typeof(ScriptableGameObjectPool))
			{
				PoolableScript go = CreateComponent<PoolableScript>();
				ScriptableGameObjectPool pool = CreateInstance<ScriptableGameObjectPool>();

				pool.Prefab = go.gameObject;

				return pool as T;
			}

			return CreateInstance<T>();
		}
	}
}