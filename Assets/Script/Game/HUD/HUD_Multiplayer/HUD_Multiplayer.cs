using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Multiplayer : GHUD 
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    [SerializeField]
    private HUDStoryGameplayBackBtn Ref_BackBtn;
    [SerializeField]
    private HUDStoryDescriptionBalloon Ref_DescriptionBalloon;
    [SerializeField]
    private HUDAvaFishCont Ref_OpponentCont;
    [SerializeField]
    private HUDAvaFishCont Ref_PlayerCont;
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
        AddHUDElement(Ref_OpponentCont);
        AddHUDElement(Ref_PlayerCont);
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

    //// Player Cont
    //public void InitPlayerContainer(FishInfo a_PlayerFishInfo)
    //{
    //    Ref_PlayerCont.Init(a_PlayerFishInfo);
    //}

    //public void OnHitPlayerFish(string a_BallBossId, float a_HitDamage)
    //{
    //    Ref_PlayerCont.OnHitPlayerFish(a_BallBossId, a_HitDamage);
    //}
    #endregion
}
