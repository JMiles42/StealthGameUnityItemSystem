using ForestOfChaosLib.Editor;
using ForestOfChaosLib.Editor.PropertyDrawers;
using ForestOfChaosLib.Extensions;
using ForestOfChaosLib.Utilities;
using UnityEditor;
using UnityEngine;

namespace JMiles42.ItemSystem
{
	[CustomPropertyDrawer(typeof(ItemStack))]
	public class ItemStackPropertyDrawer: ObjectReferenceDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using(var cc = FoCsEditor.Disposables.ChangeCheck())
			{
				var itemProp   = property.FindPropertyRelative("item");
				var amountProp = property.FindPropertyRelative("amount");

				using(var hScope = FoCsEditor.Disposables.RectHorizontalScope(4, position.Edit(RectEdit.SetHeight(SingleLine))))
				{
					using(var propScope = FoCsEditor.Disposables.PropertyScope(position, label, property))
					{
						if(itemProp.objectReferenceValue == null)
						{
							FoCsGUI.Label(hScope.GetNext(), propScope.content);
						}
						else
						{
							FoCsGUI.Label(hScope.GetNext(RectEdit.ChangeX(16)), propScope.content);
						}

						FoCsGUI.PropertyField(hScope.GetNext(2), itemProp);
						FoCsGUI.PropertyField(hScope.GetNext(),  amountProp);
					}
				}

				if(cc.changed)
					serializedObject = null;

				if(itemProp.objectReferenceValue == null)
					return;

				if(serializedObject == null)
					serializedObject = new SerializedObject(itemProp.objectReferenceValue);
			}

			foldout = DrawReference(position, serializedObject, foldout);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var itemProp = property.FindPropertyRelative("item");

			if(itemProp.objectReferenceValue == null)
				return SingleLine;

			if(serializedObject == null)
				serializedObject = new SerializedObject(itemProp.objectReferenceValue);

			if(foldout)
				return PropertyHeight(serializedObject, foldout);

			return SingleLine;
		}
	}
}