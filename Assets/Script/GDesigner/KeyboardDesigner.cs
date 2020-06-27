using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KeyboardDesigner : MonoBehaviour
{
    public string FILE_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_KEYBOARD_TEMPLATE;
    public string FILE_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_KEYBOARD_TEMPLATE + ".txt";
    public List<KbTemplateInfo> m_lKbDesign = new List<KbTemplateInfo>();

    void SetDefaultPath()
    {
        FILE_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_KEYBOARD_TEMPLATE;
        FILE_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_KEYBOARD_TEMPLATE + ".txt";
    }

    public void Save()
    {
        SetDefaultPath();

        KbJsonObj kbJsonObj = new KbJsonObj();
        kbJsonObj.m_lKbTemplate = new List<KbTemplateInfo>(m_lKbDesign);
        Debug.Log("saving ___ total KEYBOARD info = " + kbJsonObj.m_lKbTemplate.Count);

        string json = JsonUtility.ToJson(kbJsonObj);
        Debug.Log(json);

        if (json.Length > 0)
            File.WriteAllText(FILE_DB_RAW_PATH, json);
    }

    public void Load()
    {
        SetDefaultPath();

        m_lKbDesign.Clear();

        TextAsset txtAsset = Resources.Load<TextAsset>(FILE_DB_RESOURCE_PATH);
        if (txtAsset)
        {
            KbJsonObj kbJsonObj = JsonUtility.FromJson<KbJsonObj>(txtAsset.text);
            if (kbJsonObj != null)
            {
                m_lKbDesign = new List<KbTemplateInfo>(kbJsonObj.m_lKbTemplate);
                Debug.Log("Loaded ___ raw KEYBOARD info ___" + m_lKbDesign.Count);
            }
        }
    }
}
