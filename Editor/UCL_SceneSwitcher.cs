using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace UCL.SceneLib {

    [CreateAssetMenu(fileName = "New SceneSwitcher", menuName = "UCL/SceneSwitcher", order = 0)]
    ///UCL_SceneSwitcher is under Editor folder and is only for Editor usage
    public class UCL_SceneSwitcher : ScriptableObject {
        [System.Serializable]
        public struct SceneData {
            [UCL.Core.PA.UCL_Button("OpenScene")] public SceneAsset m_Scene;

            public string GetSceneName() {
                if(m_Scene == null) return "";

                return m_Scene.name;
            }
            public string GetPath() {
                if(m_Scene == null) return "";

                return AssetDatabase.GetAssetPath(m_Scene.GetInstanceID());
            }
            public void OpenScene() {
                var path = GetPath();
                EditorSceneLoader.LoadScene(path);
                //UnityEditor.Selection.activeObject = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
            }
            public void OpenScene(SceneAsset obj) {
                var path = GetPath();
                EditorSceneLoader.LoadScene(path);
                //UnityEditor.Selection.activeObject = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
            }
        }
        [MenuItem("UCL/SceneLib/SceneSwitcher")]
        static public void OpenSceneSwitcher() {
            //Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Libs/UCL_Modules/UCL_Scene/SceneSwitcher.asset");
            Selection.activeObject = Resources.Load<UCL.SceneLib.UCL_SceneSwitcher>("SceneSwitcher");
        }
        [MenuItem("UCL/SceneSwitcherWindow")]
        static public void OpenSceneSwitcherWindow() {
            UCL_SceneSwitcherEditor.ShowWindow(Resources.Load<UCL.SceneLib.UCL_SceneSwitcher>("SceneSwitcher"));
        }
        public List<SceneData> m_SceneDatas;
        [Core.PA.UCL_StrList(typeof(Lib), "GetAllScenesName")] public string m_LoadSceneName;
    }
}