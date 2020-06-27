using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBossCont : GHUDElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    [SerializeField]
    private Image Ref_BossAva;
    [SerializeField]
    private Slider Ref_BossTime;
    [SerializeField]
    private Slider Ref_BossHP;
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

    // Override GElement
    public override void Init(GHUD a_HUD)
    {
        base.Init(a_HUD);
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void SetBossAva(string a_BossID)
    {
        Ref_BossAva.sprite = Fish.GetFishCard(a_BossID);
    }

    public void UpdateBossHP(Vector2 a_HP)
    {
        Ref_BossHP.value = a_HP.x / a_HP.y;
    }

    public void UpdateBossTime(Vector2 a_Time)
    {
        Ref_BossTime.value = a_Time.x / a_Time.y;
    }
    #endregion
}
