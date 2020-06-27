using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GElement : GObject
{
    // ================================== VARIABLES ==================================
    #region Vars
    // protected vars
    protected GScene Ref_GScene;
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    // =========== OVERRIDE func ===========
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

    public virtual void Init(GScene a_Scene)
    {
        Ref_GScene = a_Scene;
    }
    #endregion
}
