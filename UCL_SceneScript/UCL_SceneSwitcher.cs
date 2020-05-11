using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace UCL.SceneLib {


#if UNITY_EDITOR
    public static class EditorSceneLoader {
        static string m_SceneToOpen;
        static bool m_Rigistered = false;
        public static void LoadScene(string scene) {
            if(UnityEditor.EditorApplication.isPlaying) {
                UnityEditor.EditorApplication.isPlaying = false;
            }

            m_SceneToOpen = scene;
            if(!m_Rigistered) {
                UnityEditor.EditorApplication.update += OnUpdate;
                m_Rigistered = true;
            } else {
                Debug.LogWarning("SceneHelper m_Rigistered!!");
            }

        }

        static void OnUpdate() {
            if(m_SceneToOpen == null) {
                UnityEditor.EditorApplication.update -= OnUpdate;
                m_Rigistered = false;
                return;
            }

            if(UnityEditor.EditorApplication.isPlaying || UnityEditor.EditorApplication.isPaused ||
                UnityEditor.EditorApplication.isCompiling || UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }

            UnityEditor.EditorApplication.update -= OnUpdate;
            m_Rigistered = false;

            if(UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(m_SceneToOpen);
                //EditorApplication.isPlaying = true;
            }
            m_SceneToOpen = null;
        }
    }
#endif



[CreateAssetMenu(fileName = "New SceneSwitcher", menuName = "UCL/SceneSwitcher", order = 0)]
    public class UCL_SceneSwitcher : ScriptableObject {
        [System.Serializable]
        public struct SceneData {
            [UCL.Core.PA.UCL_Button("OpenScene")] public Object m_Scene;

            public string GetSceneName() {
                if(m_Scene == null) return "";

                return m_Scene.name;
            }
#if UNITY_EDITOR
            public string GetPath() {
                if(m_Scene == null) return "";

                return AssetDatabase.GetAssetPath(m_Scene.GetInstanceID());
            }
            public void OpenScene() {
                var path = GetPath();
                EditorSceneLoader.LoadScene(path);
                //UnityEditor.Selection.activeObject = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
            }
            public void OpenScene(Object obj) {
                Debug.LogWarning("Cool!!:" + obj.name);
                var path = GetPath();
                EditorSceneLoader.LoadScene(path);
                //UnityEditor.Selection.activeObject = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
            }
#endif
        }
#if UNITY_EDITOR
        [MenuItem("UCL/SceneSwitcher")]
        static public void OpenSceneSwitcher() {
            //Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Libs/UCL_Modules/UCL_Scene/SceneSwitcher.asset");
            Selection.activeObject = Resources.Load<UCL.SceneLib.UCL_SceneSwitcher>("SceneSwitcher");
        }
#endif
        public List<SceneData> m_SceneDatas;
        [Core.PA.UCL_StrList(typeof(Lib), "GetAllScenesName")] public string m_LoadSceneName;
    }
}