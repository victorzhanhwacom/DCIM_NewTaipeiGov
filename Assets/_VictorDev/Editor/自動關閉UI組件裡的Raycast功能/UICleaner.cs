#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VzDev.EditorUtils
{
    public class UICleaner
    {
        [MenuItem("VictorDev Tools/UI/Disable Selection's Raycast Targets")]
        static void DisableRaycasts()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Graphic[] graphics = obj.GetComponentsInChildren<Graphic>(true);
                foreach (Graphic g in graphics)
                {
                    g.raycastTarget = false;
                    EditorUtility.SetDirty(g);
                }
            }
        }
    }
}
#endif