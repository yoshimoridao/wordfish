using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Entity
{
    // ================================== VARIABLES ==================================
    #region Vars
    // public vars
    public delegate void EventProcessEndAnim();
    public EventProcessEndAnim m_delEndAnim;
    // protected vars
    [SerializeField]
    protected Vector2 m_Time = new Vector2(0, 0.2f);
    protected Vector2 m_Frame;
    protected Sprite[] m_aSprites;
    protected SpriteRenderer m_Sr;
    protected Image m_Img;
    #endregion

    // ================================== PROPERTIES ==================================
    #region Properties
    public Sprite[] PArrSprites
    {
        get { return m_aSprites; }
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public virtual void Init(SpriteRenderer a_sr)
    {
        m_Sr = a_sr;
    }
    public virtual void Init(Image a_Img)
    {
        m_Img = a_Img;
    }

    public void LoadSprites(Sprite[] a_Sprites)
    {
        m_aSprites = a_Sprites;
        if (m_aSprites.Length > 0)
        {
            UtilityClass.SortSpriteNameAscending sortAscend = new UtilityClass.SortSpriteNameAscending();
            Array.Sort(m_aSprites, sortAscend);
        }
        m_Frame = new Vector2(0, m_aSprites.Length);
        UpdateSprite();
    }
    public void LoadSprites(string a_path)
    {
        // load sprites
        Sprite[] arrSprites = Resources.LoadAll<Sprite>(a_path);
        if (arrSprites.Length > 0)
        {
            m_aSprites = arrSprites;
            m_Frame = new Vector2(0, m_aSprites.Length);
            UpdateSprite();
        }
    }

    public void UpdateEntity(float a_dt)
    {
        if (!IsLoadedSprites())
            return;

        m_Time.x += a_dt;
        if (m_Time.x >= m_Time.y)
        {
            m_Time.x = 0;
            m_Frame.x++;
            // end anim
            if (m_Frame.x >= m_Frame.y)
            {
                m_Frame.x = 0;

                // call event func
                if (m_delEndAnim != null)
                    m_delEndAnim();
            }
            UpdateSprite();
        }
    }

    public virtual bool IsLoadedSprites()
    {
        if (m_aSprites == null)
            return false;

        return (m_aSprites.Length > 0);
    }

    public void ResetTimeFrame()
    {
        m_Time.x = 0;
        m_Frame.x = 0;
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void UpdateSprite()
    {
        if (m_Sr)
            m_Sr.sprite = m_aSprites[(int)m_Frame.x];
        else if (m_Img)
            m_Img.sprite = m_aSprites[(int)m_Frame.x];
    }
    #endregion
}
