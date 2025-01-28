using System;

namespace Hertzole.ScriptableValues
{
	public delegate void EventHandlerWithContext<in TContext>(object sender, EventArgs e, TContext context);
	
	public delegate void EventHandlerWithContext<in TEventArgs, in TContext>(object sender, TEventArgs e, TContext context);
}