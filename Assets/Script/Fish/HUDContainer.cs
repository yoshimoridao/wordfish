using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDContainer : GObject//BaseMono 
{
    public Stack<HUDElement> m_StackHUDElements = new Stack<HUDElement>();

    // ============================== PUBLIC FUNC ==============================
    public virtual void OnHUDContCreate(SceneInfo HUDInfo)
    {
        SetingUpPosOfElement(HUDInfo.m_lElementLoc);
    }
    public virtual void OnHUDContCreate() { }
    public virtual void OnHUDContDestroy()
    {
        // remove all of elements
        while (m_StackHUDElements.Count > 0)
        {
            HUDElement hudElement = m_StackHUDElements.Pop();
            hudElement.OnHUDDestroy();
            Destroy(hudElement.gameObject);
        }
    }

    /// <summary>
    /// Generate new HUD Element & push it into Stack container
    /// </summary>
    /// <param name="hudElement"></param>
    /// <returns></returns>
    public HUDElement GenHUDElements(HUDElement hudElement) 
    {
        HUDElement genHUDElement = Instantiate(hudElement.gameObject, transform).GetComponent<HUDElement>();
        genHUDElement.OnHUDCreate(this);
        m_StackHUDElements.Push(genHUDElement);
        return genHUDElement;
    }
    /// <summary>
    /// Push HUD Element into Stack container
    /// </summary>
    /// <param name="hudElement"></param>
    public void PushHUDElements(HUDElement hudElement)
    {
        hudElement.OnHUDCreate(this);
        m_StackHUDElements.Push(hudElement);
    }
    /// <summary>
    /// Pop HUD Element out of Stack container
    /// </summary>
    public void PopHUDElements()
    {
        if (m_StackHUDElements.Count > 0)
        {
            HUDElement hudElement = m_StackHUDElements.Pop();
            hudElement.OnHUDDestroy();
        }
    }

    // ============================== PRIVATE FUNC ==============================
    private void SetingUpPosOfElement(List<ObjLocation> lElementInfo)
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        
        for (int i = 0; i < lElementInfo.Count; i++)
        {
            ObjLocation elementRect = lElementInfo[i];
            for (int j = 0; j < transform.childCount; j++)
            {
                GameObject elementObj = transform.GetChild(j).gameObject;
                if (elementObj.name == elementRect.m_ObjName)
                {
                    // set scale for element
                    Vector2 hudSize = new Vector2(elementRect.m_Rect.width * screenSize.x, elementRect.m_Rect.height * screenSize.y);
                    RectTransform rt = elementObj.GetComponent<RectTransform>();
                    rt.sizeDelta = hudSize;

                    // set position for element
                    rt.position = new Vector3((elementRect.m_Rect.x * screenSize.x) + rt.sizeDelta.x / 2.0f, 
                        (elementRect.m_Rect.y * screenSize.y) - rt.sizeDelta.y / 2.0f, 0.0f);
                    break;
                }
            }
        }
    }
}
