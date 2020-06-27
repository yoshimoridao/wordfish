using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GHUD : GObject
{
    // ================================== VARIABLES ==================================
    #region Vars
    // protected vars
    /// <summary>
    /// list contains elements of HUD
    /// </summary>
    protected List<GHUDElement> m_lHUDElements = new List<GHUDElement>();
    // protected vars
    protected HUDInfo m_HUDInfo;
    #endregion

    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
    public override void OnCreateObj()
    {
        base.OnCreateObj();
    }

    public override void OnUpdateObj(float a_dt)
    {
        base.OnUpdateObj(a_dt);

        for (int i = 0; i < m_lHUDElements.Count; i++)
        {
            GHUDElement gElement = m_lHUDElements[i];
            gElement.OnUpdateObj(a_dt);
        }
    }

    public override void OnDestroyObj()
    {
        for (int i = 0; i < m_lHUDElements.Count; i++)
        {
            RemoveHUDElement(i);
        }

        base.OnDestroyObj();
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public virtual void Init(HUDInfo a_HUDInfo)
    {
        m_HUDInfo = a_HUDInfo;
    }

    public HUDInfo GetHUDInfo()
    {
        return new HUDInfo(m_HUDInfo);
    }
    #endregion

    // ================================== PROTECTED FUNCS ==================================
    #region Protected Funcs
    protected void AddHUDElement(GHUDElement a_hudElement)
    {
        // init Element
        a_hudElement.OnCreateObj();
        a_hudElement.Init(this);
        m_lHUDElements.Add(a_hudElement);

        // set location for element
        SetLocationForElement(a_hudElement.gameObject);
    }

    protected void RemoveHUDElement(GHUDElement a_hudElement)
    {
        int elementIndex = m_lHUDElements.FindIndex(x => x == a_hudElement);
        RemoveHUDElement(elementIndex);
    }
    protected void RemoveHUDElement(int a_elementIndex)
    {
        if (a_elementIndex != -1)
        {
            GHUDElement element = m_lHUDElements[a_elementIndex];
            element.OnDestroyObj();
            m_lHUDElements.RemoveAt(a_elementIndex);
        }
    }

    protected void SetLocationForElement(GameObject a_hudElementObj)
    {
        int index = m_HUDInfo.m_lElementLoc.FindIndex(x => x.m_ObjName == a_hudElementObj.name);
        if (index != -1)
        {
            Vector2 screenSize = CameraController.GetScreenSize();

            GameObject elementObj = a_hudElementObj;
            RectTransform elementRt = elementObj.GetComponent<RectTransform>();

            // set Scale of Element
            ObjLocation objLoc = m_HUDInfo.m_lElementLoc[index];
            Vector3 elementScale = elementRt.localScale;
            if (objLoc.m_ScaleSameByY != 0)
            {
                elementScale = Vector3.one * ((objLoc.m_ScaleSameByY * screenSize.y) / elementRt.sizeDelta.y);
            }
            else
            {
                elementScale.x = (objLoc.m_Rect.width * screenSize.x) / elementRt.sizeDelta.x;
                elementScale.y = (objLoc.m_Rect.height * screenSize.y) / elementRt.sizeDelta.y;
            }
            elementScale.z = 1.0f;
            elementRt.localScale = elementScale;

            // set POS of Element
            Rect objRect = objLoc.m_Rect;
            Vector3 elementPos = elementRt.position;
            elementPos.x = (objLoc.m_Rect.x + (objLoc.m_Rect.width / 2.0f)) * screenSize.x;
            elementPos.y = (objLoc.m_Rect.y - (objLoc.m_Rect.height / 2.0f)) * screenSize.y;
            elementRt.position = elementPos;
        }
    }
    #endregion
}
