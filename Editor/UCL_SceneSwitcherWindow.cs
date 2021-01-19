using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
                    if(GUILayout.Button(new GUIContent(s.Key, m_TestTexture, ""), EditorStyles.toolbarButton)) {
                        Init(s.Value);
                        //break;
                    }
                    GUILayout.Space(20);
                }
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

            //DrawToolBar();
            //GUILayout.Space(20);

            m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            m_Target = EditorGUILayout.ObjectField(m_Target, typeof(UCL_SceneSwitcher), false) as UCL_SceneSwitcher;
            if(GUILayout.Button("Add OpenScene")) {
                var data = new UCL_SceneSwitcher.SceneData();
                var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                data.m_Scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                Undo.RecordObject(m_Target, "add scene");
                m_Target.m_SceneDatas.Add(data);
                AssetDatabase.SaveAssets();
            }
            GUILayout.EndVertical();
            int delete_at = -1;
            using(var scope = new GUILayout.VerticalScope("box", GUILayout.Width(200))) {
                for(int i = 0; i < m_Target.m_SceneDatas.Count; i++) {
                    var v = m_Target.m_SceneDatas[i];
                    GUILayout.BeginHorizontal();
                    if(Core.UI.UCL_GUILayout.ButtonAutoSize("✖", 14, Color.gray, Color.red)) {
                        delete_at = i;
                    }
                    if(GUILayout.Button(v.GetSceneName())) {//, GUILayout.Width(80), GUILayout.Height(60f)
                        v.OpenScene();
                    }
                    GUILayout.EndHorizontal();
                    //GUILayout.Space(4);
                }
            }
            if(delete_at >= 0) {
                Undo.RecordObject(m_Target, "delete scene");
                m_Target.m_SceneDatas.RemoveAt(delete_at);
                AssetDatabase.SaveAssets();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();


            if(Event.current.type == EventType.Repaint) {
                var newRgn = GUILayoutUtility.GetLastRect();
                if(newRgn != m_GridRegion) {
                    m_GridRegion = newRgn;
                    Repaint();
                }
            }
        }
    }
}