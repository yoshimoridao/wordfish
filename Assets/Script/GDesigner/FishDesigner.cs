using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class FishDesigner : MonoBehaviour
{
    public string FISH_DB_RESOURCES_PATH = AssetPathConstant.FILE_RAW_DB_FISHES;
    public string FISH_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_FISHES + ".txt";
    public List<FishDataInfo> m_lFishDataInfo = new List<FishDataInfo>();

    void SetDefaultPath()
    {
        FISH_DB_RESOURCES_PATH = AssetPathConstant.FILE_RAW_DB_FISHES;
        FISH_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_FISHES + ".txt";
    }

    // ================================== UNITY FUNCS ==================================
    #region Unity funcs
    private void Update()
    {
        for (int i = 0; i < m_lFishDataInfo.Count; i++)
        {
            FishDataInfo fishInfo = m_lFishDataInfo[i];
            // fishes (Key in DB = FishKind_FishRank_FishSex_FishID_ShapeOfFish_FishColor)
            fishInfo.m_FishID = Fish.GetFishId(fishInfo);

            //fishInfo.m_SpriteName = ((int)fishInfo.m_FishKind).ToString()
            //    + ((int)fishInfo.m_FishRank).ToString()
            //    + ((int)fishInfo.m_FishSex).ToString()
            //    + fishInfo.m_FishID
            //    + ((int)fishInfo.m_Shape).ToString()
            //    + ((int)fishInfo.m_Color).ToString();
        }
    }
    #endregion

    public void Save()
    {
        SetDefaultPath();

        FishJsonObj fishJsonObj = new FishJsonObj();
        fishJsonObj.m_lFishDataInfo = new List<FishDataInfo>(m_lFishDataInfo);

        Debug.Log("saving ___ total FISHES info = " + fishJsonObj.m_lFishDataInfo.Count);

        string json = JsonUtility.ToJson(fishJsonObj);
        Debug.Log(json);

        if (json.Length > 0)
            File.WriteAllText(FISH_DB_RAW_PATH, json);
    }

    public void Load()
    {
        SetDefaultPath();
        m_lFishDataInfo.Clear();

        TextAsset txtAsset = Resources.Load<TextAsset>(FISH_DB_RESOURCES_PATH);
        if (txtAsset)
        {
            FishJsonObj fishJsonObj = JsonUtility.FromJson<FishJsonObj>(txtAsset.text);
            if (fishJsonObj != null)
            {
                foreach (var fishDataInfo in fishJsonObj.m_lFishDataInfo)
                    m_lFishDataInfo.Add(new FishDataInfo(fishDataInfo));

                Debug.Log("Loaded ___ raw FISH INFO ___" + m_lFishDataInfo.Count);
            }
        }
    }
}
