using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UCL.SceneLib {
    public class UCL_LoadSceneUI : Core.UCL_Singleton<UCL_LoadSceneUI> {
        [SerializeField] protected bool f_RigisterOnAwake = true;
        [SerializeField] protected bool f_InitOnAwake = true;
        [SerializeField] protected GameObject m_LoadingPanel;
        [SerializeField] protected GameObject m_LoadingCompletePanel;
        [SerializeField] protected Image m_ProgressBar;
        public float m_CurrentProgress;
        float m_MaxStep = 0.1f;
        bool m_ShouldEnd;

        protected LoadSceneData m_LoadSceneData;
        private void Awake() {
            /*
            if(!SetInstance(this)) {
                return;
            }
            */
            ReplaceInstance(this);
            
            if(f_InitOnAwake) {
                Init();
            }
            if(f_RigisterOnAwake) {
                UCL_SceneManager.Instance?.SetLoadSceneUI(this);
            }
            m_ShouldEnd = false;
            m_CurrentProgress = 0.0f;
        }
        private void OnApplicationQuit() {

        }
        virtual public void Init() {
            Rigister();
            if(m_LoadingPanel) m_LoadingPanel.SetActive(false);
            if(m_LoadingCompletePanel) m_LoadingCompletePanel.SetActive(false);
        }

        virtual public void AllowSceneActivation() {
            if(m_LoadSceneData == null) return;

            Debug.LogWarning("AllowSceneActivation():"+m_LoadSceneData.m_SceneName);
            m_LoadSceneData.SetAllowSceneActivation(true);
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

            m_ShouldEnd = false;
            m_CurrentProgress = 0.0f;
            if (m_ProgressBar)
            {
                m_ProgressBar.fillAmount = m_CurrentProgress;
            }

            //Debug.LogWarning("StartLoading:" + m_LoadSceneData.m_SceneName);
        }
        /// <summary>
        /// Scene loaded but not yet switch to new scene
        /// </summary>
        virtual public void CompleteLoading() {
            //Debug.LogWarning("CompleteLoading():" + m_LoadSceneData.m_SceneName);
            //if(m_LoadingCompletePanel) m_LoadingCompletePanel.SetActive(true);
        }
        virtual public void EndLoading() {
            m_ShouldEnd = true;
        }
        virtual protected void LoadingUpdate() {
            if(m_LoadSceneData == null) {//Loading
                return;
            }
            if (m_LoadSceneData.m_LoadProgress > m_CurrentProgress)
            {
                if (m_LoadSceneData.m_LoadProgress > m_CurrentProgress + m_MaxStep) m_CurrentProgress += m_MaxStep;
                else m_CurrentProgress = m_LoadSceneData.m_LoadProgress;
            }
            if (m_ProgressBar)
            {
                //m_ProgressBar.fillAmount = m_LoadSceneData.m_LoadProgress;
                m_ProgressBar.fillAmount = m_CurrentProgress;
            }
            if(m_ShouldEnd && m_CurrentProgress >= 1.0f)
            {
                if (m_LoadingCompletePanel) m_LoadingCompletePanel.SetActive(true);
                m_CurrentProgress += m_MaxStep;
                if (m_CurrentProgress > 1.2f)
                {
                    if (m_LoadingPanel) m_LoadingPanel.SetActive(false);
                    if (m_LoadingCompletePanel) m_LoadingCompletePanel.SetActive(false);
                    m_LoadSceneData = null;
                    m_ProgressBar.fillAmount = 0.0f;
                }
            }
        }
        void FixedUpdate() {
            LoadingUpdate();
        }
    }
}