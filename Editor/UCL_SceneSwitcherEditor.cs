using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UCL.SceneLib {
    [CustomEditor(typeof(UCL_SceneSwitcher), true)]
    public class UCL_SceneSwitcherEditor : Editor {

        //[UnityEditor.Callbacks.OnOpenAssetAttribute(1)]
        //public static bool step1(int instanceID, int line) {
        //    return false; // we did not handle the open
        //}

        // step2 has an attribute with index 2, so will be called after step1
        [UnityEditor.Callbacks.OnOpenAssetAttribute(2)]
        public static bool step2(int instanceID, int line) {
            var data = EditorUtility.InstanceIDToObject(instanceID) as UCL_SceneSwitcher;
            if(data != null) {
                ShowWindow(data);
                return true;
            }
            return false; // we did not handle the open
        }
        public static UCL_SceneSwitcherWindow ShowWindow(UCL_SceneSwitcher target) {
            Debug.LogWarning("UCL_SceneSwitcherWindow ShowWindow() !!");
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