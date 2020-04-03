using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCL.SceneLib {
    static public class UCL_Scene {

        /// <summary>
        /// convert scene name to path!!
        /// </summary>
        /// <param name="scene_name"></param>
        /// <returns></returns>
        static public string GetScenePath(string scene_name) {
            
#if UNITY_EDITOR
            foreach(UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
                if(S.path.Length > 6) {
                    string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                    name = name.Substring(0, name.Length - 6);
                    if(name == scene_name) {
                        return S.path;
                    }
                }
            }
#endif
            return "";
        }
        /// <summary>
        /// Active scenes only!!
        /// </summary>
        /// <returns></returns>
        static public string[] GetScenesName() {
            List<string> SceneNames = new List<string>();
#if UNITY_EDITOR
            foreach(UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
                if(S.enabled) {
                    string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                    if(name.Length > 6) {
                        name = name.Substring(0, name.Length - 6);
                        SceneNames.Add(name);
                    }
                }
            }
#endif
            return SceneNames.ToArray();
        }

        /// <summary>
        /// Include none active scenes!!
        /// </summary>
        /// <returns></returns>
        static public string[] GetAllScenesName() {
            List<string> SceneNames = new List<string>();
#if UNITY_EDITOR
            foreach(UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                if(name.Length > 6) {
                    name = name.Substring(0, name.Length - 6);
                    SceneNames.Add(name);
                }
            }
#endif
            return SceneNames.ToArray();
        }
    }
}

