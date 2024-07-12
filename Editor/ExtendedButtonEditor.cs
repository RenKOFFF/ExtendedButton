#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ExtendedButton.Editor
{
    [CustomEditor(typeof(Runtime.ExtendedButton), true)]
    [CanEditMultipleObjects]
    public sealed class ExtendedButtonEditor : ButtonEditor
    {
        private SerializedProperty _onClickProperty;
        private SerializedProperty _interactableProperty;
        private SerializedProperty _targetGraphicProperty;

        private SerializedProperty _navigationProperty;
        private GUIContent _visualizeNavigation = EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");
        private static bool _showNavigation = false;
        private static string _showNavigationKey = "SelectableEditor.ShowNavigation";
        
        private SerializedProperty _transitions;

        private SerializedProperty _imageColors;
        private SerializedProperty _imageSizes;
        private SerializedProperty _imageSprites;

        private SerializedProperty _textElement;
        private SerializedProperty _textElementColors;
        private SerializedProperty _textElementSizes;
        
        private readonly AnimBool _editImageColor = new();
        private readonly AnimBool _editImageSize = new();
        private readonly AnimBool _editImageSprite = new();

        private readonly AnimBool _editTextColor = new();
        private readonly AnimBool _editTextSize = new();


        protected override void OnEnable()
        {
            base.OnEnable();
            
            _onClickProperty = serializedObject.FindProperty("m_OnClick");
            _interactableProperty  = serializedObject.FindProperty("m_Interactable");
            _targetGraphicProperty = serializedObject.FindProperty("m_TargetGraphic");
            _navigationProperty    = serializedObject.FindProperty("m_Navigation");

            _transitions = serializedObject.FindProperty(nameof(_transitions));

            _imageColors = serializedObject.FindProperty(nameof(_imageColors));
            _imageSizes = serializedObject.FindProperty(nameof(_imageSizes));
            _imageSprites = serializedObject.FindProperty(nameof(_imageSprites));

            _textElement = serializedObject.FindProperty(nameof(_textElement));
            _textElementColors = serializedObject.FindProperty(nameof(_textElementColors));
            _textElementSizes = serializedObject.FindProperty(nameof(_textElementSizes));
            
            var transitions = GetButtonTransitions(_transitions);

            _editImageColor.value = transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageColor);
            _editImageSize.value = transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageSize);
            _editImageSprite.value = transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageSprite);
            _editTextColor.value = transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.TextColor);
            _editTextSize.value = transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.TextSize);

            _editImageColor.valueChanged.AddListener(Repaint);
            _editImageSize.valueChanged.AddListener(Repaint);
            _editImageSprite.valueChanged.AddListener(Repaint);
            _editTextColor.valueChanged.AddListener(Repaint);
            _editTextSize.valueChanged.AddListener(Repaint);
            
            _showNavigation = EditorPrefs.GetBool(_showNavigationKey);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _editImageColor.valueChanged.RemoveListener(Repaint);
            _editImageSize.valueChanged.RemoveListener(Repaint);
            _editImageSprite.valueChanged.RemoveListener(Repaint);
            _editTextColor.valueChanged.RemoveListener(Repaint);
            _editTextSize.valueChanged.RemoveListener(Repaint);
        }

        private static Runtime.ExtendedButton.ExtendedButtonTransitions GetButtonTransitions(SerializedProperty buttonTransitions)
        {
            return (Runtime.ExtendedButton.ExtendedButtonTransitions)buttonTransitions.enumValueFlag;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_interactableProperty);

            var transitions = GetButtonTransitions(_transitions);
            
            var graphic = _targetGraphicProperty.objectReferenceValue as Graphic;
            if (graphic == null)
                graphic = (target as Runtime.ExtendedButton).GetComponent<Graphic>();

            _editImageColor.target = !_transitions.hasMultipleDifferentValues && transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageColor);
            _editImageSize.target = !_transitions.hasMultipleDifferentValues && transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageSize);
            _editImageSprite.target = !_transitions.hasMultipleDifferentValues && transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageSprite);
            _editTextColor.target = !_transitions.hasMultipleDifferentValues && transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.TextColor);
            _editTextSize.target = !_transitions.hasMultipleDifferentValues && transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.TextSize);

            EditorGUILayout.PropertyField(_transitions);
            EditorGUILayout.Space();
            
            ++EditorGUI.indentLevel;
            {
                DrawImageLabel(transitions, graphic);
                DrawImageColor();
                DrawImageSize();
                DrawImageSprite();
                DrawTextLabel(transitions);
                DrawTextColor();
                DrawTextSize();
            }
            --EditorGUI.indentLevel;
            
            EditorGUILayout.Space();
            
            //DrawNavigation(); strange errors, fix if comment var transitions = GetButtonTransitions(_transitions);
            DrawOnClick();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawImageLabel(Runtime.ExtendedButton.ExtendedButtonTransitions transitions, Graphic graphic)
        {
            if (transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageColor) ||
                transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageSize) ||
                transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.ImageSprite))
            {
                EditorGUILayout.LabelField("Image transitions", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_targetGraphicProperty);
                    
                if (graphic == null) EditorGUILayout.HelpBox("You must have a Graphic target in order to use a image transitions.", MessageType.Warning);
            }
        }

        private void DrawImageColor()
        {
            if (EditorGUILayout.BeginFadeGroup(_editImageColor.faded))
            {
                EditorGUILayout.Space();
                    
                EditorGUILayout.LabelField("Image colors transitions", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_imageColors);
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void DrawImageSize()
        {
            if (EditorGUILayout.BeginFadeGroup(_editImageSize.faded))
            {
                EditorGUILayout.Space();
                    
                EditorGUILayout.LabelField("Image size transitions", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_imageSizes);
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void DrawImageSprite()
        {
            if (EditorGUILayout.BeginFadeGroup(_editImageSprite.faded))
            {
                EditorGUILayout.Space();
                    
                EditorGUILayout.LabelField("Image sprites transitions", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_imageSprites);
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void DrawTextLabel(Runtime.ExtendedButton.ExtendedButtonTransitions transitions)
        {
            if (transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.TextSize) ||
                transitions.HasFlag(Runtime.ExtendedButton.ExtendedButtonTransitions.TextColor))
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Text transitions", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_textElement);
            }
        }

        private void DrawTextColor()
        {
            if (EditorGUILayout.BeginFadeGroup(_editTextColor.faded))
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Text colors transitions", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_textElementColors);
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void DrawTextSize()
        {
            if (EditorGUILayout.BeginFadeGroup(_editTextSize.faded))
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Text sizes transitions", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_textElementSizes);
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void DrawNavigation()
        {
            EditorGUILayout.PropertyField(_navigationProperty);

            EditorGUI.BeginChangeCheck();
            Rect toggleRect = EditorGUILayout.GetControlRect();
            toggleRect.xMin += EditorGUIUtility.labelWidth;
            _showNavigation = GUI.Toggle(toggleRect, _showNavigation, _visualizeNavigation, EditorStyles.miniButton);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(_showNavigationKey, _showNavigation);
                SceneView.RepaintAll();
            }
        }

        private void DrawOnClick()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_onClickProperty);
        }
    }
}

#endif