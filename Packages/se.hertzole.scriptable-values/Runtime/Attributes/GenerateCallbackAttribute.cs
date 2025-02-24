using System;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public sealed class GenerateCallbackAttribute : Attribute
	{
		public GenerateCallbackAttribute() : this(CallbackType.PostInvoke) { }

		public GenerateCallbackAttribute(CallbackType type) { }
	}
	
	public enum CallbackType
	{
		PreInvoke = 0,
		PostInvoke = 1
	}
}