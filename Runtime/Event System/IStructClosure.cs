using System;

namespace Hertzole.ScriptableValues
{
	public interface IStructClosure
	{
		Delegate GetAction();
	}
}