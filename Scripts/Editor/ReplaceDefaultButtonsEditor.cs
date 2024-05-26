using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ExtendedButton.Scripts.Editor
{
    public class ReplaceDefaultButtonsEditor : UnityEditor.Editor
    {
        private static MonoScript _script;

        [MenuItem("CONTEXT/Button/Replace to ExtendedButton", false)]
        private static void ReplaceButtonAsExtended(MenuCommand command)
        {
            var button = (Button)command.context;
            Debug.Log($"The button named {button.name} was replaced to " +
                      $"{nameof(ExtendedButton)} from Context Menu.");

            if (!_script)
            {
                var tempObject = new GameObject("TempObject");
                var instanceScript = tempObject.AddComponent<ExtendedButton>();
                _script = MonoScript.FromMonoBehaviour(instanceScript);
                DestroyImmediate(tempObject);
            }

            var go = ((Component)command.context).gameObject;
            Undo.RegisterCompleteObjectUndo(go, $"Replace Button as {nameof(ExtendedButton)} in {go.name}");
            
            var so = new SerializedObject(command.context);
            var scriptProperty = so.FindProperty("m_Script");
            so.Update();
            scriptProperty.objectReferenceValue = _script;
            so.ApplyModifiedProperties();
        }
        
        [MenuItem("CONTEXT/Button/Replace to ExtendedButton", true)]
        public static bool ReplaceButtonAsExtendedValidate()
        {
            return Selection.activeGameObject.TryGetComponent(out Button button) && button != null && button.GetType() == typeof(Button);
        }
    }
}