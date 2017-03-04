using UnityEngine;
using UnityEngine.UI;

namespace AsuroDebug.VR
{
    public class VrDebugWindow : MonoBehaviour
    {
        [SerializeField] private ScrollRect consoleScrollRect;
        [SerializeField] private Text consoleOutput;
        [SerializeField] private Toggle keepAreaVisibleToggle;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Text fpsText;

        private float offset;
        private Transform eyeTransform;
        private SteamVR_PlayArea playArea;
        private VrDebug vrDebug;

        public void Set(VrDebug debug, Transform eyeTransform, SteamVR_PlayArea playArea, float offset)
        {
            gameObject.SetActive(false);

            vrDebug = debug;
            this.eyeTransform = eyeTransform;
            this.playArea = playArea;
            this.offset = offset;

            VrDebug.ConsoleLines.ForEach(Write);
            VrDebug.OnNewConsoleLine += (s, args) => Write(args.Content);

            Application.logMessageReceived += (condition, trace, type) =>
            {
                string log = "";
                switch (type)
                {
                    case LogType.Assert:
                        log += "[Assert] " + condition;
                        break;
                    case LogType.Error:
                        log += "[Error] " + condition;
                        break;
                    case LogType.Warning:
                        log += "[Warn] " + condition;
                        break;
                    case LogType.Log:
                        log += "[Log] " + condition;
                        break;
                    case LogType.Exception:
                        log += "[Exception] " + condition;
                        break;
                    default:
                        log += "[Unknown] " + condition;
                        break;
                }
                VrDebug.WriteLine(log);
            };

            keepAreaVisibleToggle.onValueChanged.AddListener(value => debug.IsPlayAreaAlwaysVisible = value);

            closeButton.onClick.AddListener(vrDebug.CloseDebugWindow);

            quitButton.onClick.AddListener(() =>
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            });
        }

        public void Open()
        {
            gameObject.SetActive(true);

            // TODO always spawn in play area
            transform.position = eyeTransform.position + eyeTransform.forward * offset;
            transform.rotation = Quaternion.Euler(new Vector3(0f, eyeTransform.rotation.eulerAngles.y, 0f));
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                fpsText.text = (1f / Time.smoothDeltaTime).ToString("0") + " FPS";
            }
        }

        private void Write(string line)
        {
            consoleOutput.text += line;
            if (consoleOutput.text.Length > 1000)
                consoleOutput.text = consoleOutput.text.Substring(900);
            consoleScrollRect.verticalScrollbar.value = 0f;
        }
    }
}