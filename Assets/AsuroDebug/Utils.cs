using System;
using UnityEngine;

namespace AsuroDebug
{
    public static class Utils
    {
        public static void DrawLine(Vector3 position, Vector3 direction, Color color)
        {
            if (DebugSettings.settingsObject != null && DebugSettings.settingsObject.showDebugLines)
            {
                Debug.DrawLine(position, position + direction * DebugSettings.settingsObject.debugLineScale, color);
            }
        }

        public static void DrawSphereGizmo(Vector3 position, float radius, Color color)
        {
            if (DebugSettings.settingsObject != null && DebugSettings.settingsObject.showCenterofMass)
            {
                Gizmos.color = color;
                Gizmos.DrawSphere(position, radius * DebugSettings.settingsObject.centerOfMassScale);
            }
        }

        public static Vector3 DivideParts(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3 MultiplyParts(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        // thanks stackoverflow
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length==j) ? Arr[0] : Arr[j];
        }
    }
}