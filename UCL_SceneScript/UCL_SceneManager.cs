using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UCL.SceneLib {
    [System.Serializable]
    public class LoadSceneData {
        public LoadSceneData(string _SceneName) {
            m_SceneName = _SceneName;
            m_AllowSceneActivation = true;
            m_LoadProgress = 0;
            m_LoadDone = false;
            m_LoadComplete = false;
            f_LoadStart = false;
            m_Mode = LoadSceneMode.Single;
        }
        /// <summary>
        /// Start Load scene!!
        /// </summary>
        /// <returns></returns>
        public LoadSceneData StartLoad() {
            if(f_LoadStart) return this;
            f_LoadStart = true;

            UCL_SceneManager.Instance.LoadScene(this);
            return this;
        }

        public LoadSceneData SetAllowSceneActivation(bool flag) {
            m_AllowSceneActivation = flag;
            if(m_AsyncOperation != null) {
                m_AsyncOperation.allowSceneActivation = m_AllowSceneActivation;
            }
            
            return this;
        }
        /// <summary>
        /// Call once when LoadComplete
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public LoadSceneData SetLoadCompleteAct(System.Action act) {
            m_LoadCompleteAct = act;
            return this;
        }
        /// <summary>
        /// Call once when LoadDone
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public LoadSceneData SetLoadDoneAct(System.Action act) {
            m_LoadDoneAct = act;
            return this;
        }
        public LoadSceneData SetLoadSceneMode(LoadSceneMode mode) {
            m_Mode = mode;
            return this;
        }


        public void SetActiveScene() {
            m_AllowSceneActivation = true;
        }

        #region protected 
        //Dont call any func in this region except LoadSceneManager!!

        /// <summary>
        /// Dont call this func outside LoadSceneManager!!
        /// LoadInit
        /// </summary>
        public void LoadInit() {
            if(m_AsyncOperation != null) {
                return;
            }
            m_LoadingTime = 0;
            m_PredictLoadTime = -1;
            string LoadTimeKey = LoadingTimeKey();
            if(PlayerPrefs.HasKey(LoadTimeKey)) {
                m_PredictLoadTime = PlayerPrefs.GetFloat(LoadTimeKey);
                Debug.Log(m_SceneName+"_PredictLoadTime:" + m_PredictLoadTime);
            };

            m_AsyncOperation = SceneManager.LoadSceneAsync(m_SceneName, m_Mode);
            m_AsyncOperation.allowSceneActivation = m_AllowSceneActivation;//stop auto enter new scene
        }

        /// <summary>
        /// Dont call this func outside LoadSceneManager!!
        /// return true if load end!!
        /// </summary>
        public bool LoadingUpdate() {
            const float done_percent = 0.8999f;
            m_LoadingTime += Time.deltaTime;

            if(m_LoadComplete) {
                m_LoadProgress = 1.0f;
            } else {
                if(m_PredictLoadTime > 0) {
                    float percent = m_LoadingTime / m_PredictLoadTime;

                    m_LoadProgress = percent;
                } else {
                    m_LoadProgress = (m_AsyncOperation.progress / done_percent);
                }
                if(m_LoadProgress > 0.99f) m_LoadProgress = 0.99f;
            }
            m_AsyncOperation.allowSceneActivation = m_AllowSceneActivation;

            if(!m_LoadComplete) {
                if(m_AsyncOperation.progress >= done_percent) {
                    LoadComplete();
                    PlayerPrefs.SetFloat(LoadingTimeKey(), m_LoadingTime);
                }
                Debug.Log(m_SceneName+"_LoadProgress:" + m_LoadProgress);
            }
            if(!m_LoadDone) {
                if(m_AsyncOperation.isDone) LoadDone();
            }
            
            return m_LoadDone;
        }


        #endregion
        protected string LoadingTimeKey() {
            return "UCL_LoadingTime_" + m_SceneName;
        }
        protected void LoadDone() {
            m_LoadDone = true;
            m_LoadDoneAct?.Invoke();
        }
        protected void LoadComplete() {
            m_LoadComplete = true;
            m_LoadCompleteAct?.Invoke();
        }

        public bool m_LoadComplete { get; protected set; }//90% Loaded and wait for end!!
        public bool m_LoadDone { get; protected set; }//Load End!!
        public bool m_AllowSceneActivation { get; protected set; }

        public float m_LoadProgress { get; protected set; }
        public string m_SceneName { get; protected set; }
        public LoadSceneMode m_Mode { get; protected set; }

        protected bool f_LoadStart;
        protected float m_PredictLoadTime;
        protected float m_LoadingTime;
        protected System.Action m_LoadCompleteAct;
        protected System.Action m_LoadDoneAct;
        protected AsyncOperation m_AsyncOperation;
    }

    public class UCL_SceneManager : Core.UCL_Singleton<UCL_SceneManager> {
        Queue<LoadSceneData> m_LoadSceneDataQue = new Queue<LoadSceneData>();
        protected LoadSceneData m_CurLoadSceneData = null;
        protected UCL_LoadSceneUI m_LoadSceneUI;
        //bool m_ApplicationQuit = false;
        //public bool m_Loading { get{ return m_CurLoadSceneData != null; } private set { } }
        private void Awake() {
        }
        private void Start() {
            if(m_LoadSceneUI == null) {
                if(UCL_LoadSceneUI.GetInstance()) {
                    m_LoadSceneUI = UCL_LoadSceneUI.GetInstance();
                } else {
                    var UI = UCL_SceneSetting.Get().m_DefaultLoadSceneUI;
                    if(UI) {
                        UCL_LoadSceneUI.CreateInstance(UI);
                    }
                }
            }
            UCL.Core.UCL_GameManager.Instance.m_ExitGameEvent.AddListener(ExitGameEvent);
        }
        virtual protected void ExitGameEvent() {
            m_CurLoadSceneData?.SetAllowSceneActivation(true);
        }
        override protected void OnDestroy() {
            base.OnDestroy();
            m_CurLoadSceneData?.SetAllowSceneActivation(true);
        }
        private void OnApplicationQuit() {
            //m_ApplicationQuit = true;
        }
        public void SetLoadSceneUI(UCL_LoadSceneUI _LoadSceneUI) {
            if(m_LoadSceneUI == _LoadSceneUI) return;

            if(m_LoadSceneUI) {
                m_LoadSceneUI.UnRigister();
            }

            m_LoadSceneUI = _LoadSceneUI;
        }
        public LoadSceneData LoadScene(string _SceneName) {
            var data = new LoadSceneData(_SceneName);
            //LoadScene(data);
            return data;
        }
        public void LoadScene(LoadSceneData data) {
            m_LoadSceneDataQue.Enqueue(data);
        }
        public bool GetIsLoading() { return m_CurLoadSceneData != null; }

        void StartLoadScene() {
            if(GetIsLoading() || Core.UCL_GameManager.Instance.f_ExitGame) return;
            if(m_LoadSceneDataQue.Count == 0) return;
            //Debug.LogWarning("StartLoadScene()");
            StartCoroutine(LoadSceneCoroutine(m_LoadSceneDataQue.Dequeue()));
        }

        IEnumerator LoadSceneCoroutine(LoadSceneData data) {
            if(Core.UCL_GameManager.Instance.f_ExitGame) yield break;
            //Debug.LogWarning("LoadSceneCoroutine:" + data.m_SceneName);
            m_CurLoadSceneData = data;
            UCL.Core.UCL_GameManager.Instance.m_BlockExitGameFlag.Add("LoadSceneCoroutine");
            m_CurLoadSceneData.LoadInit();
            m_LoadSceneUI?.StartLoading(data);
            bool complete = false;
            while(!m_CurLoadSceneData.LoadingUpdate()) {//!m_AsyncOperation.isDone
                
                /*
                if(m_ApplicationQuit || m_Destroyed) {
                    m_AsyncOperation.allowSceneActivation = true;
                } else {
                    m_AsyncOperation.allowSceneActivation = m_CurLoadSceneData.m_AllowSceneActivation;
                }
                */
                if(!complete && m_CurLoadSceneData.m_LoadComplete) {
                    complete = true;
                    m_LoadSceneUI?.CompleteLoading();
                }
                yield return null;
            }
            m_LoadSceneUI?.EndLoading();
            m_CurLoadSceneData = null;
            UCL.Core.UCL_GameManager.Instance.m_BlockExitGameFlag.Remove("LoadSceneCoroutine");
            //Debug.LogWarning("LoadSceneCoroutineEnd:" + data.m_SceneName);
        }
        void UpdateAction() {
            if(GetIsLoading() || Core.UCL_GameManager.Instance.f_ExitGame) return;
            if(m_LoadSceneDataQue.Count > 0) {
                StartLoadScene();
            }

        }

        private void Update() {
            UpdateAction();
        }


    }
}