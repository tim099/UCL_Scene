using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace UCL.SceneLib {
    #region Editor
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
    #endregion

}
#endif