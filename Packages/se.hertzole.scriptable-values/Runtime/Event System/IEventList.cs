using System;

namespace Hertzole.ScriptableValues
{
	internal interface IEventList
	{
		int ListenersCount { get; }
		
		ReadOnlySpan<Delegate> GetListeners();
	}
}