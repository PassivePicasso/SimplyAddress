using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.EditorGUI;
using UnityObject = UnityEngine.Object;

namespace PassivePicasso.SimplyAddress
{
    [CustomEditor(typeof(AddressableSkybox), true)]
    public class AddressableSkyboxEditor : Editor
    {
        Editor materialEditor;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var children = serializedObject.GetIterator().GetVisibleChildren();
            var addressProperty = serializedObject.FindProperty(nameof(SimpleAddress.Address));
            var addMat = target as AddressableSkybox;

            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            EditorGUILayout.PropertyField(addressProperty);
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

            using (new DisabledScope(true))
            {
                CustomDrawFoldoutInspector(addMat.material, ref materialEditor);
            }

            serializedObject.ApplyModifiedProperties();
        }

        public void CustomDrawFoldoutInspector(UnityObject target, ref Editor editor)
        {
            if (editor != null && (editor.target != target || target == null))
            {
                UnityObject.DestroyImmediate(editor);
                editor = null;
            }

            if (editor == null && target != null)
            {
                editor = CreateEditor(target);
            }

            if (!(editor == null))
            {
                editor.DrawHeader();
                editor.OnInspectorGUI();
            }
        }
    }
}