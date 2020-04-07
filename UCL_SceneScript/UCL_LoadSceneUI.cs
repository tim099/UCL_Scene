using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UCL.SceneLib {
    public class UCL_LoadSceneUI : MonoBehaviour {
        [SerializeField] protected bool f_InitOnAwake = true;
        [SerializeField] protected bool f_DontDestroyOnLoad = true;
        [SerializeField] protected GameObject m_LoadingPanel;
        [SerializeField] protected GameObject m_LoadingCompletePanel;
        [SerializeField] protected Image m_ProgressBar;

        protected LoadSceneData m_LoadSceneData;
        private void Awake() {
            if(f_InitOnAwake) {
                Init();
            }
            if(f_DontDestroyOnLoad) {
                DontDestroyOnLoad(gameObject);
            }
        }
        private void OnApplicationQuit() {

        }
        virtual public void Init() {
            Rigister();
            if(m_LoadingPanel) m_LoadingPanel.SetActive(false);
            if(m_LoadingCompletePanel) m_LoadingCompletePanel.SetActive(false);
        }

        virtual public void AllowSceneActivation() {
            Debug.LogWarning("AllowSceneActivation():"+m_LoadSceneData.m_SceneName);
            m_LoadSceneData?.SetAllowSceneActivation(true);
        }

        virtual public void Rigister() {
            UCL.SceneLib.UCL_SceneManager.Instance?.SetLoadSceneUI(this);
        }
        virtual public void UnRigister() {
            if(m_LoadingPanel) m_LoadingPanel.SetActive(false);
            m_LoadSceneData = null;
        }
        virtual public void StartLoading(LoadSceneData data) {
            if(m_LoadingPanel) m_LoadingPanel.SetActive(true);
            if(m_LoadingCompletePanel) m_LoadingCompletePanel.SetActive(false);
            m_LoadSceneData = data;

            Debug.LogWarning("StartLoading:" + m_LoadSceneData.m_SceneName);
        }
        /// <summary>
        /// Scene loaded but not yet switch to nwe scene
        /// </summary>
        virtual public void CompleteLoading() {
            Debug.LogWarning("CompleteLoading():" + m_LoadSceneData.m_SceneName);
            if(m_LoadingCompletePanel) m_LoadingCompletePanel.SetActive(true);
        }
        virtual public void EndLoading() {
            if(m_LoadingPanel) m_LoadingPanel.SetActive(false);
            if(m_LoadingCompletePanel) m_LoadingCompletePanel.SetActive(false);
            m_LoadSceneData = null;
        }
        virtual protected void LoadingUpdate() {
            if(m_LoadSceneData == null) {//Loading
                return;
            }
            if(m_ProgressBar) m_ProgressBar.fillAmount = m_LoadSceneData.m_LoadProgress;

        }
        void Update() {
            LoadingUpdate();
        }
    }
}