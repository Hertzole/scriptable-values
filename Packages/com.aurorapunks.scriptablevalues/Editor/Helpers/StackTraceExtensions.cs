using System.Diagnostics;

namespace AuroraPunks.ScriptableValues.Editor
{
	internal static class StackTraceExtensions
	{
		public static StackFrame GetSecondOrBestFrame(this StackTrace trace)
		{
			return trace.GetFrame(trace.FrameCount > 1 ? 1 : 0);
		}
	}
}