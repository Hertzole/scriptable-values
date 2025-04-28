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
		void AddCallback(TDelegate callback);

		void AddCallback<TContextDelegate, TContext>(TContextDelegate callback, TContext context) where TContextDelegate : Delegate;

		bool RemoveCallback(TDelegate callback);

		bool RemoveCallback<TContextDelegate>(TContextDelegate callback) where TContextDelegate : Delegate;
	}
}