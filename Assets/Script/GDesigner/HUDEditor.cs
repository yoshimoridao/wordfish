using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HUDDesigner))]
public class HUDEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HUDDesigner hudDesigner = (HUDDesigner)target;

        if (GUILayout.Button("Save"))
        {
            hudDesigner.Save();
        }
        if (GUILayout.Button("Clear Element List Of Active Obj"))
        {
            hudDesigner.ClearListElement();
        }
    }
}
