using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBossFish : GElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // private vars
    private int m_MvmDirection = 1;
    [SerializeField]
    private float m_MvmSpeedY = 5.0f;
    [SerializeField]
    private float m_BossAtkTimeDesign = 7; // default = 7s
    [SerializeField]
    private float m_LoseHpSpeed = 50.0f;
    private float m_TargetHp; // anim lost hp
    // time to Boss attack
    private Vector2 m_BossAtkTime;
    [SerializeField]
    private Vector2 m_MvmRangeYWorldUnit = new Vector2(1.0f, 2.0f);
    private Vector2 m_MvmRangeYCoord = Vector2.zero;
    private FishInfo m_BossInfo;
    [SerializeField]
    private FishSpritesMgr m_FishSpritesMgr = new FishSpritesMgr();
    #endregion

    // =================================== OVERRIDE func ===================================
    #region Override func
    // Object Override
    public override void OnCreateObj()
    {
        base.OnCreateObj();
    }

    public override void OnUpdateObj(float a_dt)
    {
        base.OnUpdateObj(a_dt);

        // update ENTITY
        m_FishSpritesMgr.UpdateEntity(a_dt);
        // update Boss Hp
        UpdateBossHP(a_dt);

        if (m_BossInfo.m_CurFishState == Fish.FishState.Move)
        {
            UpdatePosition(a_dt);
            // update Boss Time
            UpdateBossTime(a_dt);
        }
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }

    // GElement Override
    public override void Init(GScene a_Scene)
    {
        base.Init(a_Scene);

        // get Fish info
        string fishIndex = (Ref_GScene as StoryGameBossMgr).PCurNodeInfo.m_FishIndex;
        m_BossInfo = DbMgr.s_Instance.GetFishInfo(fishIndex);

        // set Avata off Boss
        (Ref_GScene as StoryGameBossMgr).SetBossAva(fishIndex);
        // set Atk time of Boss
        m_BossAtkTime = Vector2.one * m_BossAtkTimeDesign;
        // set Hp default of Boss
        m_TargetHp = m_BossInfo.m_FishHP.m_CurrentHP = m_BossInfo.m_FishHP.m_MaxHP;
        // update Hp bar
        Vector2 bossHP = new Vector2(m_BossInfo.m_FishHP.m_CurrentHP, m_BossInfo.m_FishHP.m_MaxHP);
        (Ref_GScene as StoryGameBossMgr).UpdateBossHP(bossHP);

        // set State default of Boss = Move
        UpdateBossState(Fish.FishState.Move);

        // set Movement range
        Vector2 camSize = CameraController.s_Instance.GetWorldUnitSize();
        Vector2 topCamPos = CameraController.s_Instance.GetTopCamPos();
        m_MvmRangeYCoord = new Vector2(topCamPos.y - (m_MvmRangeYWorldUnit.x * camSize.y), topCamPos.y - (m_MvmRangeYWorldUnit.y * camSize.y));

        // set default Position of fish (in mvm range)
        Vector2 pos = transform.position;
        pos.y = Random.RandomRange(m_MvmRangeYCoord.x, m_MvmRangeYCoord.y);
        transform.position = pos;
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void OnGetDamage(float a_CauseHp) // currently debug = voca length
    {
        // debug formular
        m_TargetHp = m_BossInfo.m_FishHP.m_CurrentHP - (m_BossInfo.m_FishHP.m_MaxHP / a_CauseHp);
        m_TargetHp = m_BossInfo.m_FishHP.m_CurrentHP - (m_BossInfo.m_FishHP.m_MaxHP / 2.0f);
        // real formular
        //m_BossInfo.m_FishHP.m_CurrentHP -= a_CauseHp;

        // to Die State
        if (m_TargetHp <= 0 && m_BossInfo.m_CurFishState != Fish.FishState.Die)
        {
            UpdateBossState(Fish.FishState.Die);
        }
        else
        {
            UpdateBossState(Fish.FishState.Hit);
        }
    }

    public void OnEventProcessEndAnim()
    {
        if (m_BossInfo.m_CurFishState == Fish.FishState.Attack || m_BossInfo.m_CurFishState == Fish.FishState.Die)
        {
            // attack state
            if (m_BossInfo.m_CurFishState == Fish.FishState.Attack)
            {
                // revert to move state for Boss
                UpdateBossState(Fish.FishState.Move);
                // active hit fx to player's fish
                (Ref_GScene as StoryGameBossMgr).OnHitPlayerFish(m_BossInfo.m_FishID, m_BossInfo.m_FishDam.m_FishDam);
            }
            // die state
            else
            {
                // invisible boss object
                SetActive(false);
                // back to tank
                (Ref_GScene as StoryGameBossMgr).OnBackToTank(true);
            }
            // remove trigger Event func
            m_FishSpritesMgr.m_delEndAnim -= OnEventProcessEndAnim;
        }
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void UpdatePosition(float a_dt)
    {
        Vector2 pos = transform.position;
        pos.y += m_MvmDirection * m_MvmSpeedY * a_dt;
        if (m_MvmDirection == 1 && pos.y > m_MvmRangeYCoord.x)
        {
            pos.y = m_MvmRangeYCoord.x;
            m_MvmDirection *= -1;
        }
        else if (m_MvmDirection == -1 && pos.y < m_MvmRangeYCoord.y)
        {
            pos.y = m_MvmRangeYCoord.y;
            m_MvmDirection *= -1;
        }

        transform.position = pos;
    }

    private void UpdateBossHP(float a_dt)
    {
        // update Lose hp
        if (m_BossInfo.m_FishHP.m_CurrentHP > m_TargetHp)
        {
            m_BossInfo.m_FishHP.m_CurrentHP -= m_LoseHpSpeed * a_dt;
            if (m_BossInfo.m_FishHP.m_CurrentHP <= m_TargetHp)
            {
                m_BossInfo.m_FishHP.m_CurrentHP = m_TargetHp;

                // return from Hit State
                if (m_BossInfo.m_FishHP.m_CurrentHP > 0)
                    UpdateBossState(Fish.FishState.Move);
            }

            if (m_BossInfo.m_FishHP.m_CurrentHP < 0)
                m_BossInfo.m_FishHP.m_CurrentHP = 0;

            // update Hp bar
            Vector2 bossHP = new Vector2(m_BossInfo.m_FishHP.m_CurrentHP, m_BossInfo.m_FishHP.m_MaxHP);
            (Ref_GScene as StoryGameBossMgr).UpdateBossHP(bossHP);
        }
    }

    private void UpdateBossTime(float a_dt)
    {
        m_BossAtkTime.x -= a_dt;
        (Ref_GScene as StoryGameBossMgr).UpdateBossTime(m_BossAtkTime);

        if (m_BossAtkTime.x <= 0)
        {
            m_BossAtkTime.x = m_BossAtkTime.y;
            // change to Atk state
            UpdateBossState(Fish.FishState.Attack);
        }
    }

    private void UpdateBossState(Fish.FishState a_FishState)
    {
        // load Sprites & set Fish State (default = STAND)
        m_BossInfo.m_CurFishState = a_FishState;
        // load Sprites for fish (fst time)
        if (!m_FishSpritesMgr.IsLoadedSprites())
            m_FishSpritesMgr.Init(GetComponent<SpriteRenderer>(), m_BossInfo);
        // update Sprites for fish (following current state)
        m_FishSpritesMgr.UpdateSprites(m_BossInfo.m_CurFishState);

        // adding trigger Event func
        if (a_FishState == Fish.FishState.Attack || a_FishState == Fish.FishState.Die)
        {
            m_FishSpritesMgr.m_delEndAnim += OnEventProcessEndAnim;
        }
        // revert full time in case player hits boss
        else if (a_FishState == Fish.FishState.Hit)
        {
            m_BossAtkTime.x = m_BossAtkTime.y;
        }
    }
    #endregion
}
