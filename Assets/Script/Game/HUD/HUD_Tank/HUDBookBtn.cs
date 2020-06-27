using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBookBtn : GHUDElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // private vars
    private GHUD m_Book = null;
    #endregion

    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
    public override void OnCreateObj()
    {
        base.OnCreateObj();

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => OnButtonClick());
    }

    public override void OnUpdateObj(float a_dt)
    {
        base.OnUpdateObj(a_dt);
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }

    public override void Init(GHUD a_HUD)
    {
        base.Init(a_HUD);
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void OnButtonClick()
    {
        // show || hide Book
        if (m_Book)
        {
            m_Book.SetActive(!m_Book.IsActive());
        }
        // gen Book HUD
        else
        {
            // instantiate vs parent transform is CANVAS
            m_Book = HUDMgr.s_Instance.GenHUD(HUDMgr.HUDType.HUD_Book);
        }

        // disable STORY MAP in case it opened
        if (m_Book.IsActive())
        {
            if (HUDMgr.s_Instance.IsGHUDAvailable(HUDMgr.HUDType.HUD_StoryMap))
            {
                HUDMgr.s_Instance.DisableGHUD(HUDMgr.HUDType.HUD_StoryMap);
            }
        }
    }
    #endregion
}
