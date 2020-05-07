using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.SceneLib {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_SceneLoader : MonoBehaviour {
        /// <summary>
        /// Scene name of the loading target
        /// </summary>
        [UCL.Core.PA.UCL_StrList(typeof(UCL.SceneLib.Lib), "GetScenesName")] public string m_SceneName;

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
        /// <summary>
        /// Load target scene!!
        /// </summary>
        virtual public void Load() {
            if(f_Loading) return;//Already Loaing!!
            f_Loading = true;
            var data = UCL.SceneLib.UCL_SceneManager.Instance.LoadScene(m_SceneName);
            data.SetAllowSceneActivation(m_AllowSceneActivation);
            data.StartLoad();
        }
        #region Editor
#if UNITY_EDITOR
        /// <summary>
        /// Create a button which invoke EditorLoadScene() when pressed
        /// </summary>
        //[Header("Press Invoke to loadscene in Editor Mode")]
        //[UCL.Core.PA.UCL_ButtonAttribute("EditorLoad")] public bool LoadScene;
        [Core.ATTR.UCL_FunctionButton("EditorLoad(Load target scene in Editor)")]
        public void EditorLoad() {
            //bool flag
            //Debug.LogWarning("EditorLoadScene:" + m_SceneName + ",flag:" + flag);
            string path = UCL.SceneLib.Lib.GetScenePath(m_SceneName);
            UCL.SceneLib.EditorSceneLoader.LoadScene(path);
        }
#endif
        #endregion
    }
}