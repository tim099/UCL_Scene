using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.Core.ObjectReflectionExtension;
namespace UCL.SceneLib {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_SceneLoader : MonoBehaviour {
        /// <summary>
        /// Scene name of the loading target
        /// </summary>
        #if UNITY_EDITOR
        //[UCL.Core.PA.UCL_StrList(typeof(UCL.SceneLib.Lib), "GetScenesName")]
        [UCL.Core.PA.UCL_List("GetScenesName")]
        #endif
        public string m_SceneName;


#if UNITY_EDITOR
        [SerializeField] protected ScriptableObject m_BuildSetting;
#endif
        /// <summary>
        /// if(m_LoadOnStart == true) Auto load On Start() 
        /// </summary>
        public bool m_LoadOnStart = false;
        public bool m_AllowSceneActivation = false;

        protected bool f_Loading = false;
        void Start() {
            if(m_LoadOnStart == true) {
                Load();
            }
        }
        private void OnValidate() {
#if UNITY_EDITOR
            CheckScenesInBuildSetting();
#endif
        }
        /// <summary>
        /// Load target scene!!
        /// </summary>
        virtual public void Load() {
            if(f_Loading) return;//Already Loaing!!
            
#if UNITY_EDITOR
            if(!CheckScenesInBuildSetting()) {
                return;
            }
#endif
            f_Loading = true;

            var data = UCL.SceneLib.UCL_SceneManager.Instance.LoadScene(m_SceneName);
            data.SetAllowSceneActivation(m_AllowSceneActivation);
            data.StartLoad();
        }

        #region Editor
#if UNITY_EDITOR
        public bool CheckScenesInBuildSetting() {
            if(string.IsNullOrEmpty(UCL.SceneLib.Lib.GetScenePath(m_SceneName))) {
                if(m_BuildSetting != null) {
                    m_BuildSetting.Invoke("ApplyScenesInBuildSetting");
                    Debug.LogWarning("m_BuildSetting.ApplyScenesInBuildSetting()!!");
                    return false;
                }
            }
            return true;
        }
        public string[] GetScenesName() {
            if(m_BuildSetting == null) {
                return UCL.SceneLib.Lib.GetScenesName();
            }
            return (string[])m_BuildSetting.Invoke("GetScenesName");
        }
        public string GetScenePath(string scene_name) {
            if(m_BuildSetting != null) {
                return (string)m_BuildSetting.Invoke("GetScenePath", scene_name);
            }
            return UCL.SceneLib.Lib.GetScenePath(m_SceneName);
        }

        /// <summary>
        /// Create a button which invoke EditorLoadScene() when pressed
        /// </summary>
        //[Header("Press Invoke to loadscene in Editor Mode")]
        //[UCL.Core.PA.UCL_ButtonAttribute("EditorLoad")] public bool LoadScene;
        [Core.ATTR.UCL_FunctionButton("EditorLoad(Load target scene in Editor)")]
        public void EditorLoad() {
            //bool flag
            //Debug.LogWarning("EditorLoadScene:" + m_SceneName + ",flag:" + flag);
            string path = GetScenePath(m_SceneName);
            UCL.SceneLib.EditorSceneLoader.LoadScene(path);
        }
#endif
        #endregion
    }
}