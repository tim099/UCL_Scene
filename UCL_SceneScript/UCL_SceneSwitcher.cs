using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UCL.SceneLib {

#if UNITY_EDITOR
    public static class SceneHelper {
        static string m_SceneToOpen;
        static bool m_Rigistered = false;
        public static void StartScene(string scene) {
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


    [CreateAssetMenu(fileName = "New SceneSwitcher", menuName = "UCL_SceneSwitcher", order = 0)]
    public class UCL_SceneSwitcher : ScriptableObject {
        [System.Serializable]
        public struct SceneData {
            public string m_SceneName;// = "Assets/Scenes/";
            public string m_Path;
            public string m_IconName;
            //public Object m_Scene;
            public string GetIconName() {
                if(string.IsNullOrEmpty(m_IconName)) {
                    int len = m_SceneName.Length;
                    if(len > 2) len = 2;
                    return m_SceneName.Substring(0, len);
                }
                return m_IconName;
            }
            public string GetPath() {
                return m_Path + m_SceneName + ".unity";
            }
#if UNITY_EDITOR
            public void OpenScene() {
                var path = GetPath();
                SceneHelper.StartScene(path);
                UnityEditor.Selection.activeObject = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
            }
#endif
        }

        public List<SceneData> m_SceneDatas;
        [Core.UCL_StrListProperty(typeof(UCL_Scene), "GetAllScenesName")] public string m_LoadSceneName;
    }
}