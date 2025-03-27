using System;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class GenerateEventCallbackAttribute : Attribute { }
}