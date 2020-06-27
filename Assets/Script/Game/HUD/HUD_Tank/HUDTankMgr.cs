using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTankMgr : GHUD
{
    // ================================== VARIABLES ==================================
    public enum UITypes
    {
        None,
        HPComponent,
        All
    }

    #region Vars
    // reference vars
    public GHUDElement Ref_StoryMapBtn;
    public GHUDElement Ref_BookBtn;
    public GHUDElement Ref_MultiplayerBtn;
    // prefab vars
    public GameObject Pre_HPComponent;
    // private vars
    private int m_HPQuota = 10;
    private bool m_IsInit = false;
    private List<HPComponent> m_HPComponents;
    private RectTransform m_CacheRect;
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region Unity funcs
    private void OnEnable()
    {
        Fish.EventProcessUI += GetUIController;
    }

    private void OnDisable()
    {
        Fish.EventProcessUI -= GetUIController;
    }
    #endregion

    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
    // override GObj
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
        if (m_HPComponents != null && m_HPComponents.Count > 0)
        {
            for (int i = 0; i < m_HPComponents.Count; ++i)
            {
                m_HPComponents[i].SelfDestroy();
            }
            m_HPComponents.Clear();
        }

        base.OnDestroyObj();
    }

    public override void Init(HUDInfo a_HUDInfo)
    {
        base.Init(a_HUDInfo);
        Init();
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void GetUIController(Transform followThis, HUDTankMgr.UITypes type, out MonoBehaviour controller)
    {
        controller = null;

        switch (type)
        {
            case UITypes.HPComponent:
                controller = GetHPComponent(followThis);
                break;
        }
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void Init()
    {
        if (m_CacheRect == null)
            m_CacheRect = GetComponent<RectTransform>();
        if (!m_IsInit)
        {
            if (Pre_HPComponent != null)
            {
                m_IsInit = true;

                if (m_HPComponents == null)
                    m_HPComponents = new List<HPComponent>();

                StartCoroutine(CreateListHPComponents());
            }
        }
        else
        {
            if (m_HPComponents != null)
            {
                foreach (var hp in m_HPComponents)
                    if (!hp.IsActive() && hp.PAvailable)
                        hp.SetActive(true);
            }
        }

        if (Ref_BookBtn)
            base.AddHUDElement(Ref_BookBtn);
        if (Ref_StoryMapBtn)
            base.AddHUDElement(Ref_StoryMapBtn);
        if (Ref_MultiplayerBtn)
            base.AddHUDElement(Ref_MultiplayerBtn);
    }

    private HPComponent GetHPComponent(Transform followThis)
    {
        if (m_HPComponents == null)
            return null;

        foreach (var component in m_HPComponents)
            if (component.PAvailable)
            {
                component.PAvailable = false;
                component.SetFollowObject(followThis);
                //component.SetActiveHP(true);
                return component;
            }

        return null;
    }

    private IEnumerator CreateListHPComponents()
    {
        for (int i = 0; i < m_HPQuota; ++i)
        {
            GameObject obj = Instantiate(Pre_HPComponent, transform);
            obj.SetActive(false);
            HPComponent hp = obj.GetComponent<HPComponent>();
            if (hp != null)
            {
                hp.SetParent(m_CacheRect);
                m_HPComponents.Add(hp);
            }
            yield return null;
        }
    }
    #endregion
}
