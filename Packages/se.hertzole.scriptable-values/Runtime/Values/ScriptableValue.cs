using UnityEngine;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Base class for a ScriptableValue without a value.
	/// </summary>
	public abstract partial class ScriptableValue : RuntimeScriptableObject
	{
		[SerializeField]
		[EditorTooltip("If read only, the value cannot be changed at runtime.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal bool isReadOnly = false;
		[SerializeField]
		[EditorTooltip("If true, the value will be reset to the default value on play mode start/game boot.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal bool resetValueOnStart = true;
		[SerializeField]
		[EditorTooltip("If true, an equality check will be run before setting the value to make sure the new value is not the same as the old one.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal bool setEqualityCheck = true;

		/// <summary>
		///     If read only, the value cannot be changed at runtime.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public bool IsReadOnly
		{
			get { return isReadOnly; }
			set
			{
				if (isReadOnly != value)
				{
					isReadOnly = value;
					NotifyPropertyChanged();
				}
			}
		}
		/// <summary>
		///     If true, the value will be reset to the default value on play mode start/game boot.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public bool ResetValueOnStart
		{
			get { return resetValueOnStart; }
			set
			{
				if (resetValueOnStart != value)
				{
					resetValueOnStart = value;
					NotifyPropertyChanged();
				}
			}
		}
		/// <summary>
		///     If true, an equality check will be run before setting the value to make sure the new value is not the same as the
		///     old one.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public bool SetEqualityCheck
		{
			get { return setEqualityCheck; }
			set
			{
				if (setEqualityCheck != value)
				{
					setEqualityCheck = value;
					NotifyPropertyChanged();
				}
			}
		}

#if UNITY_EDITOR
		// Used for the CreateAssetMenu attribute order.
		internal const int ORDER = -1000;
#endif
	}
}