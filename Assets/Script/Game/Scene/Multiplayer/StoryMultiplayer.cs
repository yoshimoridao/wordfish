using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMultiplayer : GScene
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    public StoryKeyboardMgr Ref_StoryKbMgr;
    public StoryAnswerCont Ref_StoryAnswerCont;
    public StoryPressedTextCont Ref_StoryPressedTextCont;
    [SerializeField]
    private FishMultiplayer Ref_PlayerFish;
    [SerializeField]
    private FishMultiplayer Ref_OpponentFish;

    // private vars
    private bool m_IsChangeNextVoca;
    private bool m_IsBackToTank;
    private Vector2 m_Progress = Vector2.zero;
    private Vector2 m_DelayChangeVoca = new Vector2(0, 0.5f);
    private List<VocaInfo> m_lVocas = new List<VocaInfo>();
    #endregion

    // ============================ PROPERTIES ============================
    #region Properties
    public bool PIsChangeNextVoca
    {
        get { return m_IsChangeNextVoca; }
        set
        {
            m_IsChangeNextVoca = value;
            // Hide description balloon
            if (m_IsChangeNextVoca)
            {
                HideDescriptionBalloon();
            }
        }
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs

    // ============= OVERRIDE func =============
    #region Override func
    public override void OnCreateObj()
    {
        base.OnCreateObj();
    }

    public override void OnUpdateObj(float a_dt)
    {
        if (m_IsBackToTank)
        {
            return;
        }

        base.OnUpdateObj(a_dt);

        // changing next voca
        if (m_IsChangeNextVoca)
        {
            m_DelayChangeVoca.x += a_dt;
            if (m_DelayChangeVoca.x >= m_DelayChangeVoca.y)
            {
                m_DelayChangeVoca.x = 0;
                OnChangeNextVoca();
                m_IsChangeNextVoca = false;
            }
        }
    }

    public override void OnDestroyObj()
    {
        base.OnDestroyObj();
    }

    // override GScene
    public override void Init(SceneInfo a_SceneInfo, GHUD a_GHUD)
    {
        base.Init(a_SceneInfo, a_GHUD);
        Init();
    }
    #endregion

    public void OnBackToTank()
    {
        m_IsBackToTank = true;
        // back to TANK
        SceneMgr.s_Instance.ChangeScene(SceneMgr.SceneType.Tank);
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void Init()
    {
        // add GElement
        AddGElement(Ref_StoryKbMgr);
        AddGElement(Ref_StoryAnswerCont);
        AddGElement(Ref_StoryPressedTextCont);
        AddGElement(Ref_PlayerFish);
        Ref_PlayerFish.UpdateFishInfo("010203"); // TEMP >> set default index for fish
        AddGElement(Ref_OpponentFish);
        Ref_OpponentFish.UpdateFishInfo("030102"); // TEMP >> set default index for opponent's fish

        // get all vocas in the topic
        GetAllVocaInfo();
        // update cur voca
        UpdateVoca();

        //// init player's fish
        //FishInfo playerFishInfo = DbMgr.s_Instance.GetFishInfo("010203");   // get TEMP demo fish info for player
        //(Ref_GHUD as HUDStoryBossGameplay).InitPlayerContainer(playerFishInfo);
    }

    private void OnChangeNextVoca()
    {
        m_Progress.x++;
        // out of vocabularies
        if (m_Progress.x >= m_Progress.y)
        {
            ShuffleListVoca();
        }
        else
        {
            UpdateVoca();
            // show description
            ShowDescriptionBalloon();
        }
    }
    private void UpdateVoca()
    {
        DbMgr dbMgr = DbMgr.s_Instance;
        if (m_Progress.x >= m_Progress.y)
            return;

        // get current vocainfo
        VocaInfo vocaInfo = m_lVocas[(int)m_Progress.x];
        Debug.Log(vocaInfo.m_Voca); // DEBUG
        if (vocaInfo == null)
            return;

        // pick a random template
        int[,] template = dbMgr.GetRdKbTemplate(vocaInfo.m_Voca.Length);
        // gen keyboard for new vocabulary
        Ref_StoryKbMgr.GenKb(vocaInfo.m_Voca, template);

        //// show description
        //ShowDescriptionBalloon();
    }

    // Description Balloon
    private void ShowDescriptionBalloon()
    {
        //DbMgr dbMgr = DbMgr.s_Instance;
        //VocaInfo vocaInfo = m_lVocas[(int)m_Progress.x];
        //(Ref_GHUD as HUDStoryBossGameplay).ShowDescriptionBalloon(vocaInfo.m_Def);
    }
    private void HideDescriptionBalloon()
    {
        //(Ref_GHUD as HUDStoryBossGameplay).HideDescriptionBalloon();
    }

    private void GetAllVocaInfo()
    {
        DbMgr dbMgr = DbMgr.s_Instance;
        List<VocasInfo> lVocasInfo = dbMgr.GetListVocasInfo();

        // get all vocainfo of dictionary
        for (int i = 0; i < lVocasInfo.Count; i++)
        {
            VocasInfo vocasInfo = lVocasInfo[i];
            for (int j = 0; j < vocasInfo.m_lVocas.Count; j++)
            {
                VocaInfo vocaInfo = new VocaInfo(vocasInfo.m_lVocas[j]);
                m_lVocas.Add(vocaInfo);
            }
        }
        ShuffleListVoca();
    }
    private void ShuffleListVoca()
    {
        // update progress
        m_Progress = new Vector2(0, m_lVocas.Count);

        UtilityClass.ShuffleList<VocaInfo>(ref m_lVocas);
    }
    #endregion
}
