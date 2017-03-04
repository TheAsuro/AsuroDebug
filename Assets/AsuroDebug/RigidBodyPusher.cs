using UnityEngine;

namespace AsuroDebug
{
    public class RigidBodyPusher : MonoBehaviour
    {
        public bool pushOnlyOnce = true;
        public Vector3 velocityAxis = Vector3.up;
        public bool addVelocity = false;
        public Vector3 angularVelocityAxis = Vector3.forward;
        public bool addAngularVelocity = false;

        private Rigidbody RB
        {
            get { return GetComponent<Rigidbody>(); }
        }

        void FixedUpdate()
        {
            if (addVelocity)
            {
                RB.velocity += velocityAxis * Time.deltaTime;
                if (pushOnlyOnce)
                    addVelocity = false;
            }

            if (addAngularVelocity)
            {
                RB.angularVelocity += angularVelocityAxis * Time.deltaTime;
                if (pushOnlyOnce)
                    addAngularVelocity = false;
            }
        }
    }
}