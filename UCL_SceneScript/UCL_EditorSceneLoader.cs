using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.Core.EditorLib;



namespace UCL.SceneLib {
    public static class EditorSceneLoader {
        static string m_SceneToOpen;
        static bool m_Rigistered = false;
#if UNITY_EDITOR
        public static void LoadScene(string scene) {
            if(UCL.Core.EditorLib.EditorApplicationMapper.isPlaying) {
                UCL.Core.EditorLib.EditorApplicationMapper.isPlaying = false;
            }

            m_SceneToOpen = scene;
            if(!m_Rigistered) {
                EditorApplicationMapper.update += OnUpdate;
                m_Rigistered = true;
            } else {
                Debug.LogWarning("SceneHelper m_Rigistered!!");
            }

        }

        static void OnUpdate() {
            if(m_SceneToOpen == null) {
                EditorApplicationMapper.update -= OnUpdate;
                m_Rigistered = false;
                return;
            }

            if(UCL.Core.EditorLib.EditorApplicationMapper.isPlaying || UCL.Core.EditorLib.EditorApplicationMapper.isPaused ||
                UCL.Core.EditorLib.EditorApplicationMapper.isCompiling || UCL.Core.EditorLib.EditorApplicationMapper.isPlayingOrWillChangePlaymode) {
                return;
            }

            EditorApplicationMapper.update -= OnUpdate;
            m_Rigistered = false;

            if(UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(m_SceneToOpen);
                //EditorApplication.isPlaying = true;
            }
            m_SceneToOpen = null;
        }
#endif
    }
}
