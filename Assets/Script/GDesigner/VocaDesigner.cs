using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VocaDesigner : MonoBehaviour
{
    public TopicDesigner Ref_TopicDesigner;

    public string VOCA_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_VOCA;
    public string VOCA_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_VOCA + ".txt";

    [SerializeField]
    public List<VocasInfo> m_lVocaInfo = new List<VocasInfo>();
    List<TopicInfo> m_lTopicInfo = new List<TopicInfo>();

    public List<VocasInfo> GetListVocasInfoDesign()
    {
        if (m_lVocaInfo.Count == 0)
            Load();

        return new List<VocasInfo>(m_lVocaInfo);
    }

    void SetDefaultPath()
    {
        VOCA_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_VOCA;
        VOCA_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_VOCA + ".txt";
    }

    void LoadTopic()
    {
        // load topic
        if (m_lTopicInfo == null || m_lTopicInfo.Count == 0)
            m_lTopicInfo = Ref_TopicDesigner.GetListTopicInfo();
    }

    public void Save()
    {
        SetDefaultPath();
        // load topic
        LoadTopic();

        VocasJsonObj vocasJson = new VocasJsonObj();
        vocasJson.m_lVocasInfo = new List<VocasInfo>(m_lVocaInfo);

        Debug.Log("saving ___ total voca info = " + vocasJson.m_lVocasInfo.Count);
        string json = JsonUtility.ToJson(vocasJson);
        Debug.Log(json);

        if (json.Length > 0)
            File.WriteAllText(VOCA_DB_RAW_PATH, json);
    }

    public void Load()
    {
        SetDefaultPath();
        // load topic
        LoadTopic();

        m_lVocaInfo.Clear();

        TextAsset txtAsset = Resources.Load<TextAsset>(VOCA_DB_RESOURCE_PATH);
        if (txtAsset)
        {
            VocasJsonObj vocaJsonObj = JsonUtility.FromJson<VocasJsonObj>(txtAsset.text);
            m_lVocaInfo = new List<VocasInfo>(vocaJsonObj.m_lVocasInfo);
        }
    }
}
