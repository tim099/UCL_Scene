using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.SceneLib {
    [CreateAssetMenu(fileName = "New SceneSetting", menuName = "UCL/SceneSetting")]
    public class UCL_SceneSetting : ScriptableObject {
        public UCL_LoadSceneUI m_DefaultLoadSceneUI;


        static public UCL_SceneSetting Get() {
            return Resources.Load<UCL_SceneSetting>("SceneSetting");
        }
#if UNITY_EDITOR
        [UnityEditor.MenuItem("UCL/SceneLib/SceneSetting")]
        static public void OpenSceneSwitcher() {
            //Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Libs/UCL_Modules/UCL_Scene/SceneSwitcher.asset");
            UnityEditor.Selection.activeObject = Get();
        }
#endif
    }
}

