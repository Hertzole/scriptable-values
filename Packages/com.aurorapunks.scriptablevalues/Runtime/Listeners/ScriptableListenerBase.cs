using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableListenerBase : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("When listeners should start listening.")]
		private StartListenEvents startListening = StartListenEvents.Awake;
		[SerializeField]
		[Tooltip("When listeners should stop listening.")]
		private StopListenEvents stopListening = StopListenEvents.OnDestroy;
		
		/// <summary>
		///     Is the listener currently listening to the target value?
		/// </summary>
		public bool IsListening { get; private set; }
		
		/// <summary>
		///     When listeners should start listening.
		/// </summary>
		public StartListenEvents StartListening { get { return startListening; } set { startListening = value; } }
		/// <summary>
		///     When listeners should stop listening.
		/// </summary>
		public StopListenEvents StopListening { get { return stopListening; } set { stopListening = value; } }
		
		protected virtual void Awake()
		{
			IsListening = false;

			if (!IsListening && startListening == StartListenEvents.Awake)
			{
				ToggleListening(true);
			}
		}

		protected void Start()
		{
			if (!IsListening && startListening == StartListenEvents.Start)
			{
				ToggleListening(true);
			}
		}

		protected virtual void OnEnable()
		{
			if (!IsListening && startListening == StartListenEvents.OnEnable)
			{
				ToggleListening(true);
			}
		}

		protected virtual void OnDisable()
		{
			if (IsListening && stopListening == StopListenEvents.OnDisable)
			{
				ToggleListening(false);
			}
		}

		protected virtual void OnDestroy()
		{
			if (IsListening && stopListening == StopListenEvents.OnDestroy)
			{
				ToggleListening(false);
			}
		}
		
		/// <summary>
		///     Toggles the listener on or off.
		/// </summary>
		/// <param name="listen">If the object should listen to the target value.</param>
		protected virtual void ToggleListening(bool listen)
		{
			IsListening = listen;
		}
	}
}