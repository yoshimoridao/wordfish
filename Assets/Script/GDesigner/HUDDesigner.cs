using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HUDDesigner : MonoBehaviour
{
    // ================================== VARIABLES ==================================
    [System.Serializable]
    public class HUDDesignInfo
    {
        public HUDMgr.HUDType m_HUDType;
        public GameObject m_Pref;
        public List<ObjLocation> m_lElementLoc = new List<ObjLocation>();
    }

    #region Vars
    // public vars
    public string FILE_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_HUD;
    public string FILE_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_HUD + ".txt";

    public List<HUDDesignInfo> m_lHUDDesignInfo = new List<HUDDesignInfo>();
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region Unity funcs
    private void Update()
    {
        Vector2 screenSize = CameraController.GetScreenSize();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject hudObj = transform.GetChild(i).gameObject;
            if (!hudObj.active)
                continue;

            for (int j = 0; j < m_lHUDDesignInfo.Count; j++)
            {
                HUDDesignInfo hudInfo = m_lHUDDesignInfo[j];
                if (hudObj == hudInfo.m_Pref)
                {
                    // update scene info
                    for (int k = 0; k < hudObj.transform.childCount; k++)
                    {
                        GameObject sceneElementObj = hudObj.transform.GetChild(k).gameObject;
                        if (!sceneElementObj.active)
                            continue;

                        ObjLocation objLocation = hudInfo.m_lElementLoc.Find(x => x.m_ObjName == sceneElementObj.name);
                        // update location rect of element
                        if (objLocation != null)
                        {
                            // scale for all width & height
                            if (objLocation.m_ScaleSameByY != 0)
                            {
                                Image img = sceneElementObj.GetComponent<Image>();
                                float elementScale = (screenSize.y * objLocation.m_ScaleSameByY) / img.sprite.rect.height;
                                RectTransform rt = sceneElementObj.GetComponent<RectTransform>();
                                //rt.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height) * elementScale;
                                rt.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
                                rt.localScale = Vector3.one * elementScale;
                            }
                            objLocation.m_Rect = GetRectOfObjOnCanvas(screenSize, sceneElementObj);
                        }
                        // add new rect of element
                        else
                        {
                            objLocation = new ObjLocation();
                            objLocation.m_ObjName = sceneElementObj.name;
                            objLocation.m_Rect = GetRectOfObjOnCanvas(screenSize, sceneElementObj);
                            hudInfo.m_lElementLoc.Add(objLocation);
                        }
                    }
                    break;
                }
            }
        }
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void Save()
    {
        SetDefaultPath();

        HUDJsonObj HUDJsonObj = new HUDJsonObj();
        for (int i = 0; i < m_lHUDDesignInfo.Count; i++)
        {
            HUDDesignInfo HUDDesignInfo = m_lHUDDesignInfo[i];
            HUDInfo HUDInfo = new HUDInfo();
            HUDInfo.m_HUDType = HUDDesignInfo.m_HUDType;
            HUDInfo.m_PrefPath = UtilityClass.GetPathOfObj(HUDDesignInfo.m_Pref);
            HUDInfo.m_lElementLoc = new List<ObjLocation>(HUDDesignInfo.m_lElementLoc);

            HUDJsonObj.m_lHUDInfo.Add(HUDInfo);
        }

        Debug.Log("saving ___ total HUD info = " + HUDJsonObj.m_lHUDInfo.Count);
        string json = JsonUtility.ToJson(HUDJsonObj);
        Debug.Log(json);

        if (json.Length > 0)
            File.WriteAllText(FILE_DB_RAW_PATH, json);
    }

    public void Load()
    {
        SetDefaultPath();
    }

    public void ClearListElement()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject hudObj = transform.GetChild(i).gameObject;
            if (!hudObj.active)
                continue;

            for (int j = 0; j < m_lHUDDesignInfo.Count; j++)
            {
                HUDDesignInfo hudInfo = m_lHUDDesignInfo[j];
                if (hudObj == hudInfo.m_Pref)
                {
                    hudInfo.m_lElementLoc.Clear();
                }
            }
        }
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void SetDefaultPath()
    {
        FILE_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_HUD;
        FILE_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_HUD + ".txt";
    }

    private Rect GetRectOfObjOnCanvas(Vector2 screenSize, GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();

        Vector2 topleftPos = new Vector2(rt.position.x - rt.sizeDelta.x / 2.0f, rt.position.y + rt.sizeDelta.y / 2.0f);

        Rect rect = Rect.zero;
        rect.x = topleftPos.x / screenSize.x;
        rect.y = topleftPos.y / screenSize.y;
        rect.width = rt.sizeDelta.x / screenSize.x;
        rect.height = rt.sizeDelta.y / screenSize.y;
        return rect;
    }
    #endregion
}
