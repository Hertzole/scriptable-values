#if !NETSTANDARD2_1 && !NET5_0_OR_GREATER
using System;
using System.Diagnostics;

namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue)]
	[ExcludeFromCodeCoverage]
	[DebuggerNonUserCode]
	internal sealed class NotNullAttribute : Attribute { }
}
#endif