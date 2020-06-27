using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SceneDesigner : MonoBehaviour
{
    // ================================== VARIABLES ==================================
    [System.Serializable]
    public class SceneDesignInfo
    {
        public SceneMgr.SceneType m_Type;
        public GameObject m_Pref;
        public List<ObjLocation> m_lElementLoc = new List<ObjLocation>();
    }

    #region Vars
    // public vars
    public string FILE_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_SCENE;
    public string FILE_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_SCENE + ".txt";

    public List<SceneDesignInfo> m_lSceneDesignInfo = new List<SceneDesignInfo>();
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region Unity funcs
    private void Update()
    {
        Rect camRect = CameraController.GetCamRectInEditor();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject sceneObj = transform.GetChild(i).gameObject;
            if (!sceneObj.active)
                continue;

            for (int j = 0; j < m_lSceneDesignInfo.Count; j++)
            {
                SceneDesignInfo sceneInfo = m_lSceneDesignInfo[j];
                if (sceneObj == sceneInfo.m_Pref)
                {
                    if (sceneInfo.m_lElementLoc.Count != sceneObj.transform.childCount)
                        sceneInfo.m_lElementLoc.Clear();

                    // update scene info
                    for (int k = 0; k < sceneObj.transform.childCount; k++)
                    {
                        GameObject sceneElementObj = sceneObj.transform.GetChild(k).gameObject;
                        ObjLocation objLocation = sceneInfo.m_lElementLoc.Find(x => x.m_ObjName == sceneElementObj.name);
                        // update location rect of element
                        if (objLocation != null)
                        {
                            // scale for all width & height
                            if (objLocation.m_ScaleSameByY != 0)
                            {
                                SpriteRenderer srElement = sceneElementObj.GetComponent<SpriteRenderer>();
                                float elementScale = (camRect.height * objLocation.m_ScaleSameByY * 100.0f) / srElement.sprite.rect.height;
                                sceneElementObj.transform.localScale = new Vector3(elementScale, elementScale, 1.0f);
                            }
                            objLocation.m_Rect = GetRectOfObjOnScene(camRect, sceneElementObj);
                        }
                            // add new rect of element
                        else
                        {
                            objLocation = new ObjLocation();
                            objLocation.m_ObjName = sceneElementObj.name;
                            objLocation.m_Rect = GetRectOfObjOnScene(camRect, sceneElementObj);
                            sceneInfo.m_lElementLoc.Add(objLocation);
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

        SceneJsonObj sceneJsonObj = new SceneJsonObj();
        for (int i = 0; i < m_lSceneDesignInfo.Count; i++)
        {
            SceneDesignInfo sceneDesignInfo = m_lSceneDesignInfo[i];
            SceneInfo sceneInfo = new SceneInfo();
            sceneInfo.m_Type = sceneDesignInfo.m_Type;
            sceneInfo.m_PrefPath = UtilityClass.GetPathOfObj(sceneDesignInfo.m_Pref);
            sceneInfo.m_lElementLoc = new List<ObjLocation>(sceneDesignInfo.m_lElementLoc);

            sceneJsonObj.m_lSceneInfo.Add(sceneInfo);
        }

        Debug.Log("saving ___ total SCENE info = " + sceneJsonObj.m_lSceneInfo.Count);
        string json = JsonUtility.ToJson(sceneJsonObj);
        Debug.Log(json);

        if (json.Length > 0)
            File.WriteAllText(FILE_DB_RAW_PATH, json);
    }

    public void Load()
    {
        SetDefaultPath();
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void SetDefaultPath()
    {
        FILE_DB_RESOURCE_PATH = AssetPathConstant.FILE_RAW_DB_SCENE;
        FILE_DB_RAW_PATH = "Assets/Resources/" + AssetPathConstant.FILE_RAW_DB_SCENE + ".txt";
    }

    //private string GetPathOfObj(GameObject obj)
    //{
    //    Object parentObject = EditorUtility.GetPrefabParent(obj);
    //    string path = AssetDatabase.GetAssetPath(parentObject);
    //    path = path.Replace("Assets/Resources/", "");
    //    path = path.Replace(".prefab", "");
    //    return path;
    //}

    private Rect GetRectOfObjOnScene(Rect camRect, GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Vector2 topleftPos = new Vector2(sr.bounds.min.x, sr.bounds.max.y);

        Rect rect = Rect.zero;
        rect.x = Mathf.Abs(topleftPos.x - camRect.x) / camRect.width;
        rect.y = Mathf.Abs(topleftPos.y - camRect.y) / camRect.height;
        rect.width = sr.bounds.size.x / camRect.width;
        rect.height = sr.bounds.size.y / camRect.height;
        return rect;
    }
    #endregion
}
