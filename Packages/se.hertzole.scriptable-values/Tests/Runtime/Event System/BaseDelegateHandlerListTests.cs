using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	internal abstract class BaseDelegateHandlerListTests<TDelegate, TAction, TList> : BaseRuntimeTest
		where TDelegate : Delegate
		where TAction : Delegate
		where TList : BaseDelegateHandlerList<TDelegate, TAction>, new()
	{
		private TList list;

		/// <inheritdoc />
		protected override void OnSetup()
		{
			list = new TList();
		}

		/// <inheritdoc />
		protected override void OnTearDown()
		{
			list = null;
		}

		[Test]
		public void ListenerCount_NullList_ReturnsZero()
		{
			// Don't do anything to the list so the internal callbacks list is null.

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(0));
		}

		[Test]
		public void ListenerCount_EmptyList_ReturnsZero()
		{
			// Arrange
			list.callbacks = new List<StructClosure<TAction>>();

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(0));
		}

		[Test]
		public void ListenerCount_OneListener_ReturnsOne()
		{
			// Arrange
			list.AddCallback(CreateDelegate1());

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(1));
		}

		[Test]
		public void AddCallback_NoContext_AddsCallback()
		{
			// Arrange
			TDelegate del = CreateDelegate1();

			// Act
			list.AddCallback(del);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(1));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(1));
			Assert.That(list.callbacks[0].action, Is.EqualTo(del));
		}

		[Test]
		public void AddCallback_NoContext_AlreadyAdded_Throws()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			list.AddCallback(del);

			// Act
			TestDelegate act = () => list.AddCallback(del);

			// Assert
			AssertThrows<ArgumentException>(act);
		}

		[Test]
		public void AddCallback_NoContext_Multiple_AddsCallback()
		{
			// Arrange
			TDelegate del1 = CreateDelegate1();
			TDelegate del2 = CreateDelegate2();

			// Act
			list.AddCallback(del1);
			list.AddCallback(del2);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(2));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(2));
			Assert.That(list.callbacks[0].action, Is.EqualTo(del1));
			Assert.That(list.callbacks[1].action, Is.EqualTo(del2));
		}

		[Test]
		public void AddCallback_Context_AddsCallback()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			object context = new object();

			// Act
			list.AddCallback(del, context);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(1));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(1));
			Assert.That(list.callbacks[0].action, Is.EqualTo(del));
			Assert.That(list.callbacks[0].context, Is.EqualTo(context));
		}

		[Test]
		public void AddCallback_Context_AlreadyAdded_Throws()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			object context = new object();
			list.AddCallback(del, context);

			// Act
			TestDelegate act = () => list.AddCallback(del, context);

			// Assert
			AssertThrows<ArgumentException>(act);
		}

		[Test]
		public void AddCallback_Context_Multiple_AddsCallback()
		{
			// Arrange
			TDelegate del1 = CreateDelegate1();
			TDelegate del2 = CreateDelegate2();
			object context1 = new object();
			object context2 = new object();

			// Act
			list.AddCallback(del1, context1);
			list.AddCallback(del2, context2);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(2));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(2));
			Assert.That(list.callbacks[0].action, Is.EqualTo(del1));
			Assert.That(list.callbacks[0].context, Is.EqualTo(context1));
			Assert.That(list.callbacks[1].action, Is.EqualTo(del2));
			Assert.That(list.callbacks[1].context, Is.EqualTo(context2));
		}

		[Test]
		public void RemoveCallback_NoContext_RemovesCallback()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			list.AddCallback(del);

			// Act
			bool result = list.RemoveCallback(del);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(0));
		}

		[Test]
		public void RemoveCallback_NoContext_AlreadyRemoved_ReturnsFalse()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			list.AddCallback(del);
			list.RemoveCallback(del);

			// Act
			bool result = list.RemoveCallback(del);

			// Assert
			Assert.That(result, Is.False);
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(0));
		}

		[Test]
		public void RemoveCallback_NoContext_Multiple_RemovesCallback()
		{
			// Arrange
			TDelegate del1 = CreateDelegate1();
			TDelegate del2 = CreateDelegate2();
			list.AddCallback(del1);
			list.AddCallback(del2);

			// Act
			bool result = list.RemoveCallback(del1);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(list.ListenersCount, Is.EqualTo(1));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(1));
			Assert.That(list.callbacks[0].action, Is.EqualTo(del2));
		}

		[Test]
		public void RemoveCallback_Context_RemovesCallback()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			object context = new object();
			list.AddCallback(del, context);

			// Act
			bool result = list.RemoveCallback<TDelegate>(del);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(0));
		}

		[Test]
		public void RemoveCallback_Context_AlreadyRemoved_ReturnsFalse()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			object context = new object();
			list.AddCallback(del, context);
			list.RemoveCallback(del);

			// Act
			bool result = list.RemoveCallback<TDelegate>(del);

			// Assert
			Assert.That(result, Is.False);
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(0));
		}

		[Test]
		public void RemoveCallback_Context_Multiple_RemovesCallback()
		{
			// Arrange
			TDelegate del1 = CreateDelegate1();
			TDelegate del2 = CreateDelegate2();
			object context1 = new object();
			object context2 = new object();
			list.AddCallback(del1, context1);
			list.AddCallback(del2, context2);

			// Act
			bool result = list.RemoveCallback<TDelegate>(del1);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(list.ListenersCount, Is.EqualTo(1));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(1));
			Assert.That(list.callbacks[0].action, Is.EqualTo(del2));
		}

		[Test]
		public void Clear_ClearsList()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			list.AddCallback(del);

			// Act
			list.Clear();

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(0));
		}

		[Test]
		public void Clear_NullList_DoesNothing()
		{
			// Act
			list.Clear();

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Null);
		}

		[Test]
		public void AddFrom_AddsFromOtherList()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			TList other = new TList();
			other.AddCallback(del);

			// Act
			list.AddFrom(other);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(1));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(1));
			Assert.That(list.callbacks[0].action, Is.EqualTo(del));
		}

		[Test]
		public void AddFrom_EmptyList_DoesNothing()
		{
			// Arrange
			TList other = new TList();

			// Act
			list.AddFrom(other);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Null);
		}

		[Test]
		public void AddFrom_NullList_Throws()
		{
			// Act
			TestDelegate act = () => list.AddFrom(null!);

			// Assert
			AssertThrows<ArgumentNullException>(act);
		}

		[Test]
		public void AddFrom_AlreadyAdded_Throws()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			list.AddCallback(del);
			TList other = new TList();
			other.AddCallback(del);

			// Act
			TestDelegate act = () => list.AddFrom(other);

			// Assert
			AssertThrows<ArgumentException>(act);
		}

		[Test]
		public void AddFrom_AlreadyInitialized_AddsFromOtherList()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			TDelegate del2 = CreateDelegate2();
			list.AddCallback(del);
			TList other = new TList();
			other.AddCallback(del2);

			// Act
			list.AddFrom(other);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(2));
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(list.callbacks, Has.Count.EqualTo(2));
			Assert.That(list.callbacks[0].action, Is.EqualTo(del));
			Assert.That(list.callbacks[1].action, Is.EqualTo(del2));
		}

		[Test]
		public void RemoveFrom_RemovesFromOtherList()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			list.AddCallback(del);
			TList other = new TList();
			other.AddCallback(del);

			// Act
			list.RemoveFrom(other);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Not.Null);
		}

		[Test]
		public void RemoveFrom_EmptyList_DoesNothing()
		{
			// Arrange
			TList other = new TList();
			other.AddCallback(CreateDelegate1());

			// Act
			list.RemoveFrom(other);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(0));
			Assert.That(list.callbacks, Is.Null);
		}

		[Test]
		public void RemoveFrom_EmptyOtherList_DoesNothing()
		{
			// Arrange
			list.AddCallback(CreateDelegate1());
			TList other = new TList();

			// Act
			list.RemoveFrom(other);

			// Assert
			Assert.That(list.ListenersCount, Is.EqualTo(1));
			Assert.That(list.callbacks, Is.Not.Null);
		}

		[Test]
		public void RemoveFrom_NullList_Throws()
		{
			// Act
			TestDelegate act = () => list.RemoveFrom(null!);

			// Assert
			AssertThrows<ArgumentNullException>(act);
		}

		[Test]
		public void GetDelegates_ReturnsDelegates()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			list.AddCallback(del);

			// Act
			using SpanOwner<Delegate> delegates = ((IDelegateList) list).GetDelegates();

			// Assert
			Assert.That(delegates.Length, Is.EqualTo(1));
			Assert.That(delegates.Span[0], Is.EqualTo(del));
		}

		[Test]
		public void GetDelegates_NullList_ReturnsEmpty()
		{
			// Act
			using SpanOwner<Delegate> delegates = ((IDelegateList) list).GetDelegates();

			// Assert
			Assert.That(list.callbacks, Is.Null);
			Assert.That(delegates.Length, Is.EqualTo(0));
		}

		[Test]
		public void GetDelegates_EmptyList_ReturnsEmpty()
		{
			// Arrange
			list.callbacks = new List<StructClosure<TAction>>();

			// Act
			using SpanOwner<Delegate> delegates = ((IDelegateList) list).GetDelegates();

			// Assert
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(delegates.Length, Is.EqualTo(0));
		}

		[Test]
		public void GetCallbacks_ReturnsCallbacks()
		{
			// Arrange
			TDelegate del = CreateDelegate1();
			list.AddCallback(del);

			// Act
			using SpanOwner<StructClosure<TAction>> callbacks = list.GetCallbacks();

			// Assert
			Assert.That(callbacks.Length, Is.EqualTo(1));
			Assert.That(callbacks.Span[0].action, Is.EqualTo(del));
		}

		[Test]
		public void GetCallbacks_NullList_ReturnsEmpty()
		{
			// Act
			using SpanOwner<StructClosure<TAction>> callbacks = list.GetCallbacks();

			// Assert
			Assert.That(list.callbacks, Is.Null);
			Assert.That(callbacks.Length, Is.EqualTo(0));
		}

		[Test]
		public void GetCallbacks_EmptyList_ReturnsEmpty()
		{
			// Arrange
			list.callbacks = new List<StructClosure<TAction>>();

			// Act
			using SpanOwner<StructClosure<TAction>> callbacks = list.GetCallbacks();

			// Assert
			Assert.That(list.callbacks, Is.Not.Null);
			Assert.That(callbacks.Length, Is.EqualTo(0));
		}

		[Test]
		public void Invoke_InvokesAllCallbacks()
		{
			// Arrange
			bool invoked1 = false;
			InvokeCountContext context = new InvokeCountContext();
			TDelegate del1 = CreateInvokedDelegate(() => invoked1 = true);
			TAction del2 = CreateInvokedDelegateWithContext(o =>
			{
				context.invokeCount++;
				context.AddArg("context", o);
			});

			list.AddCallback(del1);
			list.AddCallback(del2, context);

			// Act
			Invoke(list);

			// Assert
			Assert.That(invoked1, Is.True);
			Assert.That(context.invokeCount, Is.EqualTo(1));
			Assert.That(context.GetArg<object>("context"), Is.EqualTo(context));
		}

		protected abstract void Invoke(TList list);

		protected abstract TDelegate CreateDelegate1();

		protected abstract TDelegate CreateDelegate2();

		protected abstract TDelegate CreateInvokedDelegate(Action callbackToCall);

		protected abstract TAction CreateInvokedDelegateWithContext(Action<object> callbackToCall);
	}
}