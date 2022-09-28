﻿using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable SByte Event Listener", 1102)]
#endif
	public sealed class ScriptableSByteEventListener : ScriptableEventListener<sbyte> { }
}