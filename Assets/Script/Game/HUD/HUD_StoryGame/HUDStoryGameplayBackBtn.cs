using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUDStoryGameplayBackBtn : GHUDElement
{
    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
    public override void OnCreateObj()
    {
        base.OnCreateObj();

        Button btn = GetComponent<Button>();
        if (btn)
        {
            btn.onClick.AddListener(() => OnPressedBtn());
        }
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
    public void OnPressedBtn()
    {
        UtilityClass.ResetSelectedGameObject();
        // back to TANK
        SceneMgr.s_Instance.ChangeScene(SceneMgr.SceneType.Tank);
    }
    #endregion
}
