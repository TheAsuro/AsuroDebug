using UnityEngine;

namespace AsuroDebug
{
    [AddComponentMenu("AsuroDebug")]
    public class DebugSettings : ScriptableObject
    {
        public static DebugSettings settingsObject;

        [SerializeField] public bool showDebugLines = true;
        [SerializeField] public float debugLineScale = 1f;
        [SerializeField] public bool showCenterofMass = true;
        [SerializeField] public float centerOfMassScale = 1f;

        void OnEnable()
        {
            settingsObject = this;
        }

        void OnDisable()
        {
            if (settingsObject == this)
                settingsObject = null;
        }
    }
}