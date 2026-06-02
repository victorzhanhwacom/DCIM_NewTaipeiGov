using UnityEditor;
using UnityEngine;

namespace VzDev.DebugUtils
{
    public static class GizmoHelper
    {
        #region 繪製機櫃U層數Gizmo
        /// 繪製機櫃U層Gizmo
        public static void DrawRackUGizmos(Bounds bounds, float unitHeight, int totalRu=42, Color? gizmosColor = null)
        {
            Color preGizmosColor = Gizmos.color;
            Gizmos.color = gizmosColor ?? Color.green;
            
            Vector3 bottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            for (int i = 0; i <= totalRu; i++)
            {
                float y = bottomLeft.y + i * unitHeight;
                DrawHorizontalLine(bounds, y);

#if UNITY_EDITOR
                if (i < totalRu)
                    DrawULabel(bounds, i, y);
#endif
            }
            Gizmos.color = preGizmosColor;
        }

        private static void DrawHorizontalLine(Bounds bounds, float y)
        {
            Vector3 frontLeft  = new Vector3(bounds.min.x, y, bounds.min.z);
            Vector3 frontRight = new Vector3(bounds.max.x, y, bounds.min.z);
            Vector3 backLeft   = new Vector3(bounds.min.x, y, bounds.max.z);
            Vector3 backRight  = new Vector3(bounds.max.x, y, bounds.max.z);

            Gizmos.DrawLine(frontLeft, frontRight);
            Gizmos.DrawLine(backLeft, backRight);
        }
        
#if UNITY_EDITOR
        private static void DrawULabel(Bounds bounds, int index, float y)
        {
            Vector3 labelPos = new Vector3(bounds.min.x, y, bounds.min.z) + Vector3.left * 0.05f;
            Handles.Label(labelPos, $"U{index + 1}");
        }
#endif
        #endregion
    }
}