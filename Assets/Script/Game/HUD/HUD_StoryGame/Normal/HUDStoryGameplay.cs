using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDStoryGameplay : GHUD 
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    [SerializeField]
    private HUDStoryGameplayBackBtn Ref_BackBtn;
    [SerializeField]
    private HUDStoryDescriptionBalloon Ref_DescriptionBalloon;
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

    public override void Init(HUDInfo a_HUDInfo)
    {
        base.Init(a_HUDInfo);

        AddHUDElement(Ref_BackBtn);
        AddHUDElement(Ref_DescriptionBalloon);
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    // Description Balloon
    public void ShowDescriptionBalloon(string a_Description)
    {
        Ref_DescriptionBalloon.ShowDescription(a_Description);
    }

    public void HideDescriptionBalloon()
    {
        Ref_DescriptionBalloon.HideDescription();
    }
    #endregion
}
