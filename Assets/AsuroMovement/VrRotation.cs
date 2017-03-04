//using System;
//using UnityEngine;
//
//namespace AsuroMovement
//{
//    public class VrRotation : MonoBehaviour
//    {
//        public static event EventHandler OnRotate;
//
//        private void Update()
//        {
//            if (OVRInput.GetDown(OVRInput.RawButton.B))
//            {
//                transform.Rotate(new Vector3(0f, 90f, 0f), Space.Self);
//                if (OnRotate != null)
//                    OnRotate(this, new EventArgs());
//            }
//            if (OVRInput.GetDown(OVRInput.RawButton.Y))
//            {
//                transform.Rotate(new Vector3(0f, -90f, 0f), Space.Self);
//                if (OnRotate != null)
//                    OnRotate(this, new EventArgs());
//            }
//        }
//    }
//}