using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMultiplayerBtn : GHUDElement
{
    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    // ========= OVERRIDE func =========
    #region OVERRIDE func
    public override void OnCreateObj()
    {
        base.OnCreateObj();

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => OnButtonClick());
    }
    #endregion

    public void OnButtonClick()
    {
        // start MULTIPLAYER scene
        GameMgr.s_Instance.ChangeScene(SceneMgr.SceneType.Multiplayer);
    }
    #endregion
}
