using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDAvaFishCont : GHUDElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    [SerializeField]
    private Image Ref_FishPlayerAva;
    [SerializeField]
    private Image Ref_HitBall;
    [SerializeField]
    private Slider Ref_FishHP;

    // private vars
    private bool m_isActiveHitAnim;
    private float m_TargetHp;
    [SerializeField]
    private float m_LoseHpSpeed = 50.0f;
    [SerializeField]
    private Entity m_HitBallEntity = new Entity();
    private FishInfo m_PlayerFishInfo;
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void Init(FishInfo a_PlayerFishInfo)
    {
        m_PlayerFishInfo = a_PlayerFishInfo;
        Ref_FishPlayerAva.sprite = Fish.GetFishCard(m_PlayerFishInfo.m_FishID);
        // init hit ball entity
        m_HitBallEntity.Init(Ref_HitBall.GetComponent<Image>());
        // update Hp bar
        m_TargetHp = m_PlayerFishInfo.m_FishHP.m_CurrentHP = m_PlayerFishInfo.m_FishHP.m_MaxHP;
        Ref_FishHP.value = m_PlayerFishInfo.m_FishHP.m_CurrentHP / m_PlayerFishInfo.m_FishHP.m_MaxHP;
    }

    public void OnHitPlayerFish(string a_BallBossId, float a_HitDamage)
    {
        m_TargetHp -= a_HitDamage;
        // active ball anim
        ActiveBallAnim(a_BallBossId);
    }

    public void OnEventProcessEndAnim()
    {
        // invisible hit ball sprite
        m_isActiveHitAnim = true;
        Ref_HitBall.gameObject.SetActive(false);
        m_HitBallEntity.m_delEndAnim -= OnEventProcessEndAnim;
    }

    // =========== OVERRIDE func ===========
    #region OVERRIDE func
    // override Obj
    public override void OnCreateObj()
    {
        base.OnCreateObj();
    }

    public override void OnUpdateObj(float a_dt)
    {
        base.OnUpdateObj(a_dt);

        UpdateFishHP(a_dt);
        if (m_isActiveHitAnim)
            m_HitBallEntity.UpdateEntity(a_dt);
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
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void UpdateFishHP(float a_dt)
    {
        // update Lose hp
        if (m_PlayerFishInfo.m_FishHP.m_CurrentHP > m_TargetHp)
        {
            m_PlayerFishInfo.m_FishHP.m_CurrentHP -= m_LoseHpSpeed * a_dt;
            if (m_PlayerFishInfo.m_FishHP.m_CurrentHP <= m_TargetHp)
            {
                m_PlayerFishInfo.m_FishHP.m_CurrentHP = m_TargetHp;
            }
            // update Hp bar
            Ref_FishHP.value = m_PlayerFishInfo.m_FishHP.m_CurrentHP / m_PlayerFishInfo.m_FishHP.m_MaxHP;
            if (m_PlayerFishInfo.m_FishHP.m_CurrentHP <= 0)
            {
                m_PlayerFishInfo.m_FishHP.m_CurrentHP = 0;

                // back to tank
                SceneMgr sceneMgr = SceneMgr.s_Instance;
                StoryGameBossMgr curScene = sceneMgr.GetCurScene() as StoryGameBossMgr;
                curScene.OnBackToTank();
            }
        }
    }

    /// <summary>
    /// Active anim ball hit of Boss Fish to Player's fish
    /// </summary>
    /// <param name="a_BallBossId"></param>
    private void ActiveBallAnim(string a_BallBossId)
    {
        m_isActiveHitAnim = true;
        // visible hit ball sprite
        Ref_HitBall.gameObject.SetActive(true);

        if (!m_HitBallEntity.IsLoadedSprites())
        {
            string ballPath = AssetPathConstant.FOLDER_FISH_PATH + "/" + a_BallBossId + "/" + "ball";
            m_HitBallEntity.LoadSprites(ballPath);
        }
        else
        {
            m_HitBallEntity.ResetTimeFrame();
        }

        // add trigger event delegate
        m_HitBallEntity.m_delEndAnim += OnEventProcessEndAnim;
    }
    #endregion
}
