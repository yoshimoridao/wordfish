using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VocaDesigner))]
public class VocaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        VocaDesigner vocaDesigner = (VocaDesigner)target;
        if (GUILayout.Button("Save"))
        {
            vocaDesigner.Save();
        }
        if (GUILayout.Button("Load"))
        {
            vocaDesigner.Load();
        }
    }
}
