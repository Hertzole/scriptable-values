#if !UNITY_6000_4_OR_NEWER
using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
    [StructLayout(LayoutKind.Auto)]
    internal readonly struct EntityId : IEquatable<EntityId>
    {
        private readonly int data;

        public EntityId(int data)
        {
            this.data = data;
        }

        /// <inheritdoc />
        public bool Equals(EntityId other)
        {
            return data == other.data;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is EntityId other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return data;
        }

        public static bool operator ==(EntityId left, EntityId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EntityId left, EntityId right)
        {
            return !left.Equals(right);
        }
    }
}
#endif