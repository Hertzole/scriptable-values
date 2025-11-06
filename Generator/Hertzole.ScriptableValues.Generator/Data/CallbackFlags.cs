using System;

namespace Hertzole.ScriptableValues.Generator;

[Flags]
internal enum CallbackFlags : byte
{
    None = 0,
    PreInvoke = 1 << 0,
    PostInvoke = 1 << 1
}