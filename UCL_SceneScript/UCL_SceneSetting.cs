using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.SceneLib {
    [CreateAssetMenu(fileName = "New SceneSetting", menuName = "UCL/SceneSetting")]
    public class UCL_SceneSetting : ScriptableObject {
        public UCL_LoadSceneUI m_DefaultLoadSceneUI;
    }
}

