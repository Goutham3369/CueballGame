using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelHandler))]
public class LevelHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelHandler script = (LevelHandler)target;

        if (GUILayout.Button("Update Level List"))
        {
            script.UpdateLevelList();
        }
    }
}
