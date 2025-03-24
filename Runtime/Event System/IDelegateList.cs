using System;

namespace Hertzole.ScriptableValues
{
	internal interface IDelegateList
	{
		int ListenersCount { get; }

		SpanOwner<Delegate> GetDelegates();
	}

	internal interface IDelegateList<in TDelegate> : IDelegateList where TDelegate : Delegate
	{
		void RegisterCallback(TDelegate callback);

		void RegisterCallback<TContextDelegate, TContext>(TContextDelegate callback, TContext context) where TContextDelegate : Delegate;

		void RemoveCallback(TDelegate callback);

		void RemoveCallback<TContextDelegate>(TContextDelegate callback) where TContextDelegate : Delegate;
	}
}