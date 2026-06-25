#if !UNITY_6000_4_OR_NEWER
using UnityEngine;

namespace Hertzole.ScriptableValues
{
    internal static class EntityIdExtensions
    {
        public static EntityId GetEntityId(this Object obj)
        {
            return obj.GetEntityId();
            return new EntityId(obj.GetInstanceID());
        }
    }
}
#endif
