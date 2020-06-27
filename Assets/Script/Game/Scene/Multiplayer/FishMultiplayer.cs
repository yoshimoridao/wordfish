using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMultiplayer : GElement
{
    // ================================== VARIABLES ==================================
    #region Vars
    // public vars
    public enum FishType { Player, Opponent };
    // private vars
    private int m_MvmDirection = 1;
    [SerializeField]
    private float m_MvmSpeedY = 5.0f;
    [SerializeField]
    private Vector2 m_MovementZoneY = new Vector2(1.0f, 2.0f);
    private Vector2 m_MvmZoneYLimit = Vector2.zero;
    private FishInfo m_FishInfo;
    private FishSpritesMgr m_FishSpritesMgr = new FishSpritesMgr();
    [SerializeField]
    private FishType m_FishType;
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
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

        // set default POSITION of fish
        Vector2 pos = transform.position;
        pos.y = Random.RandomRange(m_MvmZoneYLimit.x, m_MvmZoneYLimit.y);
        transform.position = pos;

        // set Flip Face for fish
        SpriteRenderer fishSr = GetComponent<SpriteRenderer>();
        fishSr.flipX = (m_FishType == FishType.Player);
    }
    #endregion

    public void UpdateFishInfo(string a_FishIndex)
    {
        // Load SPRITEs for Fish
        m_FishInfo = DbMgr.s_Instance.GetFishInfo(a_FishIndex);
        // load Sprites for fish
        m_FishSpritesMgr.Init(GetComponent<SpriteRenderer>(), m_FishInfo);
        // default fish state = STAND
        m_FishInfo.m_CurFishState = Fish.FishState.Stand;
        // update Sprites for fish (following current state)
        m_FishSpritesMgr.UpdateSprites(m_FishInfo.m_CurFishState);
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
