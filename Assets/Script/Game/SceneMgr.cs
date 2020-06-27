using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : GObject//BaseMono
{
    // ================================== VARIABLES ==================================
    #region Vars
    // enum vars
    public enum SceneType { None, Tank, StoryGame, StoryBossGame, Multiplayer };
    // reference vars
    private HUDMgr Ref_HUDMgr;
    // public vars
    public static SceneMgr s_Instance;

    // private vars
    private SceneType m_NextSceneType = SceneType.None;
    private GScene m_CurGScene = null;
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region UNITY funcs
    private void Awake()
    {
        s_Instance = this;
    }

    private void Start()
    {
        Ref_HUDMgr = HUDMgr.s_Instance;
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void ChangeScene(SceneType a_sceneType)
    {
        m_NextSceneType = a_sceneType;
    }

    public GScene GetCurScene()
    {
        return m_CurGScene;
    }

    // ======== GAME CYCLE FUNCS ========
    #region GAME CYCLE funcs
    public void Init()
    {

    }

    public void OnUpdate(float a_dt)
    {
        if (m_CurGScene)
            m_CurGScene.OnUpdateObj(a_dt);
    }

    public void OnLateUpdate(float a_dt)
    {
        // change to next scene
        if (m_NextSceneType != SceneType.None)
        {
            if (GenScene(m_NextSceneType))
            {
                m_NextSceneType = SceneType.None;
            }
            else
            {
                Debug.LogError("Init Scene Error");
            }
        }
    }
    #endregion
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private bool GenScene(SceneType a_sceneType)
    {
        SceneInfo sceneInfo = DbMgr.s_Instance.GetSceneInfo(a_sceneType);
        if (sceneInfo == null)
            return false;

        GameObject scenePref = Resources.Load<GameObject>(sceneInfo.m_PrefPath);
        if (scenePref == null)
            return false;

        // destroy old scene (if had)
        if (m_CurGScene)
            m_CurGScene.OnDestroyObj();

        // instantiate & init new scene
        GameObject sceneObj = Instantiate(scenePref, transform);
        m_CurGScene = sceneObj.GetComponent<GScene>();
        m_CurGScene.OnCreateObj();

        // add HUD
        GHUD gHUD = GenHUD(sceneInfo);
        m_CurGScene.Init(sceneInfo, gHUD);

        return true;
    }

    private GHUD GenHUD(SceneInfo sceneInfo)
    {
        HUDMgr.HUDType hudType = HUDMgr.HUDType.None;
        bool isClearOldHUD = true;
        switch (sceneInfo.m_Type)
        {
            case SceneType.Tank: 
                hudType = HUDMgr.HUDType.HUD_Tank;
                break;
            case SceneType.StoryGame:
                hudType = HUDMgr.HUDType.HUD_StoryGame;
                break;
            case SceneType.StoryBossGame:
                hudType = HUDMgr.HUDType.HUD_StoryBoss;
                break;
            //case SceneType.Multiplayer:
            //    hudType = HUDMgr.HUDType.HUD_Multiplayer;
            //    break;
        }

        if (hudType != HUDMgr.HUDType.None)
            return Ref_HUDMgr.GenHUD(hudType, isClearOldHUD);
        return null;
    }
    #endregion
}
