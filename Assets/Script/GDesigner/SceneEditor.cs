using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneDesigner))]
public class SceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneDesigner sceneDesigner = (SceneDesigner)target;
        if (GUILayout.Button("Save"))
        {
            sceneDesigner.Save();
        }
        if (GUILayout.Button("Load"))
        {
            sceneDesigner.Load();
        }
    }
}
