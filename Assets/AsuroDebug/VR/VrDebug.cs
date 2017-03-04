using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace AsuroDebug.VR
{
    public class VrDebug : MonoBehaviour
    {
        [SerializeField] private GameObject debugWindowPrefab;
        [SerializeField] private SteamVR_ControllerManager controllerManager;
        [SerializeField] private SteamVR_PlayArea playArea;
        [SerializeField] private Transform eyeTransform;
        [SerializeField] private float displaySpawnDistance = 0.75f;
        [SerializeField] private LayerMask menuRaycastLayer;
        [SerializeField] private Vector3 menuRayStart;
        [SerializeField] private Vector3 menuRayDirection;
        [SerializeField] private float menuRayLength;

        private LineRenderer menuRayRenderer;
        private VrDebugWindow debugWindow;

        private bool IsDebugWindowOpen
        {
            get { return debugWindow != null && debugWindow.gameObject.activeSelf; }
        }

        public bool IsPlayAreaAlwaysVisible { get; set; }

        public bool IsPlayAreaVisible
        {
            get { return playArea.GetComponent<MeshRenderer>().enabled; }
            set { playArea.GetComponent<MeshRenderer>().enabled = value; }
        }

        private SteamVR_TrackedObject leftControllerObj;
        private SteamVR_TrackedObject rightControllerObj;

        private SteamVR_Controller.Device LeftControllerDevice
        {
            get { return SteamVR_Controller.Input((int) leftControllerObj.index); }
        }

        private SteamVR_Controller.Device RightControllerDevice
        {
            get { return SteamVR_Controller.Input((int) rightControllerObj.index); }
        }

        private Vector3 RayStart
        {
            get { return rightControllerObj.transform.position + rightControllerObj.transform.rotation * menuRayStart; }
        }

        private Vector3 RayDirection
        {
            get { return rightControllerObj.transform.localRotation * menuRayDirection.normalized; }
        }

        private void Awake()
        {
            leftControllerObj = controllerManager.left.GetComponent<SteamVR_TrackedObject>();
            rightControllerObj = controllerManager.right.GetComponent<SteamVR_TrackedObject>();

            menuRayRenderer = rightControllerObj.gameObject.GetComponent<LineRenderer>();
            menuRayRenderer.useWorldSpace = false;
            menuRayRenderer.SetPositions(new[] {menuRayStart, menuRayStart + menuRayDirection * menuRayLength});
            menuRayRenderer.enabled = false;

//            OVRManager.display.RecenteredPose += SetTrackingParticleSpawners;
//            OVRManager.TrackingAcquired += SetTrackingParticleSpawners;
//            VrRotation.OnRotate += (s, e) => SetTrackingParticleSpawners();
        }

        private void Update()
        {
            if (IsDebugWindowOpen && rightControllerObj.isValid && RightControllerDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                RaycastHit hit;
                if (Physics.Raycast(RayStart, RayDirection, out hit, menuRayLength, menuRaycastLayer))
                {
                    Button button = hit.collider.GetComponent<Button>();
                    if (button != null)
                        button.onClick.Invoke();
                    Toggle toggle = hit.collider.GetComponent<Toggle>();
                    if (toggle != null)
                        toggle.isOn = !toggle.isOn;
                }
            }

            if (leftControllerObj.isValid && rightControllerObj.isValid)
            {
                if ((LeftControllerDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && RightControllerDevice.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                    || (LeftControllerDevice.GetPress(SteamVR_Controller.ButtonMask.Trigger) && RightControllerDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)))
                {
                    if (!IsDebugWindowOpen)
                        OpenDebugWindow();
                    else
                        CloseDebugWindow();
                }
            }
        }

        public void OpenDebugWindow()
        {
            CheckDebugWindow();
            debugWindow.Open();
            IsPlayAreaVisible = true;
            menuRayRenderer.enabled = true;
        }

        public void CloseDebugWindow()
        {
            Assert.IsNotNull(debugWindow);
            debugWindow.Close();
            IsPlayAreaVisible = IsPlayAreaAlwaysVisible;
            menuRayRenderer.enabled = false;
        }

        private void CheckDebugWindow()
        {
            if (debugWindow == null)
            {
                debugWindow = Instantiate(debugWindowPrefab).GetComponent<VrDebugWindow>();
                debugWindow.Set(this, eyeTransform, playArea, displaySpawnDistance);
            }
        }

        #region "Static Stuff"

        public static event EventHandler<DebugEventArgs<string>> OnNewConsoleLine;
        private static List<string> consoleLines = new List<string>();

        public static List<string> ConsoleLines
        {
            get { return consoleLines; }
        }

        private static List<GameObject> balls = new List<GameObject>();

        public static void Write(string text)
        {
            consoleLines.Add(text);
            if (OnNewConsoleLine != null)
                OnNewConsoleLine(null, new DebugEventArgs<string>(text));
        }

        public static void WriteLine(string text)
        {
            Write(text + '\n');
        }

        public static void SpawnBall(Vector3 worldPos, Color color)
        {
            GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.GetComponent<MeshRenderer>().material.color = color;
            Destroy(ball.GetComponent<Collider>());
            ball.transform.position = worldPos;
            ball.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            balls.Add(ball);
        }

        public static void ClearBalls()
        {
            balls.ForEach(Destroy);
            balls.Clear();
        }

        #endregion
    }

    public class DebugEventArgs<T> : EventArgs
    {
        public T Content { get; private set; }

        public DebugEventArgs(T content)
        {
            Content = content;
        }
    }
}