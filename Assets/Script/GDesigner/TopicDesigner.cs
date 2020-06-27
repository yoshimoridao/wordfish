using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TopicDesigner : MonoBehaviour
{
    public string TOPIC_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_TOPIC;
    public string TOPIC_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_TOPIC + ".txt";
    public List<TopicInfo> m_lTopicInfo = new List<TopicInfo>();

    public List<TopicInfo> GetListTopicInfo()
    {
        if (m_lTopicInfo.Count == 0)
            Load();

        return new List<TopicInfo>(m_lTopicInfo);
    }

    void SetDefaultPath()
    {
        TOPIC_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_TOPIC;
        TOPIC_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_TOPIC + ".txt";
    }

    public void Save()
    {
        SetDefaultPath();

        TopicJsonObj topicJsonObj = new TopicJsonObj();
        topicJsonObj.m_lTopicInfo = new List<TopicInfo>(m_lTopicInfo);

        Debug.Log("saving ___ total topic info = " + topicJsonObj.m_lTopicInfo.Count);
        string json = JsonUtility.ToJson(topicJsonObj);
        Debug.Log(json);
        if (json.Length > 0)
            File.WriteAllText(TOPIC_DB_RAW_PATH, json);
    }

    public void Load()
    {
        SetDefaultPath();

        m_lTopicInfo.Clear();

        TextAsset txtAsset = Resources.Load<TextAsset>(TOPIC_DB_RESOURCE_PATH);
        if (txtAsset)
        {
            TopicJsonObj topicJsonObj = JsonUtility.FromJson<TopicJsonObj>(txtAsset.text);
            if (topicJsonObj != null)
            {
                m_lTopicInfo = topicJsonObj.m_lTopicInfo;
                Debug.Log("Loaded ___ raw TOPIC info ___" + m_lTopicInfo.Count);
            }
        }
    }
}
