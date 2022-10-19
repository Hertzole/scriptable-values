using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues.Debugging;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using Object = UnityEngine.Object;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class StackTracesTest
	{
		private const int MAX_STACK_TRACE_LENGTH = IStackTraceProvider.MAX_STACK_TRACE_ENTRIES;

		[Test]
		public void MaxStackTraces_ScriptableValues()
		{
			TestStackTraceCount<ScriptableInt>(i =>
			{
				i.Value++;
			});
		}

		[Test]
		public void StackTraceEventInvoked_ScriptableValues_SetValue()
		{
			TestStackTraceEventInvoked<ScriptableInt>(null, i =>
			{
				i.Value = 1;
			});
		}

		[Test]
		public void StackTracesReset_ScriptableValues()
		{
			TestStackTracesWereReset<ScriptableInt>(i =>
			{
				i.Value = 1;
			});
		}
		
		[Test]
		public void MaxStackTraces_ScriptableEvent()
		{
			TestStackTraceCount<ScriptableEvent>(i =>
			{
				i.Invoke(this);
			});
		}

		[Test]
		public void StackTraceEventInvoked_ScriptableEvent_Invoke()
		{
			TestStackTraceEventInvoked<ScriptableEvent>(null, i =>
			{
				i.Invoke(this);
			});
		}
		
		[Test]
		public void StackTracesReset_ScriptableEvent()
		{
			TestStackTracesWereReset<ScriptableEvent>(i =>
			{
				i.Invoke(this);
			});
		}
		
		[Test]
		public void MaxStackTraces_ScriptableGenericEvent()
		{
			TestStackTraceCount<ScriptableIntEvent>(i =>
			{
				i.Invoke(this, 1);
			});
		}

		[Test]
		public void StackTraceEventInvoked_ScriptableGenericEvent_Invoke()
		{
			TestStackTraceEventInvoked<ScriptableIntEvent>(null, i =>
			{
				i.Invoke(this, 1);
			});
		}
		
		[Test]
		public void StackTracesReset_ScriptableGenericEvent()
		{
			TestStackTracesWereReset<ScriptableIntEvent>(i =>
			{
				i.Invoke(this, 1);
			});
		}
		
		[Test]
		public void MaxStackTraces_ScriptableList()
		{
			TestStackTraceCount<TestScriptableList>(i =>
			{
				i.Add(i.Count);
			});
		}

		[Test]
		public void StackTraceEventInvoked_ScriptableList_Add()
		{
			TestStackTraceEventInvoked<TestScriptableList>(null, i =>
			{
				i.Add(1);
			});
		}

		[Test]
		public void StackTraceEventInvoked_ScriptableList_Insert()
		{
			TestStackTraceEventInvoked<TestScriptableList>(null, i =>
			{
				i.Insert(0, 1);
			});
		}
		
		[Test]
		public void StackTraceEventInvoked_ScriptableList_Remove()
		{
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.Remove(3);
			});
		}
		
		[Test]
		public void StackTraceEventInvoked_ScriptableList_RemoveAt()
		{
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.RemoveAt(0);
			});
		}
		
		[Test]
		public void StackTraceEventInvoked_ScriptableList_Clear()
		{
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
			}, i =>
			{
				i.Clear();
			});
		}
		
		[Test]
		public void StackTraceEventInvoked_ScriptableList_SetValue()
		{
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
			}, i =>
			{
				i[0] = 42;
			});
		}
		
		[Test]
		public void StackTraceEventInvoked_ScriptableList_Reverse()
		{
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.Reverse();
			});
			
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.Reverse(0, i.Count);
			});
		}
		
		[Test]
		public void StackTraceEventInvoked_ScriptableList_Sort()
		{
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.Sort();
			});
			
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.Sort(Comparer<int>.Default);
			});
			
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.Sort((x, y) => x.CompareTo(y));
			});
			
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.Sort(0, i.Count, Comparer<int>.Default);
			});
		}
		
		[Test]
		public void StackTraceEventInvoked_ScriptableList_TrimExcess()
		{
			TestStackTraceEventInvoked<TestScriptableList>(i =>
			{
				i.Add(1);
				i.Add(2);
				i.Add(3);
				i.Add(4);
				i.Add(5);
			}, i =>
			{
				i.TrimExcess();
			});
		}
		
		[Test]
		public void StackTracesReset_ScriptableList()
		{
			TestStackTracesWereReset<TestScriptableList>(i =>
			{
				i.Add(0);
			});
		}
		
		private static void TestStackTraceCount<T>(Action<T> action) where T : ScriptableObject, IStackTraceProvider
		{
			T instance = ScriptableObject.CreateInstance<T>();

			IStackTraceProvider provider = instance;

			Assert.IsNotNull(provider);

			Assert.AreEqual(0, provider.Invocations.Count);

			for (int i = 0; i <= MAX_STACK_TRACE_LENGTH + 1; i++)
			{
				action(instance);
			}

			Assert.AreEqual(MAX_STACK_TRACE_LENGTH, provider.Invocations.Count);

			Object.DestroyImmediate(instance);
		}

		private static void TestStackTraceEventInvoked<T>(Action<T> setup, Action<T> invoke) where T : RuntimeScriptableObject, IStackTraceProvider
		{
			T instance = ScriptableObject.CreateInstance<T>();

			bool eventInvoked = false;

			setup?.Invoke(instance);
			
			instance.OnStackTraceAdded += () => { eventInvoked = true; };
		
			invoke.Invoke(instance);

			try
			{

				Assert.IsTrue(eventInvoked, "Stack trace event was not invoked.");
			}
			catch (AssertionException) { }
			finally
			{
				Object.DestroyImmediate(instance);
			}
		}
		
		private static void TestStackTracesWereReset<T>(Action<T> invoke) where T : RuntimeScriptableObject, IStackTraceProvider
		{
			T instance = ScriptableObject.CreateInstance<T>();
		
			invoke.Invoke(instance);

			try
			{
				Assert.IsTrue(instance.Invocations.Count > 0, "No stack traces have been added.");
				instance.ResetValues();
				Assert.AreEqual(0, instance.Invocations.Count, "Stack traces were not cleared.");
			}
			catch (AssertionException) { }
			finally
			{
				Object.DestroyImmediate(instance);
			}
		}

		private class TestScriptableList : ScriptableList<int> { }
	}
}