using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Tjgl.SerializedType.Editor
{
	/// <summary>
	/// An advanced dropdown component for selecting a <see cref="System.Type"/> within a Unity Editor inspector.
	/// </summary>
	/// <seealso cref="SerializedType{T}"/>
	public sealed class TypesDropdown : AdvancedDropdown
	{
		private readonly IEnumerable<System.Type> _types;
		private readonly SelectedCallback _selectionCallback;
		
		/// <summary> Creates an instance of a TypesDropdown. </summary>
		/// <param name="types">A collection of types to display. </param>
		/// <param name="selectionCallback">A callback triggered when the selected item changes. </param>
		public TypesDropdown(IEnumerable<System.Type> types, SelectedCallback selectionCallback) : base(new AdvancedDropdownState())
		{
			_types = types;
			_selectionCallback = selectionCallback;
		}

		protected override AdvancedDropdownItem BuildRoot()
		{
			var root = new AdvancedDropdownItem("Types");
			var items = new Dictionary<string, (AdvancedDropdownItem item, System.Type type)>();

			// We first need to create the items without assigning them to a parent.
			// They are sorted later to have a correct inheritance hierarchy.
			foreach (var type in _types)
			{
				var item = new AdvancedDropdownItem(type.Name)
				{
					id = type.AssemblyQualifiedName.GetHashCode()
				};
				
				// We use the full name otherwise we could get conflicts on larger trees (like for MonoBehaviour)
				items.Add(type.FullName, (item, type));
				
				// If this item has children, we need to create a child version of itself so that we can select it.
				if (_types.Any(t => t.BaseType == type))
				{
					var selfChild = new AdvancedDropdownItem($"[{type.Name}]")
					{
						id = type.AssemblyQualifiedName.GetHashCode()
					};
					item.AddChild(selfChild);
				}
			}

			// Since we have created all items, we can sort them correctly by assigning them to an existing parent
			foreach (var (item, type) in items.Values)
			{
				AdvancedDropdownItem parent = root;

				if (type.BaseType != null)
				{
					bool hasBase = items.TryGetValue(type.BaseType.FullName ?? "", out var baseType);
					parent = hasBase ? baseType.item : root;
				}
				
				parent.AddChild(item);
			}

			return root;
		}

		protected override void ItemSelected(AdvancedDropdownItem item)
		{
			base.ItemSelected(item);

			var selectedType = _types.FirstOrDefault(t => t.AssemblyQualifiedName.GetHashCode() == item.id);
			if (selectedType != null)
				_selectionCallback(selectedType);
		}

		/// <summary>
		/// A delegate for handling the selection of a <see cref="System.Type"/> in the <see cref="TypesDropdown"/> component.
		/// </summary>
		/// <param name="type">The type selected from the dropdown.</param>
		public delegate void SelectedCallback(System.Type type);
	}
}
