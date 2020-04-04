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
            //public string m_SceneName;// = "Assets/Scenes/";
            //public string m_Path;
            public string m_IconName;
            public Object m_Scene;
            /*
            public bool UpdateData() {
                bool altered = false;
#if UNITY_EDITOR
                if(m_Scene == null) {
                    return false;
                }
                
                if(m_SceneName != m_Scene.name) {
                    m_SceneName = m_Scene.name;
                    altered = true;
                }
                var path = AssetDatabase.GetAssetPath(m_Scene.GetInstanceID());
                if(path != m_Path) {
                    m_Path = path;
                    altered = true;
                }
#endif
                return altered;
            }
            */
            public string GetSceneName() {
                if(m_Scene == null) return "";

                return m_Scene.name;
            }
            public string GetIconName() {
                if(m_Scene == null) return "";

                string m_SceneName = m_Scene.name;
                if(string.IsNullOrEmpty(m_IconName)) {
                    int len = m_SceneName.Length;
                    if(len > 2) len = 2;
                    return m_SceneName.Substring(0, len);
                }
                return m_IconName;
            }
            public string GetPath() {
                if(m_Scene == null) return "";

                return AssetDatabase.GetAssetPath(m_Scene.GetInstanceID());
            }
#if UNITY_EDITOR
            public void OpenScene() {
                var path = GetPath();
                EditorSceneLoader.LoadScene(path);
                //UnityEditor.Selection.activeObject = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
            }
#endif
        }

        [MenuItem("UCL/SceneSwitcher")]
        static public void OpenSceneSwitcher() {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Libs/UCL_Modules/UCL_Scene/SceneSwitcher.asset");
        }

        public List<SceneData> m_SceneDatas;
        [Core.UCL_StrListProperty(typeof(UCL_Scene), "GetAllScenesName")] public string m_LoadSceneName;
    }
}