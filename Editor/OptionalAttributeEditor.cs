#if UNITY_EDITOR
using Attributes;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(OptionalAttribute))]
public class OptionalAttributeEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Draw the default property field
        
        label.text += " (Optional)";
        EditorGUI.PropertyField(position, property, label);
        
        // Check if the property represents a reference type (i.e., a class or interface)
        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            // Retrieve the object value of the property
            UnityEngine.Object objValue = property.objectReferenceValue;

            // Check if the object value is null
            if (objValue == null)
            {
                // Calculate the position for the "Optional" label
                Rect labelPosition = new Rect(position.xMax - 75f, position.y, 75f, position.height);

                // Draw the "Optional" label
                EditorGUI.LabelField(labelPosition, "Optional", EditorStyles.miniLabel);
            }
        }
       
    }
}
#endif