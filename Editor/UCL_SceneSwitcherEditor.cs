using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UCL.SceneLib {
    [CustomEditor(typeof(UCL_SceneSwitcher), true)]
    public class UCL_SceneSwitcherEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var controller = (UCL_SceneSwitcher)target;

            if(GUILayout.Button("Load:" + controller.m_LoadSceneName)) {
                SceneHelper.StartScene(UCL_Scene.GetScenePath(controller.m_LoadSceneName));
            }

            var buts = controller.m_SceneDatas;
            if(buts != null) {
                foreach(var but in buts) {
                    if(GUILayout.Button(but.m_SceneName)) {
                        but.OpenScene();
                    }
                }
            }
        }
    }
}