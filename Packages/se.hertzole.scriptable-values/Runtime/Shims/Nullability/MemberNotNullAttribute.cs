#if !NET5_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    [DebuggerNonUserCode]
    internal sealed class MemberNotNullAttribute : Attribute
    {
        public string[] Members { get; }

        public MemberNotNullAttribute(string member)
        {
            Members = new[] { member };
        }

        public MemberNotNullAttribute(params string[] members)
        {
            Members = members;
        }
    }
}
#endif