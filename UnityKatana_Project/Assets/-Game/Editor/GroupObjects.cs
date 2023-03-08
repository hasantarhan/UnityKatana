using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GroupUnityObjects : Editor
{
    [MenuItem("Edit/Group %g", false)]
    public static void Group()
    {
        if (Selection.transforms.Length > 0)
        {
            var group = new GameObject("Group");

            // Set Pivot
            var pivotPosition = Vector3.zero;
            foreach (var g in Selection.transforms) pivotPosition += g.transform.position;
            pivotPosition /= Selection.transforms.Length;
            group.transform.position = pivotPosition;

            // Undo action
            Undo.RegisterCreatedObjectUndo(group, "Group");
            foreach (var s in Selection.gameObjects) Undo.SetTransformParent(s.transform, group.transform, "Group");

            Selection.activeGameObject = group;
        }
        else
        {
            Debug.LogWarning("No selection!");
        }
    }
}