using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapDesigner : MonoBehaviour
{
    public TopicDesigner Ref_TopicDesigner;
    public VocaDesigner Ref_VocaDesigner;

    public string FILE_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_MAP;
    public string FILE_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_MAP + ".txt";

    // ref list
    List<VocasInfo> m_lVocaInfoDesign = new List<VocasInfo>();
    List<TopicInfo> m_lTopicInfo = new List<TopicInfo>();
    // design list
    public List<MapInfo> m_lMapInfo = new List<MapInfo>();

    void SetDefaultPath()
    {
        FILE_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_MAP;
        FILE_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_MAP + ".txt";
    }

    void GetRefData()
    {
        // load topic
        if (m_lTopicInfo == null || m_lTopicInfo.Count == 0)
            m_lTopicInfo = Ref_TopicDesigner.GetListTopicInfo();
        if (m_lVocaInfoDesign == null || m_lVocaInfoDesign.Count == 0)
            m_lVocaInfoDesign = Ref_VocaDesigner.GetListVocasInfoDesign();
    }

    public void Save()
    {
        SetDefaultPath();
        GetRefData();

        MapJsonObj mapJsonObj = new MapJsonObj();
        mapJsonObj.m_lMapInfo = new List<MapInfo>(m_lMapInfo);

        Debug.Log("saving ___ total voca info = " + mapJsonObj.m_lMapInfo.Count);
        string json = JsonUtility.ToJson(mapJsonObj);
        Debug.Log(json);

        if (json.Length > 0)
            File.WriteAllText(FILE_DB_RAW_PATH, json);
    }

    public void Load()
    {
        SetDefaultPath();
        GetRefData();

        m_lMapInfo.Clear();

        TextAsset txtAsset = Resources.Load<TextAsset>(FILE_DB_RESOURCE_PATH);
        if (txtAsset)
        {
            MapJsonObj mapJsonObj = JsonUtility.FromJson<MapJsonObj>(txtAsset.text);
            if (mapJsonObj != null)
            {
                m_lMapInfo = new List<MapInfo>(mapJsonObj.m_lMapInfo);
                Debug.Log("Loaded ___ raw MAP INFO ___" + m_lMapInfo.Count);
            }
        }
    }
}
