using System;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public sealed class GenerateValueCallbackAttribute : Attribute
	{
		public GenerateValueCallbackAttribute() : this(ValueCallbackType.Changed) { }

		public GenerateValueCallbackAttribute(ValueCallbackType type) { }
	}

	public enum ValueCallbackType
	{
		Changing = 0,
		Changed = 1
	}
}