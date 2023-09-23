using Script.UI;
using UnityEditor;
using UnityEditor.UI;

namespace Editor
{
    [CustomEditor(typeof(DTButton))]
    public class DTButtonInspector : ButtonEditor
    {
        private DTButton targetRef;
        private SerializedProperty _minLongTouchTime;
        private SerializedProperty _touchSoundKey;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _minLongTouchTime = serializedObject.FindProperty(nameof(DTButton.minLongClickTime));
            _touchSoundKey = serializedObject.FindProperty(nameof(DTButton.touchSoundKey));
            serializedObject.Update();
            targetRef = (DTButton) target;
            _minLongTouchTime.floatValue = EditorGUILayout.Slider("long touch 위해 필요한 시간 ", _minLongTouchTime.floatValue , 0.1f, 10f);
            serializedObject.ApplyModifiedProperties();
        }
    }
}