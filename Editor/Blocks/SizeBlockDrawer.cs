#if UNITY_EDITOR

using ExtendedButton.Runtime.Blocks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ExtendedButton.Editor.Blocks
{
    [CustomPropertyDrawer(typeof(SizeBlock), true)]
    public class SizeBlockDrawer : PropertyDrawer
    {
        const string _normalSize = nameof(_normalSize);
        const string _highlightedSize = nameof(_highlightedSize);
        const string _pressedSize = nameof(_pressedSize);
        const string _selectedSize = nameof(_selectedSize);
        const string _disabledSize = nameof(_disabledSize);
        const string _fadeDuration = nameof(_fadeDuration);

        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = rect;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty normalSize = prop.FindPropertyRelative(_normalSize);
            SerializedProperty highlightedSize = prop.FindPropertyRelative(_highlightedSize);
            SerializedProperty pressedSize = prop.FindPropertyRelative(_pressedSize);
            SerializedProperty selectedSize = prop.FindPropertyRelative(_selectedSize);
            SerializedProperty disabledSize = prop.FindPropertyRelative(_disabledSize);
            SerializedProperty fadeDuration = prop.FindPropertyRelative(_fadeDuration);

            EditorGUI.PropertyField(drawRect, normalSize);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, highlightedSize);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, pressedSize);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, selectedSize);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, disabledSize);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, fadeDuration);
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return 6 * EditorGUIUtility.singleLineHeight + 5 * EditorGUIUtility.standardVerticalSpacing;
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            var properties = new[]
            {
                property.FindPropertyRelative(_normalSize),
                property.FindPropertyRelative(_highlightedSize),
                property.FindPropertyRelative(_pressedSize),
                property.FindPropertyRelative(_selectedSize),
                property.FindPropertyRelative(_disabledSize),
                property.FindPropertyRelative(_fadeDuration)
            };

            foreach (var prop in properties)
            {
                var field = new PropertyField(prop);
                container.Add(field);
            }

            return container;
        }
    }
}

#endif