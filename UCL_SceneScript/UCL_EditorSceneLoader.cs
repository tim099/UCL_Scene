using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.Core.EditorLib;



namespace UCL.SceneLib {
    public static class EditorSceneLoader {
#if UNITY_EDITOR
        public static void LoadScene(string iSceneName) {
            if(UCL.Core.EditorLib.EditorApplicationMapper.isPlaying) {
                UCL.Core.EditorLib.EditorApplicationMapper.isPlaying = false;
            }

            UCL.Core.EditorLib.UCL_EditorUpdateManager.AddAction(()=>Load(iSceneName));
        }

        static void Load(string iSceneName) {
            if(string.IsNullOrEmpty(iSceneName)) {
                return;
            }

            if(UCL.Core.EditorLib.EditorApplicationMapper.isPlaying || UCL.Core.EditorLib.EditorApplicationMapper.isPaused ||
                UCL.Core.EditorLib.EditorApplicationMapper.isCompiling || UCL.Core.EditorLib.EditorApplicationMapper.isPlayingOrWillChangePlaymode) {
                UCL.Core.EditorLib.UCL_EditorUpdateManager.AddAction(() => Load(iSceneName));//Wait 1 Frame
                return;
            }

            if(UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(iSceneName);
            }
        }
#endif
    }
}
