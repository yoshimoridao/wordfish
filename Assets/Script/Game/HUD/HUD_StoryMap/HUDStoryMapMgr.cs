using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HUDStoryMapMgr : GHUD
{
    // ================================== VARIABLES ==================================
    #region Vars
    // enum vars
    // note: index of STATE equal to index of SPRITE
    public enum NodeState { Locked, Unlocked, LockedBoss, UnlockedBoss };
    // reference vars
    public HUDStoryHighlightNode Ref_HighlightBtn;
    public HUDProgressMap Ref_ProgressMap;
    public GameObject Ref_MapTemplateObj;
    private DbMgr Ref_DbMgr;
    // prefab vars
    public GameObject Pre_NodeZoneTouch;
    // private vars
    [SerializeField]
    private float m_SwitchMapSpeed = 2.0f; // percent of screen width per second
    private int m_SwipeAnimDir;
    private NodeSaveInfo m_CurSelectedNode;
    private NodeSaveInfo m_LastUnlockedNode;
    private MapInfo m_CurMapInfo = new MapInfo();
    private List<GameObject> m_lNodeObjs = new List<GameObject>();
    private List<GameObject> m_lGenMapObjs = new List<GameObject>();
    #endregion

    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
    public override void OnCreateObj()
    {
        base.OnCreateObj();

        Ref_DbMgr = DbMgr.s_Instance;

        InputMgr.m_eventProcessSwipeUp += ProcessSwipeUp;
        InputMgr.m_eventProcessSwipeDown += ProcessSwipeDown;
        InputMgr.m_eventProcessSwipeRight += ProcessSwipeRight;
        InputMgr.m_eventProcessSwipeLeft += ProcessSwipeLeft;
    }

    public override void OnUpdateObj(float a_dt)
    {
        base.OnUpdateObj(a_dt);

        // update anim
        if (m_lGenMapObjs.Count > 1)
        {
            for (int i = 0; i < m_lGenMapObjs.Count; i++)
            {
                GameObject mapObj = m_lGenMapObjs[i];
                Vector2 mapPos = mapObj.GetComponent<RectTransform>().localPosition;
                mapPos.x += m_SwipeAnimDir * m_SwitchMapSpeed * Screen.width * a_dt;
                mapObj.GetComponent<RectTransform>().localPosition = mapPos;
                // remove old map obj
                if (i == m_lGenMapObjs.Count - 1)
                {
                    if ((m_SwipeAnimDir == 1 && mapPos.x >= 0) || (m_SwipeAnimDir == -1 && mapPos.x <= 0))
                    {
                        DestroyOldMap();
                    }
                }
            }
        }
    }

    public override void OnDestroyObj()
    {
        InputMgr.m_eventProcessSwipeUp -= ProcessSwipeUp;
        InputMgr.m_eventProcessSwipeDown -= ProcessSwipeDown;
        InputMgr.m_eventProcessSwipeRight -= ProcessSwipeRight;
        InputMgr.m_eventProcessSwipeLeft -= ProcessSwipeLeft;
        base.OnDestroyObj();
    }

    // override HUD
    public override void Init(HUDInfo a_HUDInfo)
    {
        base.Init(a_HUDInfo);

        // hide map template
        Ref_MapTemplateObj.SetActive(false);
        SetLocationForElement(Ref_MapTemplateObj);

        AddHUDElement(Ref_HighlightBtn);

        // get index of last unlocked & selected nodes
        m_LastUnlockedNode = Ref_DbMgr.PLastUnlockedNode;
        m_CurSelectedNode = Ref_DbMgr.PLastSelectedNode;
        CheckRevertToUnlockedTopic();
        // get list of node infos
        m_CurMapInfo = Ref_DbMgr.GetMapInfo(m_CurSelectedNode.m_TopicIndex);

        // gen map
        GenMap();
        // show highlight with scale equal to the gen map
        ShowHighlight();
        // show progress of node
        ShowNodeProgress();
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void OnPressNode(int a_index)
    {
        if (a_index >= m_CurMapInfo.m_lNodes.Count)
            return;

        UpdateSelectedNode(m_CurSelectedNode.m_TopicIndex, a_index);
        ShowHighlight();

        // changing to story GAMEPLAY (Boss || Normal mode)
        DbMgr dbMgr = DbMgr.s_Instance;
        NodeInfo curNodeInfo = dbMgr.GetNodeInfo(m_CurSelectedNode);
        FishInfo fishOfCurNodeInfo = dbMgr.GetFishInfo(curNodeInfo.m_FishIndex);

        if (fishOfCurNodeInfo.m_FishKind == Fish.FishKind.Boss)
            SceneMgr.s_Instance.ChangeScene(SceneMgr.SceneType.StoryBossGame);
        else
            SceneMgr.s_Instance.ChangeScene(SceneMgr.SceneType.StoryGame);
    }

    #region Delegate Funcs
    public void ProcessSwipeUp()
    {
    }

    public void ProcessSwipeDown()
    {
    }

    public void ProcessSwipeLeft()
    {
        ProcessSwipeHorizontal(true);
    }

    public void ProcessSwipeRight()
    {
        ProcessSwipeHorizontal(false);
    }
    #endregion

    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void ProcessSwipeHorizontal(bool a_isSwipeLeft)
    {
        int scrollSide = a_isSwipeLeft ? 1 : -1;
        // update topic index
        int nextTopicIndex = m_CurSelectedNode.m_TopicIndex + scrollSide;
        if (Ref_DbMgr.IsMapInfoExist(nextTopicIndex))
        {
            m_CurMapInfo = Ref_DbMgr.GetMapInfo(nextTopicIndex);
            int nextNodeIndex = 0;
            // update node index
            if (scrollSide == -1) // if scroll left
            {
                // node index = index of last node (if current map smaller than unlocked map)
                if (nextTopicIndex < m_LastUnlockedNode.m_TopicIndex)
                    nextNodeIndex = m_CurMapInfo.m_lNodes.Count - 1;
                // node index = index of last unlocked node (if current map equal to unlocked map)
                else if (nextTopicIndex == m_LastUnlockedNode.m_TopicIndex)
                    nextNodeIndex = m_LastUnlockedNode.m_NodeIndex;
            }
            else
            {
                nextNodeIndex = 0;
            }
            // update NODE INDEX & TOPIC INDEX of cur node
            UpdateSelectedNode(nextTopicIndex, nextNodeIndex);
            GenMap(scrollSide);
            // store switch side to create anim
            m_SwipeAnimDir = a_isSwipeLeft ? -1 : 1;
            // HIDE highlight & progress to start SWIPE MAP ANIM
            Ref_HighlightBtn.HideHighlight();
            Ref_ProgressMap.HideProgress();
        }
    }

    private void DestroyOldMap()
    {
        if (m_lGenMapObjs.Count <= 1)
            return;

        //destroy old map
        Destroy(m_lGenMapObjs[0]);
        m_lGenMapObjs.RemoveAt(0);
        // reset pos of MAP
        RectTransform rtMap = m_lGenMapObjs[0].GetComponent<RectTransform>();
        Vector2 mapPos = rtMap.localPosition;
        mapPos.x = 0;
        rtMap.localPosition = mapPos;

        // show HIGH LIGHT & PROGRESS of node
        ShowHighlight();
        ShowNodeProgress();
    }

    /// <summary>
    /// Gen MAP
    /// </summary>
    /// <param name="a_genSide">-1:left side || 0:center || 1:right side (ratio vs screen width)</param>
    private void GenMap(int a_genSide = 0)
    {
        // clear old nodes
        m_lNodeObjs.Clear();
        // gen MAP
        GameObject mapObjPref = Resources.Load<GameObject>(AssetPathConstant.FOLDER_MAP_PATH + "/" + m_CurSelectedNode.m_TopicIndex.ToString());
        if (mapObjPref == null)
            return;
        GameObject genMapObj = Instantiate(mapObjPref, transform);
        genMapObj.transform.SetAsFirstSibling();
        m_lGenMapObjs.Add(genMapObj);

        // set SIZE & SCALE following map template
        RectTransform rtMapTemplate = Ref_MapTemplateObj.GetComponent<RectTransform>();
        RectTransform rtMapObj = genMapObj.GetComponent<RectTransform>();
        rtMapObj.sizeDelta = rtMapTemplate.sizeDelta;
        rtMapObj.localScale = rtMapTemplate.localScale;

        // set POS following side
        Vector3 mapPos = rtMapObj.localPosition;
        mapPos.x = rtMapTemplate.localPosition.x + (rtMapTemplate.sizeDelta.x * rtMapTemplate.localScale.x * a_genSide);
        mapPos.y = rtMapTemplate.localPosition.y;
        rtMapObj.localPosition = mapPos;

        // set up NODEs of map
        GameObject nodesContObj = genMapObj.transform.GetChild(0).gameObject;
        for (int i = 0; i < m_CurMapInfo.m_lNodes.Count; i++)
        {
            if (i >= nodesContObj.transform.childCount)
                break;

            NodeInfo curNode = m_CurMapInfo.m_lNodes[i];
            GameObject nodeObj = nodesContObj.transform.GetChild(i).gameObject;
            // set SPRITE depend on STATE of node
            NodeState nodeState = NodeState.Locked;
            // BOSS node (if list of vocas = null)
            if (curNode.m_lVocaIndex.Count == 0)
                nodeState = NodeState.LockedBoss;
            // UNLOCKED node || Unlocked boss node
            if ((curNode.m_TopicIndex == m_LastUnlockedNode.m_TopicIndex && i <= m_LastUnlockedNode.m_NodeIndex) ||
                curNode.m_TopicIndex < m_LastUnlockedNode.m_TopicIndex)
            {
                nodeState = nodeState == NodeState.LockedBoss ? NodeState.UnlockedBoss : NodeState.Unlocked;
            }

            string spritePath = AssetPathConstant.FOLDER_HIGHLIGHT_NODE_PATH + "/" + ((int)nodeState).ToString();
            Sprite nodeSprite = Resources.Load<Sprite>(spritePath);
            RectTransform rtNode = nodeObj.GetComponent<RectTransform>();
            if (nodeSprite)
            {
                Image nodeImg = nodeObj.GetComponentInChildren<Image>();
                nodeImg.sprite = nodeSprite;
                rtNode.sizeDelta = new Vector2(nodeImg.sprite.rect.width, nodeImg.sprite.rect.height);
            }
            m_lNodeObjs.Add(nodeObj);

            // gen TOUCH ZONE & set SIZE for node
            if (nodeState == NodeState.Unlocked || nodeState == NodeState.UnlockedBoss)
            {
                GameObject nodeTouchZoneObj = Instantiate(Pre_NodeZoneTouch, nodeObj.transform);
                nodeTouchZoneObj.GetComponent<RectTransform>().sizeDelta = rtNode.sizeDelta;

                Button nodeBtn = nodeTouchZoneObj.GetComponent<Button>();
                if (nodeBtn)
                {
                    int btnIndex = i;
                    nodeBtn.onClick.AddListener(() => OnPressNode(btnIndex));
                }
            }
        }
    }

    private void ShowHighlight()
    {
        if (m_CurSelectedNode.m_TopicIndex > m_LastUnlockedNode.m_TopicIndex)
            return;

        // show highlight with scale equal to the gen map
        int hlNodeIndex = m_CurSelectedNode.m_NodeIndex;
        Ref_HighlightBtn.ShowHighlight(m_lNodeObjs[hlNodeIndex], m_lGenMapObjs[0]);
    }

    private void ShowNodeProgress()
    {
        if (m_CurSelectedNode.m_TopicIndex == m_LastUnlockedNode.m_TopicIndex)
        {
            int lastUnlockedNode = m_LastUnlockedNode.m_NodeIndex;
            if (lastUnlockedNode < m_lNodeObjs.Count)
                Ref_ProgressMap.ShowProgress(m_lNodeObjs[lastUnlockedNode], m_LastUnlockedNode.m_Progress, m_lGenMapObjs[0]);
        }
    }

    private void CheckRevertToUnlockedTopic()
    {
        if (m_CurSelectedNode.m_TopicIndex > m_LastUnlockedNode.m_TopicIndex)
        {
            UpdateSelectedNode(m_LastUnlockedNode.m_TopicIndex, m_LastUnlockedNode.m_NodeIndex);
        }
    }
    private void UpdateSelectedNode(int a_nextTopicIndex, int a_nextNodeIndex)
    {
        m_CurSelectedNode.m_TopicIndex = a_nextTopicIndex;
        m_CurSelectedNode.m_NodeIndex = a_nextNodeIndex;

        // save last selected node
        Ref_DbMgr.UpdateLastSelectedNode(0, a_nextNodeIndex, a_nextTopicIndex);
    }
    #endregion
}
