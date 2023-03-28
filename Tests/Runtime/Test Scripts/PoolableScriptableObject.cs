﻿using UnityEngine;

namespace Hertzole.ScriptableValues.Tests
{
	public class PoolableScriptableObject : ScriptableObject, IPoolable
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