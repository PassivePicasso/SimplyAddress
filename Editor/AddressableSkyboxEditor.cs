using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.EditorGUI;
using UnityObject = UnityEngine.Object;

namespace PassivePicasso.SimplyAddress
{
    [InitializeOnLoad]
    [CustomEditor(typeof(AddressableSkybox), true)]
    public class AddressableSkyboxEditor : Editor
    {
        [InitializeOnLoadMethod]
        static void InitializeSkyboxSystem()
        {
            EditorSceneManager.sceneSaving += SceneSaving;
            EditorSceneManager.sceneSaved += SceneSaved;
        }

        static void SceneSaved(Scene scene)
        {
            var addressableSkybox = scene.GetRootGameObjects().SelectMany(rgo => rgo.GetComponentsInChildren<AddressableSkybox>()).FirstOrDefault();
            if (!addressableSkybox) return;
            RenderSettings.skybox = addressableSkybox.material;
        }

        static void SceneSaving(Scene scene, string path)
        {
            var addressableSkybox = scene.GetRootGameObjects().SelectMany(rgo => rgo.GetComponentsInChildren<AddressableSkybox>()).FirstOrDefault();
            if (!addressableSkybox) return;
            RenderSettings.skybox = null;
        }

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