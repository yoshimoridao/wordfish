using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGameMgr : GScene
{
    // ================================== VARIABLES ==================================
    #region Vars
    // reference vars
    public StoryKeyboardMgr Ref_StoryKbMgr;
    public StoryProgressCont Ref_StoryProgressCont;
    public StoryAnswerCont Ref_StoryAnswerCont;
    public StoryPressedTextCont Ref_StoryPressedTextCont;
    public StoryFish Ref_StoryFish;

    // private vars
    private bool m_IsChangeNextVoca;
    private NodeInfo m_CurNodeInfo;
    private Vector2 m_Progress = Vector2.zero;
    private Vector2 m_DelayChangeVoca = new Vector2(0, 0.5f);
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
    public void ShowDescriptionBalloon()
    {
        if ((int)m_Progress.x >= m_CurNodeInfo.m_lVocaIndex.Count)
            return;

        DbMgr dbMgr = DbMgr.s_Instance;
        VocaInfo vocaInfo = dbMgr.GetVocaInfo(m_CurNodeInfo.m_TopicIndex, m_CurNodeInfo.m_lVocaIndex[(int)m_Progress.x]);
        (Ref_GHUD as HUDStoryGameplay).ShowDescriptionBalloon(vocaInfo.m_Def);
    }

    public void OnBackToTank(bool isCompleteNode = false)
    {
        if (isCompleteNode)
        {
            // update Save -> change to next node
            DbMgr.s_Instance.UpdateUnlockedNodeByAmount(false, true, false);
        }

        m_IsBackToTank = true;
        // back to TANK
        SceneMgr.s_Instance.ChangeScene(SceneMgr.SceneType.Tank);
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void Init()
    {
        // get info of current node
        DbMgr dbMgr = DbMgr.s_Instance;
        NodeSaveInfo lastSelectedNode = dbMgr.PLastSelectedNode;
        m_CurNodeInfo = dbMgr.GetNodeInfo(lastSelectedNode);

        // add GElement
        AddGElement(Ref_StoryKbMgr);
        AddGElement(Ref_StoryProgressCont);
        AddGElement(Ref_StoryAnswerCont);
        AddGElement(Ref_StoryPressedTextCont);
        AddGElement(Ref_StoryFish);

        // show progress
        m_Progress = lastSelectedNode.m_Progress;
        m_Progress.y = m_CurNodeInfo.m_lVocaIndex.Count;
        Ref_StoryProgressCont.ShowProgress(m_CurNodeInfo.m_TopicIndex, m_Progress);

        // gen voca
        UpdateVoca();
    }

    private void OnChangeNextVoca()
    {
        m_Progress.x++;
        // update Save -> update progress
        DbMgr.s_Instance.UpdateUnlockedNodeByAmount(true, false, false);

        Ref_StoryProgressCont.ShowProgress(m_CurNodeInfo.m_TopicIndex, m_Progress, true);
        // finish topic
        if (m_Progress.x >= m_Progress.y)
        {
            // back to tank
            OnBackToTank(true);
        }
        else
        {
            UpdateVoca();
        }
    }
    private void UpdateVoca()
    {
        DbMgr dbMgr = DbMgr.s_Instance;
        if (m_Progress.x >= m_Progress.y)
            return;

        VocaInfo vocaInfo = dbMgr.GetVocaInfo(m_CurNodeInfo.m_TopicIndex, m_CurNodeInfo.m_lVocaIndex[(int)m_Progress.x]);
        Debug.Log(vocaInfo.m_Voca); // DEBUG
        if (vocaInfo == null)
            return;

        // get list of kb template following length of current vocabulary
        var listTemplate = dbMgr.GetListKbTemplate(vocaInfo.m_Voca.Length);
        // init for keyboard
        if (listTemplate != null)
        {
            // pick a random template
            int[,] template = listTemplate[Random.RandomRange(0, listTemplate.Count)];
            Ref_StoryKbMgr.GenKb(vocaInfo.m_Voca, template);
        }

        // show description
        ShowDescriptionBalloon();
    }

    private void HideDescriptionBalloon()
    {
        (Ref_GHUD as HUDStoryGameplay).HideDescriptionBalloon();
    }

    //private void ShuffleVocas()
    //{
    //    // Get all of vocas in map
    //    if (m_CurNodeInfo.m_VocaIndexes.Count == 0)
    //    {
    //        DbMgr dbMgr = DbMgr.s_Instance;
    //        m_CurNodeInfo.m_VocaIndexes = dbMgr.GetVocaIndexesOfMap(m_CurNodeInfo.m_TopicIndex);
    //    }

    //    // Shuffle vocas list
    //    for (int i = 0; i < m_CurNodeInfo.m_VocaIndexes.Count; i++)
    //    {
    //        int swapIndex = Random.RandomRange(0, m_CurNodeInfo.m_VocaIndexes.Count);
    //        int temp = m_CurNodeInfo.m_VocaIndexes[i];
    //        m_CurNodeInfo.m_VocaIndexes[i] = m_CurNodeInfo.m_VocaIndexes[swapIndex];
    //        m_CurNodeInfo.m_VocaIndexes[swapIndex] = temp;
    //    }

    //    m_Progress = new Vector2(0, m_CurNodeInfo.m_VocaIndexes.Count);
    //}
    #endregion
}
