using UnityEngine;
using UnityEditor;

/// <summary>
/// Enhanced Hierarchy navigation with Auto-Focus.
/// Uses EditorGUIUtility.PingObject to ensure the selected object stays in view.
/// </summary>
public class HierarchyLevelNavigator : Editor
{
    [MenuItem("Tools/Hierarchy/Move To Parent _%[")]
    private static void MoveToParent()
    {
        Transform selected = Selection.activeTransform;

        if (selected == null || selected.parent == null) return;

        // Perform the re-parenting
        Undo.SetTransformParent(selected, selected.parent.parent, "Move Up Level");
        
        // Focus and Ping logic
        FocusOnSelected(selected.gameObject);
    }

    [MenuItem("Tools/Hierarchy/Move To Child _%]")]
    private static void MoveToChild()
    {
        Transform selected = Selection.activeTransform;
        if (selected == null) return;

        Transform previousSibling = GetPreviousSibling(selected);

        if (previousSibling != null)
        {
            Undo.SetTransformParent(selected, previousSibling, "Move Down Level");
            
            // Focus and Ping logic
            FocusOnSelected(selected.gameObject);
        }
    }

    private static void FocusOnSelected(GameObject obj)
    {
        // Keep the selection active
        Selection.activeGameObject = obj;

        // Force Hierarchy to scroll to the object and highlight it
        EditorGUIUtility.PingObject(obj);
    }

    private static Transform GetPreviousSibling(Transform t)
    {
        int currentIndex = t.GetSiblingIndex();
        if (currentIndex <= 0) return null;

        if (t.parent != null)
        {
            return t.parent.GetChild(currentIndex - 1);
        }
        else
        {
            // Root-level sibling search
            GameObject[] roots = t.gameObject.scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                if (root.transform.GetSiblingIndex() == currentIndex - 1)
                    return root.transform;
            }
        }
        return null;
    }
}