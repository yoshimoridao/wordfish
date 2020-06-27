using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDStoryDescriptionBalloon : GHUDElement 
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    [SerializeField]
    private Text Ref_Text;
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
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }
    #endregion

    // =================================== PUBLIC FUNC ===================================
    #region Public Funcs
    public void ShowDescription(string a_Description)
    {
        if (!Ref_Text)
            return;

        if (!IsActive())
            SetActive(true);
        Ref_Text.text = a_Description;
    }

    public void HideDescription()
    {
        SetActive(false);
    }
    #endregion
}
