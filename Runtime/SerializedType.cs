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

		/// <summary> Creates a new instance of SerializedType from the specified generic type parameter. </summary>
		/// <typeparam name="T2">
		/// The type to assign to the SerializedType.
		/// It must be assignable to the generic parameter T. </typeparam>
		/// <returns>The SerializedType initialized with the correct Type.</returns>
		public static SerializedType<T> FromType<T2>() where T2 : T => new()
		{
			Type = typeof(T2),
			_assemblyQualifiedName = typeof(T2).AssemblyQualifiedName
		};

		public static implicit operator System.Type(SerializedType<T> type) => type.Type;

		// Disabled by default since this doesn't causes a compile time error, only a runtime exception.
#if ST_ALLOW_IMPLICIT_CASTS
		public static implicit operator SerializedType<T>(System.Type type)
		{
			if (type != null && !typeof(T).IsAssignableFrom(type))
				throw new System.ArgumentException($"Type {type} is not assignable to {typeof(T)}");
			
			return new SerializedType<T>
			{
				Type = type,
				_assemblyQualifiedName = type.AssemblyQualifiedName
			};
		}
#endif
	}
}
