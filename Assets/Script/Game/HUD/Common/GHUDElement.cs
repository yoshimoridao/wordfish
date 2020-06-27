using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GHUDElement : GObject
{
    // ================================== VARIABLES ==================================
    #region Vars
    // protected vars
    protected GHUD Ref_GHUD;
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

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public virtual void Init(GHUD a_HUD)
    {
        Ref_GHUD = a_HUD;
    }
    #endregion
}
