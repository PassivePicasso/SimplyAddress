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
    [CustomEditor(typeof(AddressableMaterial), true)]
    public class AddressableMaterialEditor : Editor
    {
        Editor materialEditor;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var children = serializedObject.GetIterator().GetVisibleChildren();
            var addressProperty = serializedObject.FindProperty(nameof(SimpleAddress.Address));
            var targetComponentsProperty = serializedObject.FindProperty(nameof(AddressableMaterial.TargetComponents));
            var addMat = target as AddressableMaterial;

            var rList = new ReorderableList(serializedObject, targetComponentsProperty, true, true, true, true);
            rList.drawElementCallback += OnDrawElement;
            void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                var element = rList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element);
            }

            targetComponentsProperty.isExpanded = EditorGUILayout.Foldout(targetComponentsProperty.isExpanded, "Target Components");
            if (targetComponentsProperty.isExpanded)
            {
                rList.DoLayoutList();
            }

            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            EditorGUILayout.PropertyField(addressProperty);
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

            using (new DisabledScope(true))
            {
                CustomDrawFoldoutInspector(addMat.AssetInstance, ref materialEditor);
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