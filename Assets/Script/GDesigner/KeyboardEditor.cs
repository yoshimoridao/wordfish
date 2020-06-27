using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KeyboardDesigner))]
public class KeyboardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        KeyboardDesigner kbDesigner = (KeyboardDesigner)target;
        if (GUILayout.Button("Save"))
        {
            kbDesigner.Save();
        }
        if (GUILayout.Button("Load"))
        {
            kbDesigner.Load();
        }
    }
}
