using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryFish : GElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // private vars
    private int m_MvmDirection = 1;
    [SerializeField]
    private float m_MvmSpeedY = 5.0f;
    [SerializeField]
    private Vector2 m_MovementZoneY = new Vector2(1.0f, 2.0f);
    private Vector2 m_MvmZoneYLimit = Vector2.zero;
    private FishInfo m_FishInfo;
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

        // update frame and position of ENTITY
        m_FishSpritesMgr.UpdateEntity(a_dt);
        UpdatePosition(a_dt);
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }

    // GElement Override
    public override void Init(GScene a_Scene)
    {
        base.Init(a_Scene);

        // set SCALE for mvm zone
        Vector2 camSize = CameraController.s_Instance.GetWorldUnitSize();
        Vector2 topCamPos = CameraController.s_Instance.GetTopCamPos();
        m_MvmZoneYLimit = new Vector2(topCamPos.y - (m_MovementZoneY.x * camSize.y), topCamPos.y - (m_MovementZoneY.y * camSize.y));

        // Load SPRITEs for Fish
        string fishIndex = (Ref_GScene as StoryGameMgr).PCurNodeInfo.m_FishIndex;
        m_FishInfo = DbMgr.s_Instance.GetFishInfo(fishIndex);

        // load Sprites for fish
        m_FishSpritesMgr.Init(GetComponent<SpriteRenderer>(), m_FishInfo);
        // default fish state = STAND
        m_FishInfo.m_CurFishState = Fish.FishState.Stand;
        // update Sprites for fish (following current state)
        m_FishSpritesMgr.UpdateSprites(m_FishInfo.m_CurFishState);

        // set default POSITION of fish
        Vector2 pos = transform.position;
        pos.y = Random.RandomRange(m_MvmZoneYLimit.x, m_MvmZoneYLimit.y);
        transform.position = pos;
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void UpdatePosition(float a_dt)
    {
        Vector2 pos = transform.position;
        pos.y += m_MvmDirection * m_MvmSpeedY * a_dt;
        if (m_MvmDirection == 1 && pos.y > m_MvmZoneYLimit.x)
        {
            pos.y = m_MvmZoneYLimit.x;
            m_MvmDirection *= -1;
        }
        else if (m_MvmDirection == -1 && pos.y < m_MvmZoneYLimit.y)
        {
            pos.y = m_MvmZoneYLimit.y;
            m_MvmDirection *= -1;
        }

        transform.position = pos;
    }
    #endregion
}
