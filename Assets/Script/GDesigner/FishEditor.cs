using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FishDesigner))]
public class FishEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FishDesigner fishDesigner = (FishDesigner)target;
        if (GUILayout.Button("Save"))
        {
            fishDesigner.Save();
        }
        if (GUILayout.Button("Load"))
        {
            fishDesigner.Load();
        }
    }
}
