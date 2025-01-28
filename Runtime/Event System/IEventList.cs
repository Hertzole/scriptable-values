#nullable enable

using System;

namespace Hertzole.ScriptableValues
{
	internal interface IEventList
	{
		int ListenersCount { get; }

		ReadOnlySpan<Delegate> GetListeners();

		void AddListener<TDelegate>(TDelegate action, object? context = null) where TDelegate : Delegate;

		void RemoveListener<TDelegate>(TDelegate action) where TDelegate : Delegate;
	}
}