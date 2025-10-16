#if UNITY_2022_3_OR_NEWER
using System;
using System.Collections.Generic;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace My.Namespace
{
	[GenerateScriptableCallbacks]
	public partial class ChangingClass
	{
		[GenerateEventCallback]
		public ScriptableBoolEvent onBoolChanged;
		[GenerateEventCallback]
		public ScriptableFloatEvent m_OnFloatEvent;
		[GenerateValueCallback]
		public ScriptableInt onionHealth;

        private partial void OnOnionHealthChanged(int oldValue, int newValue)
        {
            throw new System.NotImplementedException();
        }

        private partial void OnionHealthChanged(int oldValue, int newValue)
        {
            throw new System.NotImplementedException();
        }

        private partial void OnFloatEventInvoked(object sender, float args)
        {
            throw new System.NotImplementedException();
        }

        private partial void OnBoolChangedInvoked(object sender, bool args)
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif