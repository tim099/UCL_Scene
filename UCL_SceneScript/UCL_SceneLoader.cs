using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.Core.ObjectReflectionExtension;
namespace UCL.SceneLib {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_SceneLoader : MonoBehaviour {
        public bool IsLoading { get { return m_IsLoading; } }

        /// <summary>
        /// Scene name of the loading target
        /// </summary>
#if UNITY_EDITOR
        //[UCL.Core.PA.UCL_StrList(typeof(UCL.SceneLib.Lib), "GetScenesName")]
        [UCL.Core.PA.UCL_List("GetScenesName")]
#endif
        public string m_SceneName;

        /// <summary>
        /// The BuildSetting this SceneLoader use, if not setted then use the Editor setting
        /// 設定要使用的BuildSetting, 若為空則使用Editor的場景設定
        /// </summary>
        protected ScriptableObject BuildSetting
        {
            get
            {
                if (string.IsNullOrEmpty(m_BuildSetting))
                {
                    return null;
                }
                return UCL.Core.EditorLib.AssetDatabaseMapper.LoadAssetAtPath<UnityEngine.ScriptableObject>(m_BuildSetting);
            }
        }
        /// <summary>
        /// BuildSetting use for show list of SceneName
        /// </summary>
        [SerializeField] [UCL.Core.PA.UCL_EditorOnly] protected string m_BuildSetting = string.Empty;
        /// <summary>
        /// if(m_LoadOnStart == true) Auto load On Start() 
        /// 設為true時會在Start時自動載入場景
        /// </summary>
        public bool m_LoadOnStart = false;
        public bool m_AllowSceneActivation = false;

        protected bool m_IsLoading = false;
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
            Load(m_SceneName);
        }
        /// <summary>
        /// Load scene by iSceneName
        /// </summary>
        /// <param name="iSceneName">target scene name</param>
        virtual public void Load(string iSceneName, System.Action iLoadEndAct = null)
        {
            if (m_IsLoading)
            {
                Debug.LogError("Load:" + iSceneName + " ,Fail!! Already Loaing!!");
                return;//Already Loaing!!
            }
#if UNITY_EDITOR
            if (!CheckScenesInBuildSetting())
            {
                Debug.LogError("!CheckScenesInBuildSetting()");
                return;
            }
#endif
            m_IsLoading = true;

            var data = UCL.SceneLib.UCL_SceneManager.Instance.LoadScene(iSceneName);
            data.SetAllowSceneActivation(m_AllowSceneActivation);
            data.StartLoad();
            data.SetLoadDoneAct(delegate()
            {
                m_IsLoading = false;
                if(iLoadEndAct != null) iLoadEndAct.Invoke();
            });
        }
        #region Editor
#if UNITY_EDITOR
        public bool CheckScenesInBuildSetting() {
            return CheckScenesInBuildSetting(m_SceneName);
        }
        public bool CheckScenesInBuildSetting(string iSceneName)
        {
            if (string.IsNullOrEmpty(UCL.SceneLib.Lib.GetScenePath(iSceneName)))
            {
                if (BuildSetting != null)
                {
                    BuildSetting.Invoke("ApplyScenesInBuildSetting");
                    Debug.LogWarning("m_BuildSetting.ApplyScenesInBuildSetting()!!");
                    return false;
                }
            }
            return true;
        }
        public string[] GetScenesName() {
            if(BuildSetting == null) {
                return UCL.SceneLib.Lib.GetScenesName();
            }
            return (string[])BuildSetting.Invoke("GetScenesName");
        }
        public string GetScenePath(string iSceneName) {
            if(BuildSetting != null) {
                return (string)BuildSetting.Invoke("GetScenePath", iSceneName);
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