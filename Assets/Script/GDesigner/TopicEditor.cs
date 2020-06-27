using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopicDesigner))]
public class TopicEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TopicDesigner topicDesigner = (TopicDesigner)target;
        if (GUILayout.Button("Save"))
        {
            topicDesigner.Save();
        }
        if (GUILayout.Button("Load"))
        {
            topicDesigner.Load();
        }
    }
}
