using System;

namespace Hertzole.ScriptableValues.Tests
{
	internal class DelegateHandlerList1Tests : BaseDelegateHandlerListTests<Action<int>, Action<int, object>, DelegateHandlerList<Action<int>, int>>
	{
		protected override void Invoke(DelegateHandlerList<Action<int>, int> list)
		{
			list.Invoke(GetRandomNumber());
		}

		protected override Action<int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int> CreateInvokedDelegate(Action callbackToCall)
		{
			return i => { callbackToCall.Invoke(); };
		}

		protected override Action<int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj) { }

		private static void TargetDelegate2(int obj) { }
	}

	internal class DelegateHandlerList2Tests : BaseDelegateHandlerListTests<Action<int, int>, Action<int, int, object>,
		DelegateHandlerList<Action<int, int>, int, int>>
	{
		protected override void Invoke(DelegateHandlerList<Action<int, int>, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i, j, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2) { }

		private static void TargetDelegate2(int obj1, int obj2) { }
	}

	internal class DelegateHandlerList3Tests : BaseDelegateHandlerListTests<Action<int, int, int>, Action<int, int, int, object>,
		DelegateHandlerList<Action<int, int, int>, int, int, int>>
	{
		protected override void Invoke(DelegateHandlerList<Action<int, int, int>, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i, j, k, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3) { }
	}

	internal class DelegateHandlerList4Tests : BaseDelegateHandlerListTests<Action<int, int, int, int>, Action<int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int>, int, int, int, int>>
	{
		protected override void Invoke(DelegateHandlerList<Action<int, int, int, int>, int, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i, j, k, l, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3, int obj4) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3, int obj4) { }
	}

	internal class DelegateHandlerList5Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int>, Action<int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int>, int, int, int, int, int>>
	{
		protected override void Invoke(DelegateHandlerList<Action<int, int, int, int, int>, int, int, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i, j, k, l, m, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3, int obj4, int obj5) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3, int obj4, int obj5) { }
	}

	internal class DelegateHandlerList6Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int, int>, Action<int, int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int, int>, int, int, int, int, int, int>>
	{
		protected override void Invoke(DelegateHandlerList<Action<int, int, int, int, int, int>, int, int, int, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m, n) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i, j, k, l, m, n, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6) { }
	}

	internal class DelegateHandlerList7Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int, int, int>,
		Action<int, int, int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int, int, int>, int, int, int, int, int, int, int>>
	{
		protected override void Invoke(DelegateHandlerList<Action<int, int, int, int, int, int, int>, int, int, int, int, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m, n, o) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i1, i2, i3, i4, i5, i6, i7, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7) { }
	}

	internal class DelegateHandlerList8Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int, int, int, int>,
		Action<int, int, int, int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int>>
	{
		protected override void Invoke(DelegateHandlerList<Action<int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(),
				GetRandomNumber());
		}

		protected override Action<int, int, int, int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m, n, o, p) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i1, i2, i3, i4, i5, i6, i7, i8, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7, int obj8) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7, int obj8) { }
	}

	internal class DelegateHandlerList9Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int, int, int, int, int>,
		Action<int, int, int, int, int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int>>
	{
		protected override void Invoke(
			DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(),
				GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int, int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m, n, o, p, q) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, int, int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i1, i2, i3, i4, i5, i6, i7, i8, i9, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7, int obj8, int obj9) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7, int obj8, int obj9) { }
	}

	internal class DelegateHandlerList10Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int, int, int, int, int, int>,
		Action<int, int, int, int, int, int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int>>
	{
		protected override void Invoke(
			DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(),
				GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m, n, o, p, q, r) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7, int obj8, int obj9, int obj10) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7, int obj8, int obj9, int obj10) { }
	}

	internal class DelegateHandlerList11Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int, int, int, int, int, int, int>,
		Action<int, int, int, int, int, int, int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int>>
	{
		protected override void Invoke(
			DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(),
				GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m, n, o, p, q, r, s) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, object> CreateInvokedDelegateWithContext(Action<object> callbackToCall)
		{
			return (i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7, int obj8, int obj9, int obj10, int obj11) { }

		private static void TargetDelegate2(int obj1, int obj2, int obj3, int obj4, int obj5, int obj6, int obj7, int obj8, int obj9, int obj10, int obj11) { }
	}

	internal class DelegateHandlerList12Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int, int, int, int, int, int, int, int>,
		Action<int, int, int, int, int, int, int, int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int>>
	{
		protected override void Invoke(
			DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int>
				list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(),
				GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m, n, o, p, q, r, s, t) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, int, object> CreateInvokedDelegateWithContext(
			Action<object> callbackToCall)
		{
			return (i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1,
			int obj2,
			int obj3,
			int obj4,
			int obj5,
			int obj6,
			int obj7,
			int obj8,
			int obj9,
			int obj10,
			int obj11,
			int obj12) { }

		private static void TargetDelegate2(int obj1,
			int obj2,
			int obj3,
			int obj4,
			int obj5,
			int obj6,
			int obj7,
			int obj8,
			int obj9,
			int obj10,
			int obj11,
			int obj12) { }
	}

	internal class DelegateHandlerList13Tests : BaseDelegateHandlerListTests<Action<int, int, int, int, int, int, int, int, int, int, int, int, int>,
		Action<int, int, int, int, int, int, int, int, int, int, int, int, int, object>,
		DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int,
			int>>
	{
		protected override void Invoke(
			DelegateHandlerList<Action<int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int,
				int, int> list)
		{
			list.Invoke(GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(),
				GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber());
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, int, int> CreateDelegate1()
		{
			return TargetDelegate;
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, int, int> CreateDelegate2()
		{
			return TargetDelegate2;
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, int, int> CreateInvokedDelegate(Action callbackToCall)
		{
			return (i, j, k, l, m, n, o, p, q, r, s, t, u) => { callbackToCall.Invoke(); };
		}

		protected override Action<int, int, int, int, int, int, int, int, int, int, int, int, int, object> CreateInvokedDelegateWithContext(
			Action<object> callbackToCall)
		{
			return (i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, i13, context) => { callbackToCall.Invoke(context); };
		}

		private static void TargetDelegate(int obj1,
			int obj2,
			int obj3,
			int obj4,
			int obj5,
			int obj6,
			int obj7,
			int obj8,
			int obj9,
			int obj10,
			int obj11,
			int obj12,
			int obj13) { }

		private static void TargetDelegate2(int obj1,
			int obj2,
			int obj3,
			int obj4,
			int obj5,
			int obj6,
			int obj7,
			int obj8,
			int obj9,
			int obj10,
			int obj11,
			int obj12,
			int obj13) { }
	}
}