using UnityEngine;

namespace Tjgl.SerializedType
{
	/// <summary>
	/// SerializedType is a class that serializes and deserializes <see cref="System.Type"/> values
	/// for the Unity inspector.
	/// </summary>
	/// <typeparam name="T">The type to serialize.</typeparam>
	[System.Serializable]
	public sealed class SerializedType<T> : ISerializationCallbackReceiver
	{
		[SerializeField] private string _assemblyQualifiedName = string.Empty;
		
		/// <summary> The type that is serialized. </summary>
		public System.Type Type { get; private set; }
		
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_assemblyQualifiedName = Type?.AssemblyQualifiedName ?? _assemblyQualifiedName;
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (string.IsNullOrEmpty(_assemblyQualifiedName))
				return;
			
			if (TryGetType(_assemblyQualifiedName, out System.Type type))
				Type = type;
			else
				Debug.LogError($"Failed to deserialize type: {_assemblyQualifiedName}");
		}
		
		private static bool TryGetType(string assemblyQualifiedName, out System.Type type)
		{
			type = null;
			
			if (string.IsNullOrEmpty(assemblyQualifiedName))
				return false;
			
			type = System.Type.GetType(assemblyQualifiedName);
			return type != null;
		}
		
		public static implicit operator System.Type(SerializedType<T> type) => type.Type;
	}
}
