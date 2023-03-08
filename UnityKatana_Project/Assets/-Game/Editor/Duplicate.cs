using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DuplicateSpecial : EditorWindow
{
    private Vector3 conOffset;
    private int count;
    private Vector3 relOffset;
    private Renderer rend;

    // object to duplicate
    private GameObject target;

    // wrapper object for duplicates
    private GameObject wrapper;

    private void OnDestroy()
    {
        if (count == 0) Rollback();
    }

    private void OnGUI()
    {
        target = Selection.activeGameObject;
        if (target == null)
        {
            ErrorMsg("Please select an object to duplicate from Hierarchy View");
            return;
        }

        rend = target.GetComponentInChildren<Renderer>();

        EditorGUI.BeginChangeCheck();

        count = EditorGUILayout.IntField("Count", count);
        relOffset = EditorGUILayout.Vector3Field("Relative offset", relOffset);
        conOffset = EditorGUILayout.Vector3Field("Constant offset", conOffset);

        if (EditorGUI.EndChangeCheck())
        {
            Rollback(); // destroy wrapper from previous changes check tick before creating a new one
            Apply();
        }

        if (GUILayout.Button("Ok")) Close();
        if (GUILayout.Button("Cancel"))
        {
            Rollback();
            Close();
        }
    }

    [MenuItem("Foxy/Duplicate Tool")]
    private static void Init()
    {
        var window = (DuplicateSpecial) GetWindow(typeof(DuplicateSpecial), true, "Duplicate Tool");
        window.Show();
    }

    private void Apply()
    {
        wrapper = new GameObject(target.name + "_duplicates_" + count);

        for (var i = 1; i <= count; i++)
        {
            var clone = Instantiate(target, target.transform.position, target.transform.rotation);

            var offset = new Vector3(rend.bounds.size.x * relOffset.x + conOffset.x,
                             rend.bounds.size.y * relOffset.y + conOffset.y,
                             rend.bounds.size.z * relOffset.z + conOffset.z)
                         * i;
            clone.transform.Translate(offset);
            clone.transform.SetParent(wrapper.transform);
        }
    }

    private void Rollback()
    {
        DestroyImmediate(wrapper);
    }

    private void ErrorMsg(string msg)
    {
        EditorGUILayout.LabelField(msg);
    }
}