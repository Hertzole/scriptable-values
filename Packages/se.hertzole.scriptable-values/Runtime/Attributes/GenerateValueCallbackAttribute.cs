#if UNITY_2022_3_OR_NEWER // Only 2022.3 and newer versions support incremental generators.
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
#endif