using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapDesigner))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapDesigner mapDesigner = (MapDesigner)target;
        if (GUILayout.Button("Save"))
        {
            mapDesigner.Save();
        }
        if (GUILayout.Button("Load"))
        {
            mapDesigner.Load();
        }
    }
}
