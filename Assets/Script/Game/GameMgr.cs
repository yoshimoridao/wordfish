using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    // ================================== VARIABLES ==================================
    #region Vars
    // enum vars
    public enum GamePhase { None, Loading, Playing, Pause, Quit }
    // reference vars
    private DbMgr Ref_DbMgr;
    private SceneMgr Ref_SceneMgr;
    private HUDMgr Ref_HUDMgr;

    // public vars
    public static GameMgr s_Instance;
    // private vars
    private GamePhase m_CurGPhase = GamePhase.None;
    private GamePhase m_NextGPhase = GamePhase.None;
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region UNITY funcs
    private void Awake()
    {
        s_Instance = this;
    }

    private void Start()
    {
        Ref_DbMgr = DbMgr.s_Instance;
        Ref_SceneMgr = SceneMgr.s_Instance;
        Ref_HUDMgr = HUDMgr.s_Instance;

        // default loading Game
        ChangeGamePhase(GamePhase.Loading);
    }

    private void Update()
    {
        OnGUpdate(Time.deltaTime);
    }

    private void LateUpdate()
    {
        OnGLateUpdate(Time.deltaTime);
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void ChangeGamePhase(GamePhase a_GPhase)
    {
        m_NextGPhase = a_GPhase;
    }

    public void ChangeScene(SceneMgr.SceneType a_SceneType)
    {
        Ref_SceneMgr.ChangeScene(a_SceneType);
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    // ===== GAME CYCLE FUNCS =====
    #region GAME CYCLE funcs
    private void OnGLoading()
    {
        Ref_DbMgr.Init();
        Ref_SceneMgr.Init();
        Ref_HUDMgr.Init();

        // change to Playing if Game'd loaded done
        m_CurGPhase = GamePhase.Playing;
        Ref_SceneMgr.ChangeScene(SceneMgr.SceneType.Tank);
    }

    private void OnGUpdate(float a_dt)
    {
        Ref_SceneMgr.OnUpdate(a_dt);
        Ref_HUDMgr.OnUpdate(a_dt);
    }

    private void OnGLateUpdate(float a_dt)
    {
        // change phase of Game
        if (m_NextGPhase != GamePhase.None)
        {
            OnChangeGamePhase();
        }
        else
        {
            Ref_SceneMgr.OnLateUpdate(a_dt);
        }
    }

    private void OnGPause()
    {

    }

    private void OnGQuit()
    {

    }
    #endregion

    private void OnChangeGamePhase()
    {
        m_CurGPhase = m_NextGPhase;
        m_NextGPhase = GamePhase.None;
        switch (m_CurGPhase)
        {
            case GamePhase.Loading:
                OnGLoading();
                break;
            case GamePhase.Pause:
                OnGPause();
                break;
            case GamePhase.Playing:
                break;
            case GamePhase.Quit:
                OnGQuit();
                break;
        }
    }
    #endregion
}
