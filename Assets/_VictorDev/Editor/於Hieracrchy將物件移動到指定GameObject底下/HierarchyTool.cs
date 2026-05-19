using UnityEngine;
using UnityEditor;

public class HierarchyTool : EditorWindow
{
    // The target parent object
    private GameObject targetParent;

    [MenuItem("VictorDev Tools/Hierarchy視窗/移動所選物件到指定GameObject底下")]
    public static void ShowWindow()
    {
        GetWindow<HierarchyTool>("移動所選物件到指定GameObject底下");
    }

    private void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        
        // Field to assign the destination parent
        targetParent = (GameObject)EditorGUILayout.ObjectField("Target Parent", targetParent, typeof(GameObject), true);

        if (GUILayout.Button("Move Selected Objects"))
        {
            MoveSelected();
        }
    }

    private void MoveSelected()
    {
        if (targetParent == null)
        {
            Debug.LogError("Please assign a Target Parent first!");
            return;
        }

        // Get all selected transforms in the Hierarchy
        Transform[] selections = Selection.transforms;

        foreach (Transform t in selections)
        {
            // Records the action for the Undo system
            Undo.SetTransformParent(t, targetParent.transform, "Move to Parent");
            
            // Reset local position if needed (Optional)
            // t.localPosition = Vector3.zero;
        }

        Debug.Log($"Successfully moved {selections.Length} objects to {targetParent.name}");
    }
}