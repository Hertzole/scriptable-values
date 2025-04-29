#if UNITY_2022_3_OR_NEWER
using Hertzole.ScriptableValues;
using UnityEngine;

namespace My.Namespace
{
	[GenerateScriptableCallbacks]
	public partial class ChangingClass
	{
		[GenerateValueCallback(ValueCallbackType.Changing)]
		public ScriptableString ValueProperty { get; set; }

        private partial void OnValuePropertyChanging(string oldValue, string newValue)
        {
            throw new System.NotImplementedException();
        }

        [GenerateEventCallback]
		public ScriptableBoolEvent eventTest;

        private partial void OnEventTestInvoked(object sender, bool args)
        {
            throw new System.NotImplementedException();
        }

        [GenerateCollectionCallback]
		public ScriptableList<string> listTest;

        private partial void OnListTestChanged(CollectionChangedArgs<string> args)
        {
            throw new System.NotImplementedException();
        }

        [GenerateCollectionCallback]
		public ScriptableDictionary<int, string> dicTest;

        private partial void OnDicTestChanged(CollectionChangedArgs<System.Collections.Generic.KeyValuePair<int, string>> args)
        {
            throw new System.NotImplementedException();
        }

        [GeneratePoolCallback]
		public ScriptableObjectPool<ScriptableFloat> camerapool;

        private partial void OnCamerapoolChanged(PoolAction action, ScriptableFloat item)
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif