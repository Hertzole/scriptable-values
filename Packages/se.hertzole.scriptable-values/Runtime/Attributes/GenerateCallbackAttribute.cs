using System;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public sealed class GenerateCallbackAttribute : Attribute
	{
		/// <summary>
		/// If true, the parameters in the generated methods will be marked as 'in'.
		/// </summary>
		public bool GenerateInParameters { get; set; } = true;
		
		public GenerateCallbackAttribute() : this(CallbackType.PostInvoke) { }

		public GenerateCallbackAttribute(CallbackType type) { }
	}

	public enum CallbackType
	{
		PreInvoke = 0,
		PostInvoke = 1
	}
}