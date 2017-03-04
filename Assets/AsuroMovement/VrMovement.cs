//using UnityEngine;
//
//namespace AsuroMovement
//{
//    [RequireComponent(typeof(ParticleSystem))]
//    public class VrMovement : MonoBehaviour
//    {
//        [SerializeField] private OVRCameraRig playerCam;
//        [SerializeField] private OvrAvatar avatar;
//        [SerializeField] private float aimXzMultiplier = 0.8f;
//        [SerializeField] private float aimYMultiplier = 0.8f;
//
//        private bool aiming;
//
//        private void Update()
//        {
//            if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
//            {
//                aiming = true;
//            }
//            if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick))
//            {
//                aiming = false;
//                GetComponent<ParticleSystem>().Clear();
//            }
//
//            if (aiming)
//            {
//                ParticleSystem ps = GetComponent<ParticleSystem>();
//                ps.Clear();
//
//                Vector3 playerFootPosition = playerCam.trackingSpace.TransformPoint(avatar.Base.transform.position);
//                //VrDebug.SpawnBall(playerFootPosition, Color.black); wrong
//                // TODO @LEFT not nice for left-handed people :(
//                Vector3 playerHandPosition = playerCam.trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
//                Quaternion playerHandRotation = playerCam.trackingSpace.rotation * OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
//                Vector3 playerHandForward = playerHandRotation * Vector3.forward;
//
//                Vector3 currentPos = playerHandPosition;
//                Vector3 currentVelocity = new Vector3(aimXzMultiplier * playerHandForward.x, aimYMultiplier * playerHandForward.y, aimXzMultiplier * playerHandForward.z);
//                for (int i = 0; currentPos.y > playerFootPosition.y && i < 1000; i++)
//                {
//                    currentVelocity += new Vector3(0f, -0.1f, 0f);
//                    currentPos += currentVelocity;
//                    ps.Emit(new ParticleSystem.EmitParams() {position = currentPos}, 1);
//                }
//            }
//        }
//    }
//}