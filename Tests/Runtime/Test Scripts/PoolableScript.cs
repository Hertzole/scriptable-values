using UnityEngine;

namespace Hertzole.ScriptableValues.Tests
{
    public class PoolableScript : MonoBehaviour, IPoolable
    {
        public bool IsPooled { get; set; }

        public void OnUnpooled()
        {
            IsPooled = false;
        }

        public void OnPooled()
        {
            IsPooled = true;
        }
    }
}