using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HUDStoryHighlightNode : GHUDElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // public vars
    public List<Sprite> m_ListLockedAnimSprites = new List<Sprite>();
    public List<Sprite> m_ListUnlockedAnimSprites = new List<Sprite>();
    public List<Sprite> m_ListBossAnimSprites = new List<Sprite>();
    public List<Sprite> m_ListUnlockedBossAnimSprites = new List<Sprite>();
    // private vars
    private HUDStoryMapMgr.NodeState m_NodeState;
    private GameObject m_ParentNode;
    private Vector2 m_Frame = Vector2.zero;
    [SerializeField]
    private Vector2 m_Dt = new Vector2(0, 0.15f);
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

        OnUpdate(a_dt);
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void ShowHighlight(GameObject nodeObj, GameObject mapObj)
    {
        // enable SPRITE of old parent node
        if (m_ParentNode)
            m_ParentNode.transform.GetChild(0).GetComponent<Image>().enabled = true; // p/s: get image obj of node
        m_ParentNode = nodeObj;

        gameObject.SetActive(true);
        // get NODE STATE following name of parent SPRITE
        Image parentImg = m_ParentNode.transform.GetChild(0).GetComponent<Image>(); // p/s: get image obj of node
        m_NodeState = (HUDStoryMapMgr.NodeState)int.Parse(parentImg.sprite.name);
        // disable SPRITE of curr parent node
        parentImg.enabled = false;

        // set POS & SCALE of hl
        RectTransform rt = GetComponent<RectTransform>();
        rt.position = m_ParentNode.GetComponent<RectTransform>().position;
        rt.localScale = mapObj.GetComponent<RectTransform>().localScale;

        // set FRAME
        List<Sprite> l_sprite = new List<Sprite>();
        switch (m_NodeState)
        {
            case HUDStoryMapMgr.NodeState.Unlocked:
                l_sprite = m_ListUnlockedAnimSprites;
                break;
            case HUDStoryMapMgr.NodeState.Locked:
                l_sprite = m_ListLockedAnimSprites;
                break;
            case HUDStoryMapMgr.NodeState.LockedBoss:
                l_sprite = m_ListBossAnimSprites;
                break;
            case HUDStoryMapMgr.NodeState.UnlockedBoss:
                l_sprite = m_ListUnlockedBossAnimSprites;
                break;
        }
        m_Frame = new Vector2(0, l_sprite.Count);
        SetSpriteForHl();
    }

    public void HideHighlight()
    {
        gameObject.SetActive(false);
        // enable SPRITE of parent node
        if (m_ParentNode)
            m_ParentNode.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void OnUpdate(float a_dt)
    {
        m_Dt.x += a_dt;
        if (m_Dt.x >= m_Dt.y)
        {
            m_Dt.x = 0;
            m_Frame.x++;
            if (m_Frame.x >= m_Frame.y)
                m_Frame.x = 0;
            SetSpriteForHl();
        }
    }

    private void SetSpriteForHl()
    {
        List<Sprite> l_sprite = new List<Sprite>();
        switch (m_NodeState)
        {
            case HUDStoryMapMgr.NodeState.Unlocked:
                l_sprite = m_ListUnlockedAnimSprites;
                break;
            case HUDStoryMapMgr.NodeState.Locked:
                l_sprite = m_ListLockedAnimSprites;
                break;
            case HUDStoryMapMgr.NodeState.LockedBoss:
                l_sprite = m_ListBossAnimSprites;
                break;
            case HUDStoryMapMgr.NodeState.UnlockedBoss:
                l_sprite = m_ListUnlockedBossAnimSprites;
                break;
        }

        if ((int)m_Frame.x < l_sprite.Count)
        {
            Sprite hlSprite = l_sprite[(int)m_Frame.x];
            Image img = GetComponent<Image>();
            // set SPRITE for hl
            img.sprite = hlSprite;
            // set SIZE DELTA for hl
            RectTransform rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
        }
    }
    #endregion
}
