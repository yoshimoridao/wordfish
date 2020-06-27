using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGameBossMgr : GScene
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    public StoryKeyboardMgr Ref_StoryKbMgr;
    public StoryAnswerCont Ref_StoryAnswerCont;
    public StoryPressedTextCont Ref_StoryPressedTextCont;
    public StoryBossFish Ref_StoryBossFish;

    // private vars
    private bool m_IsChangeNextVoca;
    private Vector2 m_Progress = Vector2.zero;
    private Vector2 m_DelayChangeVoca = new Vector2(0, 0.5f);
    private List<VocaInfo> m_lVocas = new List<VocaInfo>();
    private NodeInfo m_CurNodeInfo;
    private bool m_IsBackToTank;
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
                HideDescriptionBalloon();
            // Cause damage to Boss
            OnPlayerFishAtk();
        }
    }
    public NodeInfo PCurNodeInfo
    {
        get { return m_CurNodeInfo; }
    }
    #endregion

    // =================================== OVERRIDE func ===================================
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

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    // Description Balloon
    public void ShowDescriptionBalloon()
    {
        DbMgr dbMgr = DbMgr.s_Instance;
        VocaInfo vocaInfo = m_lVocas[(int)m_Progress.x];
        (Ref_GHUD as HUDStoryBossGameplay).ShowDescriptionBalloon(vocaInfo.m_Def);
    }

    // Boss Cont
    public void SetBossAva(string a_BossID)
    {
        (Ref_GHUD as HUDStoryBossGameplay).SetBossAva(a_BossID);
    }

    public void UpdateBossHP(Vector2 a_HP)
    {
        (Ref_GHUD as HUDStoryBossGameplay).UpdateBossHP(a_HP);
    }

    public void UpdateBossTime(Vector2 a_Time)
    {
        (Ref_GHUD as HUDStoryBossGameplay).UpdateBossTime(a_Time);
    }

    // Player's Fish
    public void OnPlayerFishAtk()
    {
        Ref_StoryBossFish.OnGetDamage(m_lVocas.Count);
    }

    public void OnBackToTank(bool isCompleteTopic = false)
    {
        if (isCompleteTopic)
        {
            // update Save -> change to next Topic
            DbMgr.s_Instance.UpdateUnlockedNodeByAmount(false, false, true);
        }

        m_IsBackToTank = true;
        // back to TANK
        SceneMgr.s_Instance.ChangeScene(SceneMgr.SceneType.Tank);
    }

    // HUD Player Cont funcs
    public void OnHitPlayerFish(string a_BallBossId, float a_HitDamage)
    {
        (Ref_GHUD as HUDStoryBossGameplay).OnHitPlayerFish(a_BallBossId, a_HitDamage);
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void Init()
    {
        // get all vocas in the topic
        GetAllVocasInTopic();

        // add GElement
        AddGElement(Ref_StoryKbMgr);
        AddGElement(Ref_StoryAnswerCont);
        AddGElement(Ref_StoryPressedTextCont);
        AddGElement(Ref_StoryBossFish);

        // update cur voca
        UpdateVoca();

        // init player's fish
        FishInfo playerFishInfo = DbMgr.s_Instance.GetFishInfo("010203");   // get TEMP demo fish info for player
        (Ref_GHUD as HUDStoryBossGameplay).InitPlayerContainer(playerFishInfo);
    }

    private void OnChangeNextVoca()
    {
        m_Progress.x++;
        // finish topic
        if (m_Progress.x >= m_Progress.y)
        {
            // back to tank
            OnBackToTank(true);
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

        // get list of kb template following length of current vocabulary
        var listTemplate = dbMgr.GetListKbTemplate(vocaInfo.m_Voca.Length);
        if (listTemplate != null)
        {
            // pick a random template
            int[,] template = listTemplate[Random.RandomRange(0, listTemplate.Count)];

            // gen keyboard
            Ref_StoryKbMgr.GenKb(vocaInfo.m_Voca, template);
        }

        // show description
        ShowDescriptionBalloon();
    }

    private void HideDescriptionBalloon()
    {
        (Ref_GHUD as HUDStoryBossGameplay).HideDescriptionBalloon();
    }

    private void GetAllVocasInTopic()
    {
        DbMgr dbMgr = DbMgr.s_Instance;
        m_CurNodeInfo = dbMgr.GetNodeInfo(dbMgr.PLastSelectedNode);
        int curTopicIndex = m_CurNodeInfo.m_TopicIndex;
        MapInfo mapInfo = dbMgr.GetMapInfo(curTopicIndex);

        // get list vocas of cur Map
        List<VocaInfo> lVocas = new List<VocaInfo>();
        for (int i = 0; i < mapInfo.m_lNodes.Count; i++)
        {
            NodeInfo node = mapInfo.m_lNodes[i];
            if (node.m_lVocaIndex.Count == 0)
                continue;

            foreach (int vocaIndex in node.m_lVocaIndex)
                lVocas.Add(dbMgr.GetVocaInfo(node.m_TopicIndex, vocaIndex));
        }
        if (lVocas.Count > 0)
        {
            m_lVocas = lVocas;
            // update progress
            m_Progress = new Vector2(0, m_lVocas.Count);
        }

        // shuffle list vocas
        UtilityClass.ShuffleList<VocaInfo>(ref m_lVocas);
    }

    private void ShuffleVocas()
    {
        // Shuffle vocas list
        for (int i = 0; i < m_lVocas.Count; i++)
        {
            int swapIndex = Random.RandomRange(0, m_lVocas.Count);
            var temp = m_lVocas[i];
            m_lVocas[i] = m_lVocas[swapIndex];
            m_lVocas[swapIndex] = temp;
        }
    }
    #endregion
}
