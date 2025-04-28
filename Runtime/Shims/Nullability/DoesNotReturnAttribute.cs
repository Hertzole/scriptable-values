#if !NETSTANDARD2_1 && !NET5_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ExcludeFromCodeCoverage]
	[DebuggerNonUserCode]
	internal sealed class DoesNotReturnAttribute : Attribute { }
}
#endif