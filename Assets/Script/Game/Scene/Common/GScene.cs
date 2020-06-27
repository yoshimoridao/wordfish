using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GScene : GObject
{
    // ================================== VARIABLES ==================================
    #region Vars
    // protected vars
    /// <summary>
    /// list contains objs of scene
    /// </summary>
    protected List<GElement> m_lGElements = new List<GElement>();
    // protected vars
    protected GHUD Ref_GHUD;
    // private vars
    private SceneInfo m_SceneInfo;
    #endregion

    // ================================== PROPERTIES ==================================
    #region Properties
    public SceneInfo PSceneInfo
    {
        get { return m_SceneInfo; }
    }
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

        for (int i = 0; i < m_lGElements.Count; i++)
        {
            GElement gElement = m_lGElements[i];
            gElement.OnUpdateObj(a_dt);
        }
    }

    public override void OnDestroyObj()
    {
        for (int i = 0; i < m_lGElements.Count; i++)
        {
            RemoveGElement(i);
        }

        base.OnDestroyObj();
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public virtual void Init(SceneInfo a_SceneInfo, GHUD a_GHUD)
    {
        m_SceneInfo = a_SceneInfo;
        Ref_GHUD = a_GHUD;
    }
    #endregion

    // ================================== PROTECTED FUNCS ==================================
    #region Protected Funcs
    protected void AddGElement(GElement a_gElement)
    {
        // init Element
        a_gElement.OnCreateObj();
        a_gElement.Init(this);
        m_lGElements.Add(a_gElement);

        // set pos for element
        SetLocationForElement(a_gElement);
    }

    protected void RemoveGElement(GElement a_gElement)
    {
        int elementIndex = m_lGElements.FindIndex(x => x == a_gElement);
        RemoveGElement(elementIndex);
    }

    protected void RemoveGElement(int elementIndex)
    {
        if (elementIndex != -1)
        {
            GElement element = m_lGElements[elementIndex];
            element.OnDestroyObj();
            m_lGElements.RemoveAt(elementIndex);
        }
    }

    protected void SetLocationForElement(GElement gElement)
    {
        // detect element following its name
        int index = m_SceneInfo.m_lElementLoc.FindIndex(x => x.m_ObjName == gElement.gameObject.name);
        if (index != -1)
        {
            Rect camRect = CameraController.GetCamRectInEditor();

            GameObject elementObj = gElement.gameObject;
            SpriteRenderer elementSr = elementObj.GetComponent<SpriteRenderer>();

            // set SIZE of Element
            ObjLocation objLoc = m_SceneInfo.m_lElementLoc[index];
            Vector3 elementScale = elementObj.transform.localScale;
            if (objLoc.m_ScaleSameByY != 0)
            {
                float scale = (camRect.height * objLoc.m_ScaleSameByY * 100.0f) / elementSr.sprite.rect.height;
                elementScale = new Vector3(scale, scale, 1.0f);
            }
            else
            {
                elementScale = new Vector3(camRect.width * objLoc.m_Rect.width, camRect.height * objLoc.m_Rect.height, 1.0f);
            }
            elementObj.transform.localScale = elementScale;

            Vector2 topleftCamPos = CameraController.s_Instance.GetTopLeftCamPos();
            // set POS of Element
            Rect objRect = objLoc.m_Rect;
            Vector3 elementPos = elementObj.transform.position;
            elementPos.x = topleftCamPos.x + ((objLoc.m_Rect.x * camRect.width) + elementSr.bounds.extents.x);
            elementPos.y = topleftCamPos.y - ((objLoc.m_Rect.y * camRect.height) + elementSr.bounds.extents.y);
            elementSr.transform.position = elementPos;
        }
    }
    #endregion
}
