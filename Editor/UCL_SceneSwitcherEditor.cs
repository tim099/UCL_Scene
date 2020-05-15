using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UCL.SceneLib {
    public class UCL_SceneSwitcherWindow : EditorWindow {
        Texture2D m_TestTexture;
        Rect m_GridRegion = new Rect();
        Vector2 m_ScrollPos = new Vector2(0, 0);
        public Dictionary<string, UCL_SceneSwitcher> m_Switchers = new Dictionary<string, UCL_SceneSwitcher>();
        public UCL_SceneSwitcher m_Target;
        virtual public void Init(UCL_SceneSwitcher _Target = null) {
            if(_Target != null) {
                m_Target = _Target;
            }
            m_Switchers[m_Target.name] = m_Target;

            Selection.activeObject = m_Target;

            m_TestTexture = EditorGUIUtility.Load("icons/animation.addevent.png") as Texture2D;
            m_ScrollPos = new Vector2(0, 0);
            //m_CubeManager.m_Cubes.ForEach(node => node.RefreshNode());
        }
        protected virtual void DrawToolBar() {
            using(new EditorGUILayout.HorizontalScope(EditorStyles.toolbar)) {
                foreach(var s in m_Switchers) {
                    if(GUILayout.Button(new GUIContent(s.Key, m_TestTexture, ""),EditorStyles.toolbarButton)) {
                        Init(s.Value);
                        //break;
                    }
                    GUILayout.Space(20);
                }
                /*
                if(GUILayout.Button(new GUIContent("Refresh", m_TestTexture, "Refresh graph status"),
                    EditorStyles.toolbarButton, GUILayout.Width(80))) {
                    //Init(m_CubeManager);
                }
                GUILayout.Space(20);
                if(GUILayout.Button(new GUIContent("Refresh", m_TestTexture, "Refresh graph status"),
                    EditorStyles.toolbarButton, GUILayout.Width(80))) {
                    //Init(m_CubeManager);
                }
                */
            }
        }
#if UNITY_2017_2_OR_NEWER
        private void OnPlaymodeChanged(PlayModeStateChange s) {
            /*
            if(m_controller != null && m_controller.TargetGraph != null) {
                SaveGraph();
            }
            */
            if(s == PlayModeStateChange.EnteredEditMode || s == PlayModeStateChange.EnteredPlayMode) {
                Init();
            }
        }
#endif

        private void OnGUI() {
            if(m_Target == null) return;
            GUILayout.BeginVertical();
            DrawToolBar();
            GUILayout.Space(20);
            using(var scrollScope = new EditorGUILayout.ScrollViewScope(m_ScrollPos)) {
                m_ScrollPos = scrollScope.scrollPosition;
                using(new EditorGUILayout.VerticalScope()) {
                    int wid = 4;
                    int x = 0;
                    //int y = 0;
                    foreach(var v in m_Target.m_SceneDatas) {
                        
                        if(x == 0) {
                            GUILayout.BeginHorizontal();
                        }
                        if(GUILayout.Button(new GUIContent(v.GetSceneName(), m_TestTexture, ""),
                                    EditorStyles.toolbarButton)) {//, GUILayout.Width(80), GUILayout.Height(60f)
                            v.OpenScene();
                        }
                        GUILayout.Space(30);

                        x++;
                        if(x >= wid) {
                            GUILayout.EndHorizontal();
                            x = 0;
                        }
                    }
                    /*
                    //m_GirdBackground.Draw(m_GridRegion, m_ScrollPos);
                    //Handles.DrawLine(new Vector3(0f, 0f, 0f), new Vector3(100f, 500f, 0f));
                    {
                        BeginWindows();
                        //m_CubeManager.m_Cubes.ForEach(node => node.DrawNode());
                        EndWindows();
                    }
                    
                    if(Event.current.type == EventType.Layout) {
                        GUILayoutUtility.GetRect(new GUIContent(string.Empty), GUIStyle.none, GUILayout.Width(0),
                            GUILayout.Height(0));
                    }
                    */
                }
            }
            GUILayout.EndVertical();

            if(Event.current.type == EventType.Repaint) {
                var newRgn = GUILayoutUtility.GetLastRect();
                if(newRgn != m_GridRegion) {
                    m_GridRegion = newRgn;
                    Repaint();
                }
            }
        }
    }

    [CustomEditor(typeof(UCL_SceneSwitcher), true)]
    public class UCL_SceneSwitcherEditor : Editor {
        public UCL_SceneSwitcherWindow ShowWindow(UCL_SceneSwitcher target) {
            Debug.LogWarning("ShowWindow() !!");
            var window = EditorWindow.GetWindow<UCL_SceneSwitcherWindow>("SceneSwitcher");
            window.Init(target);
            return window;
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var controller = (UCL_SceneSwitcher)target;

            if(GUILayout.Button("Load:" + controller.m_LoadSceneName)) {
                EditorSceneLoader.LoadScene(Lib.GetScenePath(controller.m_LoadSceneName));
            }
            GUILayout.Space(10);
            if(GUILayout.Button("Show Window")) {
                ShowWindow(controller);
            }
            GUILayout.Space(20);
            //GUILayout.Height(100);
            var buts = controller.m_SceneDatas;
            if(buts != null) {
                foreach(var but in buts) {
                    if(GUILayout.Button(but.GetSceneName())) {
                        but.OpenScene();
                    }
                }
                //if(altered) EditorUtility.SetDirty(controller);
                
            }
        }
    }
}