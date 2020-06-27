using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPComponent : HUDElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // static vars
    // reference vars
    // prefab vars
    // const vars
    // public vars
    public Slider RefHPSlider;
    public Text RefHPTime;

    // private vars
    #region HP Reference
    private RectTransform m_Parent;
    private Transform m_FollowObject;
    #endregion

    #region Param for HP
    private float m_OffsetY = 0.5f;
    private Vector3 m_Target = Vector3.zero;
    #endregion

    #region Fields
    private bool m_IsAvailable = true;
    private RectTransform m_CacheRect;
    #endregion

    #endregion

    // ================================== PROPERTIES ==================================
    #region Properties
    public bool PAvailable
    {
        get
        {
            return m_IsAvailable;
        }
        set
        {
            m_IsAvailable = value;
        }
    }

    public float OffsetY
    {
        get
        {
            return m_OffsetY;
        }
        set
        {
            m_OffsetY = value;
        }
    }
    #endregion 

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void SetFollowObject(Transform obj)
    {
        m_FollowObject = obj;
    }

    public void SetParent(RectTransform parent)
    {
        m_Parent = parent;
    }

    //    public void SetActiveHP (bool active)
    //    {
    //        gameObject.SetActive(active);
    //    }

    public void UpdatePosition()
    {
        if (m_CacheRect == null)
            m_CacheRect = GetComponent<RectTransform>();

        if (m_FollowObject != null)
        {
            m_Target.x = m_FollowObject.position.x;// +m_Offset;
            m_Target.y = m_FollowObject.position.y + m_OffsetY;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(m_Target);

            if (m_CacheRect != null)
            {
                m_CacheRect.transform.position = screenPos;
            }

            //Vector2 mov;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Parent, screenPos, Camera.main, out mov);
            //transform.position = m_Parent.TransformPoint(mov);
        }

    }
    #endregion
}
