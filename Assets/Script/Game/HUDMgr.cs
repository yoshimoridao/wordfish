using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMgr : GObject
{
    // ================================== VARIABLES ==================================
    #region Vars
    // public vars
    public static HUDMgr s_Instance;
    public enum HUDType { None, HUD_Tank, HUD_StoryMap, HUD_StoryGame, HUD_StoryBoss, HUD_Multiplayer, HUD_Book };

    // private vars
    private List<GHUD> m_lHUD = new List<GHUD>();
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region UNITY funcs
    private void Awake()
    {
        s_Instance = this;
    }
    #endregion

    // ================================== GAME CYCLE FUNCS ==================================
    #region GAME CYCLE funcs
    public void Init()
    {

    }

    public void OnUpdate(float a_dt)
    {
        for (int i = 0; i < m_lHUD.Count; i++)
        {
            GHUD gHUD = m_lHUD[i];
            gHUD.OnUpdateObj(a_dt);
        }
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public GHUD GenHUD(HUDType a_HUDType, bool a_IsClearOldHUD = false)
    {
        if (a_IsClearOldHUD)
        {
            for (int i = 0; i < m_lHUD.Count; i++)
            {
                DestroyHUD(i);
                i--;
            }
        }
        return GenHUD(a_HUDType);
    }

    public bool IsGHUDAvailable(HUDType a_HUDType)
    {
        GHUD ghud = GetHUD(a_HUDType);
        return ghud != null ? true : false;
    }

    public void DisableGHUD(HUDType a_HUDType)
    {
        GHUD ghud = GetHUD(a_HUDType);
        if (ghud != null)
        {
            ghud.SetActive(false);
        }
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private GHUD GetHUD(HUDType a_HUDType)
    {
        for (int i = 0; i < m_lHUD.Count; i++)
        {
            GHUD ghud = m_lHUD[i];
            if (ghud.GetHUDInfo().m_HUDType == a_HUDType)
                return ghud;
        }
        return null;
    }
    private GHUD GenHUD(HUDType a_HUDType)
    {
        HUDInfo hudInfo = DbMgr.s_Instance.GetHUDInfo(a_HUDType);
        if (hudInfo == null)
            return null;

        GameObject hudPref = Resources.Load<GameObject>(hudInfo.m_PrefPath);
        if (hudPref == null)
            return null;

        // instantiate & init new HUD
        GameObject hudObj = Instantiate(hudPref, transform);
        GHUD gHUD = hudObj.GetComponent<GHUD>();
        gHUD.OnCreateObj();
        gHUD.Init(hudInfo);
        m_lHUD.Add(gHUD);

        return gHUD;
    }

    private void DestroyHUD(int a_Index)
    {
        if (a_Index >= m_lHUD.Count)
            return;

        GHUD gHUD = m_lHUD[a_Index];
        gHUD.OnDestroyObj();
        m_lHUD.RemoveAt(a_Index);
    }
    #endregion
}
