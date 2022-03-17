using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace PassivePicasso.SimplyAddress
{
    [CustomEditor(typeof(SimpleAddress), true)]
    public class SimpleAddressEditor : Editor
    {
        protected virtual bool CanUseAddress() => true;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            var iterator = serializedObject.GetIterator().Copy();
            var children = iterator.GetVisibleChildren().Where(child => child.name != "m_Script").ToArray();
            foreach (var child in children)
            {
                if (child.isArray && child.arrayElementType != "char")
                {
                    var rList = new ReorderableList(child.serializedObject, child, true, true, true, true);

                    void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) => EditorGUI.PropertyField(rect, rList.serializedProperty.GetArrayElementAtIndex(index));
                    void OnHeader(Rect rect) => GUI.Label(rect, child.displayName);

                    rList.drawHeaderCallback += OnHeader;
                    rList.drawElementCallback += OnDrawElement;

                    var imguiContainer = new IMGUIContainer(() =>
                    {
                        serializedObject.Update();
                        rList.DoLayoutList();
                        serializedObject.ApplyModifiedProperties();
                    });
                    root.Add(imguiContainer);
                }
                else
                    root.Add(new PropertyField(child));
            }
            return root;
        }
    }
}