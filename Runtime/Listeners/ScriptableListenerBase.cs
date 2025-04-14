using System;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
	public abstract class ScriptableListenerBase : MonoBehaviour
	{
		[SerializeField]
		[EditorTooltip("When listeners should start listening.")]
		private StartListenEvents startListening = StartListenEvents.Awake;
		[SerializeField]
		[EditorTooltip("When listeners should stop listening.")]
		private StopListenEvents stopListening = StopListenEvents.OnDestroy;

		/// <summary>
		///     Is the listener currently listening to the target value?
		/// </summary>
		public bool IsListening { get; private set; }

		/// <summary>
		///     When listeners should start listening.
		/// </summary>
		public StartListenEvents StartListening
		{
			get { return startListening; }
			set { startListening = value; }
		}
		/// <summary>
		///     When listeners should stop listening.
		/// </summary>
		public StopListenEvents StopListening
		{
			get { return stopListening; }
			set { stopListening = value; }
		}

		protected virtual void Awake()
		{
			IsListening = false;

			if (startListening == StartListenEvents.Awake)
			{
				SetListening(true);
			}
		}

		protected void Start()
		{
			if (!IsListening && startListening == StartListenEvents.Start)
			{
				SetListening(true);
			}
		}

		protected virtual void OnEnable()
		{
			if (!IsListening && startListening == StartListenEvents.OnEnable)
			{
				SetListening(true);
			}
		}

		protected virtual void OnDisable()
		{
			if (IsListening && stopListening == StopListenEvents.OnDisable)
			{
				SetListening(false);
			}
		}

		protected virtual void OnDestroy()
		{
			if (IsListening && stopListening == StopListenEvents.OnDestroy)
			{
				SetListening(false);
			}
		}

		/// <summary>
		///     Sets the listening state of the object.
		/// </summary>
		/// <param name="listen">If the object should listen to the target value.</param>
		protected virtual void SetListening(bool listen)
		{
			IsListening = listen;
		}

		#region Obsolete
#if UNITY_EDITOR // Don't include in builds.
		[Obsolete("Use 'SetListening' instead. This will be removed in build.", true)]
		protected virtual void ToggleListening(bool listen)
		{
			throw new NotSupportedException(nameof(ToggleListening) + " is obsolete. Use " + nameof(SetListening) + " instead.");
		}
#endif // UNITY_EDITOR
		#endregion
	}
}