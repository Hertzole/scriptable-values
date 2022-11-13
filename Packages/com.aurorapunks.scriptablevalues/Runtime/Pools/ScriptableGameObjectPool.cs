using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Game Object Pool", menuName = "Aurora Punks/Scriptable Values/Pools/Game Object Pool", order = 1300)]
#endif
	public class ScriptableGameObjectPool : ScriptablePool<GameObject>
	{
		[SerializeField] 
		private GameObject prefab = default;
		
		protected override GameObject CreateObject()
		{
			return Instantiate(prefab);
		}

		protected override void DestroyObject(GameObject item)
		{
			Destroy(item);
		}
	}
}