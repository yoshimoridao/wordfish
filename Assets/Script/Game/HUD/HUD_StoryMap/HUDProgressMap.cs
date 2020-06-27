using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDProgressMap : GHUDElement
{
    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
    public override void OnCreateObj()
    {
        base.OnCreateObj();
    }

    public override void OnUpdateObj(float a_dt)
    {
        base.OnUpdateObj(a_dt);
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void ShowProgress(GameObject nodeObj, Vector2 nodeProgress, GameObject mapObj)
    {
        SetActive(true);
        // set SCALE & POSITION of progress cont following target node
        RectTransform rtProgressCont = GetComponent<RectTransform>();
        rtProgressCont.localScale = mapObj.GetComponent<RectTransform>().localScale;
        rtProgressCont.position = nodeObj.GetComponent<RectTransform>().position;

        // parse PROGRESS string
        string strProgress = "";
        strProgress += (nodeProgress.x < 10) ? "0" + nodeProgress.x.ToString() : nodeProgress.x.ToString();
        strProgress += "/";
        strProgress += (nodeProgress.y < 10) ? "0" + nodeProgress.y.ToString() : nodeProgress.y.ToString();

        // format: xx/xx (length = 5)
        GameObject progressObj = transform.GetChild(0).gameObject;
        for (int i = 0; i < 5; i++)
        {
            GameObject noObj = progressObj.transform.GetChild(i).gameObject;
            Sprite sprite = Resources.Load<Sprite>(AssetPathConstant.FOLDER_HIGHLIGHT_PROGRESS_NO_PATH + "/" + strProgress[i].ToString());
            if (sprite)
            {
                noObj.GetComponent<Image>().sprite = sprite;
                RectTransform rtNoObj = noObj.GetComponent<RectTransform>();
                rtNoObj.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
            }
        }
    }

    public void HideProgress()
    {
        SetActive(false);
    }
    #endregion
}
