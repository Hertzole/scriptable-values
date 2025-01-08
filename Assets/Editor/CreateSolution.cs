#if UNITY_EDITOR
using Unity.CodeEditor;
using UnityEditor;

namespace GitTools
{
	public static class Solution
	{
		[MenuItem("Git Tools/Manual Sync")]
		public static void Sync()
		{
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			CodeEditor.CurrentEditor.SyncAll();
		}
	}
}
#endif