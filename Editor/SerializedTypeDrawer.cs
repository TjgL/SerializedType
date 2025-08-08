using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tjgl.SerializedType.Editor
{
	[CustomPropertyDrawer(typeof(SerializedType<>))]
	internal sealed class SerializedTypeDrawer : PropertyDrawer
	{
		private System.Type[] _types;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			FindTypes();
			
			var typeIdProperty = property.FindPropertyRelative("_assemblyQualifiedName");

			if (string.IsNullOrEmpty(typeIdProperty.stringValue))
			{
				typeIdProperty.stringValue = _types.First().AssemblyQualifiedName;
				property.serializedObject.ApplyModifiedProperties();
			}
			
			var dropdown =  new TypesDropdown(_types, selectedType =>
			{
				typeIdProperty.stringValue = selectedType.AssemblyQualifiedName;
				property.serializedObject.ApplyModifiedProperties();
			});

			Rect controlRect = EditorGUI.PrefixLabel(position, label);
			if (GUI.Button(controlRect, System.Type.GetType(typeIdProperty.stringValue)?.Name, EditorStyles.popup))
			{
				dropdown.Show(new Rect(position.x, position.y, position.width, position.height));
			}
		}

		private void FindTypes()
		{
			var genericType = fieldInfo.FieldType.GetGenericArguments()[0];
			var types = TypeCache.GetTypesDerivedFrom(genericType).ToList();
			if (!genericType.IsAbstract && !genericType.IsInterface)
				types.Add(genericType);

			_types = types.ToArray();
		}
	}
}
