#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING && FISHNET
using System.Runtime.CompilerServices;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;

namespace Hertzole.ScriptableValues
{
	partial class SyncedScriptableValue<T> : SyncBase, ICustomSync
	{
		private bool valueChanged;

		private T currentValue;

		partial void OnValueChanging(T previousValue, T newValue)
		{
			if (!IsRegistered)
			{
				return;
			}

			if (!NetworkBehaviour.IsServer)
			{
				return;
			}

			valueChanged = true;
			currentValue = newValue;

			Dirty();
		}

		public override void WriteFull(PooledWriter writer)
		{
			if (!isInitialized || !valueChanged)
			{
				return;
			}

			WriteHeader(writer, false);

			writer.Write(currentValue);
		}

		public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			if (!isInitialized)
			{
				return;
			}
            
			base.WriteDelta(writer, resetSyncTick);

			writer.Write(currentValue);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void Read(PooledReader reader, bool asServer)
		{
			if (!isInitialized)
			{
				return;
			}
			
			bool asClientAndHost = !asServer && NetworkManager.IsServer;

			T newValue = reader.Read<T>();

			if (!asClientAndHost)
			{
				targetValue.Value = newValue;
			}
		}

		public override void Reset()
		{
			base.Reset();
			valueChanged = false;
		}

		public object GetSerializedType()
		{
			return null;
		}

		public void Dispose()
		{
			DisposeScriptableValue();
		}
	}
}
#endif