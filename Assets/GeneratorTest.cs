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
		public ScriptableBoolEvent eventTest;

		[GenerateCollectionCallback]
		public ScriptableList<string> listTest;

		[GenerateCollectionCallback]
		public ScriptableDictionary<int, string> dicTest;

		[GeneratePoolCallback]
		public ScriptableObjectPool<ScriptableFloat> camerapool;

		[GenerateValueCallback]
		public ValueReference<bool> FloatReference;
		
		[GenerateValueCallback(ValueCallbackType.Changing)]
		public ScriptableString ValueProperty { get; set; }

		private partial void OnFloatReferenceChanged(bool oldValue, bool newValue)
		{
			throw new NotImplementedException();
		}

		private partial void OnValuePropertyChanging(string oldValue, string newValue)
		{
			throw new NotImplementedException();
		}

		private partial void OnEventTestInvoked(object sender, bool args)
		{
			throw new NotImplementedException();
		}

		private partial void OnListTestChanged(CollectionChangedArgs<string> args)
		{
			throw new NotImplementedException();
		}

		private partial void OnDicTestChanged(CollectionChangedArgs<KeyValuePair<int, string>> args)
		{
			throw new NotImplementedException();
		}

		private partial void OnCamerapoolChanged(PoolAction action, ScriptableFloat item)
		{
			throw new NotImplementedException();
		}
	}
}
#endif