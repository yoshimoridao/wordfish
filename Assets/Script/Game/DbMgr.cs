using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DbMgr : MonoBehaviour
{
    public class SaveDataInfo
    {
        // story map
        public NodeSaveInfo m_LastUnlockedNode;
        public NodeSaveInfo m_LastSelectedNode;
    }

    // ================================== VARIABLES ==================================
    #region Vars
    // static vars
    public static DbMgr s_Instance;
    // public vars
    public SaveDataInfo m_saveDataInfo;
    // private vars
    // --- loaded data ---
    // scene
    private Dictionary<SceneMgr.SceneType, SceneInfo> m_dSceneInfo = new Dictionary<SceneMgr.SceneType, SceneInfo>();
    // HUD
    private Dictionary<HUDMgr.HUDType, HUDInfo> m_dHUDInfo = new Dictionary<HUDMgr.HUDType, HUDInfo>();
    // topic
    private List<TopicInfo> m_lTopicInfos = new List<TopicInfo>();
    // voca (key = topic_index,)
    private Dictionary<int, VocasInfo> m_dVocaInfos = new Dictionary<int, VocasInfo>();
    // keyboard (key = voca length)
    private Dictionary<int, List<int[,]>> m_dKbTemplate = new Dictionary<int,List<int[,]>>();
    // map (key = topic_index)
    private Dictionary<int, MapInfo> m_dMap = new Dictionary<int, MapInfo>();
    // fishes (Key in DB = FishKind_FishRank_FishSex_FishID_ShapeOfFish_FishColor)
    private Dictionary<string, FishDataInfo> m_DictFishInfo = new Dictionary<string, FishDataInfo>();
    #endregion

    // ================================== PROPERTIES ==================================
    #region Properties
    public NodeSaveInfo PLastSelectedNode
    {
        get { return new NodeSaveInfo(m_saveDataInfo.m_LastSelectedNode); }
    }
    public NodeSaveInfo PLastUnlockedNode
    {
        get { return new NodeSaveInfo(m_saveDataInfo.m_LastUnlockedNode); }
    }
    #endregion

    // ================================== UNITY FUNCS ==================================
    #region Unity funcs
    private void Awake()
    {
        s_Instance = this;

        // Temp (need to load from saving data)
        m_saveDataInfo = new SaveDataInfo();
        m_saveDataInfo.m_LastSelectedNode = new NodeSaveInfo(0, 2, new Vector2(0,0));
        m_saveDataInfo.m_LastUnlockedNode = new NodeSaveInfo(0, 2, new Vector2(0,0));
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void Init()
    {
        LoadData();
    }

    // ============================== GET / SET ==============================
    // === SCENE INFO ===
    public SceneInfo GetSceneInfo(SceneMgr.SceneType sceneType)
    {
        if (!m_dSceneInfo.ContainsKey(sceneType))
            return null;

        return new SceneInfo(m_dSceneInfo[sceneType]);
    }

    // === HUD INFO ===
    public HUDInfo GetHUDInfo(HUDMgr.HUDType HUDtype)
    {
        if (!m_dHUDInfo.ContainsKey(HUDtype))
            return null;

        return new HUDInfo(m_dHUDInfo[HUDtype]);
    }

    // === MAP || NODE INFO ===
    public bool IsMapInfoExist(int topicIndex)
    {
        if (!m_dMap.ContainsKey(topicIndex))
            return false;

        return true;
    }

    public MapInfo GetMapInfo(int topicIndex)
    {
        if (IsMapInfoExist(topicIndex))
            return new MapInfo(m_dMap[topicIndex]);
        return null;
    }

    public NodeInfo GetNodeInfo(NodeSaveInfo a_nodeSaveInfo)
    {
        if (!m_dMap.ContainsKey(a_nodeSaveInfo.m_TopicIndex))
            return null;

        MapInfo mapInfo = m_dMap[a_nodeSaveInfo.m_TopicIndex];
        if (a_nodeSaveInfo.m_NodeIndex >= mapInfo.m_lNodes.Count)
            return null;

        return new NodeInfo(mapInfo.m_lNodes[a_nodeSaveInfo.m_NodeIndex]);
    }

    // === TOPIC || VOCA ===
    public TopicInfo GetTopicInfo(int a_TopicIndex)
    {
        if (a_TopicIndex >= 0 && a_TopicIndex < m_lTopicInfos.Count)
        {
            return m_lTopicInfos[a_TopicIndex];
        }
        return null;
    }
    public VocasInfo GetVocasInfo(int a_TopicIndex)
    {
        if (!m_dVocaInfos.ContainsKey(a_TopicIndex))
            return null;

        return new VocasInfo(m_dVocaInfos[a_TopicIndex]);
    }
    public VocaInfo GetVocaInfo(int a_TopicIndex, int a_VocaIndex)
    {
        if (!m_dVocaInfos.ContainsKey(a_TopicIndex))
            return null;

        VocasInfo vocasInfo = m_dVocaInfos[a_TopicIndex];
        if (a_VocaIndex >= vocasInfo.m_lVocas.Count)
            return null;

        return new VocaInfo(vocasInfo.m_lVocas[a_VocaIndex]);
    }
    public List<VocasInfo> GetListVocasInfo()
    {
        return new List<VocasInfo>(m_dVocaInfos.Values);
    }

    // === KB TEMPLATE ===
    public List<int[,]> GetListKbTemplate(int a_VocaLength)
    {
        if (!m_dKbTemplate.ContainsKey(a_VocaLength))
            return null;

        return new List<int[,]>(m_dKbTemplate[a_VocaLength]);
    }
    public int[,] GetRdKbTemplate(int a_VocaLength)
    {
        var listTemplate = GetListKbTemplate(a_VocaLength);
        if (listTemplate != null)
        {
            // pick a random template
            int[,] template = listTemplate[UnityEngine.Random.RandomRange(0, listTemplate.Count)];
            return template;
        }
        return null;
    }

    // === FISHINFO ===
    public FishInfo GetFishInfo(string fishID)
    {
        if (!m_DictFishInfo.ContainsKey(fishID))
            return null;

        FishDataInfo fishDataInfo = m_DictFishInfo[fishID];
        return new FishInfo(fishDataInfo);
    }

    #region save data funcs
    /// <summary>
    /// Func update last unlocked node by increase amount
    /// </summary>
    /// <param name="a_IsProgressIncrease"></param>
    /// <param name="a_IsNodeIndexIncrease"></param>
    /// <param name="a_IsTopicIncrease"></param>
    public void UpdateUnlockedNodeByAmount(bool a_IsProgressIncrease, bool a_IsNodeIndexIncrease, bool a_IsTopicIncrease)
    {
        // Check Player unlock new progress || node || topic
        NodeSaveInfo lastSelectedNode = m_saveDataInfo.m_LastSelectedNode;
        NodeSaveInfo lastUnlockedNode = m_saveDataInfo.m_LastUnlockedNode;

        // We unlocked NEW NODE || NEW TOPIC in case matching of current node and last node
        if (lastUnlockedNode.m_NodeIndex != lastSelectedNode.m_NodeIndex || lastUnlockedNode.m_TopicIndex != lastSelectedNode.m_TopicIndex)
            return;

        Vector2 nextProgress = lastSelectedNode.m_Progress;
        if (a_IsProgressIncrease)
            nextProgress.x++;
        int nextNodeIndex = lastSelectedNode.m_NodeIndex + (a_IsNodeIndexIncrease ? 1 : 0);
        int nextTopicIndex = lastSelectedNode.m_TopicIndex + (a_IsTopicIncrease ? 1 : 0);

        // check TOPIC is out of range
        if (m_dMap.ContainsKey(nextTopicIndex))
        {
            if (a_IsTopicIncrease)
            {
                nextNodeIndex = 0;
                nextProgress = Vector2.zero;
            }
            if (a_IsNodeIndexIncrease)
            {
                nextProgress = Vector2.zero;
            }

            // check NODE is out of range
            MapInfo nextMapInfo = m_dMap[nextTopicIndex];
            if (nextNodeIndex < nextMapInfo.m_lNodes.Count)
            {
                // check progress out of range
                if (nextProgress.y == 0)
                    nextProgress.y = nextMapInfo.m_lNodes[nextNodeIndex].m_lVocaIndex.Count;
                if (nextProgress.x > nextProgress.y)
                    nextProgress.x = 0;
            }
        }

        lastUnlockedNode.m_Progress = nextProgress;
        lastUnlockedNode.m_NodeIndex = nextNodeIndex;
        lastUnlockedNode.m_TopicIndex = nextTopicIndex;

        m_saveDataInfo.m_LastUnlockedNode = lastUnlockedNode;
        // update last selectednode
        m_saveDataInfo.m_LastSelectedNode = new NodeSaveInfo(m_saveDataInfo.m_LastUnlockedNode);
    }

    public void UpdateLastSelectedNode(int a_Progress, int a_NodeIndex, int a_TopicIndex)
    {
        NodeSaveInfo lastSelectedNode = m_saveDataInfo.m_LastSelectedNode;

        // update progress
        lastSelectedNode.m_Progress.x = a_Progress;
        // check progress out of range
        if (lastSelectedNode.m_Progress.x >= lastSelectedNode.m_Progress.y)
            lastSelectedNode.m_Progress.x = 0;

        // update node index
        lastSelectedNode.m_NodeIndex = a_NodeIndex;
        // check node out of range
        int nodesOfCurTopic = m_dMap[lastSelectedNode.m_TopicIndex].m_lNodes.Count;
        if (lastSelectedNode.m_NodeIndex >= nodesOfCurTopic)
            lastSelectedNode.m_NodeIndex = 0;

        // update topic index
        lastSelectedNode.m_TopicIndex = a_TopicIndex;
        // check topic out of range
        if (!m_dMap.ContainsKey(lastSelectedNode.m_TopicIndex))
            lastSelectedNode.m_TopicIndex = 0;

        m_saveDataInfo.m_LastSelectedNode = lastSelectedNode;
    }
    #endregion
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    private void LoadData()
    {
        string textPath = AssetPathConstant.FILE_RAW_DB_SCENE;
        TextAsset textAsset = Resources.Load<TextAsset>(textPath);
        // load scene info
        if (textAsset)
        {
            SceneJsonObj sceneJsonObj = JsonUtility.FromJson<SceneJsonObj>(textAsset.text);
            foreach (SceneInfo sceneInfo in sceneJsonObj.m_lSceneInfo)
                m_dSceneInfo.Add(sceneInfo.m_Type, sceneInfo);
        }

        // load hud info
        textPath = AssetPathConstant.FILE_RAW_DB_HUD;
        textAsset = Resources.Load<TextAsset>(textPath);
        if (textAsset)
        {
            HUDJsonObj hudJsonObj = JsonUtility.FromJson<HUDJsonObj>(textAsset.text);
            foreach (HUDInfo HUDInfo in hudJsonObj.m_lHUDInfo)
                m_dHUDInfo.Add(HUDInfo.m_HUDType, HUDInfo);
        }

        // load topic info
        textPath = AssetPathConstant.FILE_RAW_DB_TOPIC;
        textAsset = Resources.Load<TextAsset>(textPath);
        if (textAsset)
        {
            TopicJsonObj topicJsonObj = JsonUtility.FromJson<TopicJsonObj>(textAsset.text);
            m_lTopicInfos = new List<TopicInfo>(topicJsonObj.m_lTopicInfo);
        }

        // load voca info
        textPath = AssetPathConstant.FILE_RAW_DB_VOCA;
        textAsset = Resources.Load<TextAsset>(textPath);
        if (textAsset)
        {
            VocasJsonObj vocasJsonObj = JsonUtility.FromJson<VocasJsonObj>(textAsset.text);
            foreach (VocasInfo vocasInfo in vocasJsonObj.m_lVocasInfo)
                m_dVocaInfos.Add(vocasInfo.m_TopicIndex, vocasInfo);
        }

        // load keyboard info
        textPath = AssetPathConstant.FILE_RAW_DB_KEYBOARD_TEMPLATE;
        textAsset = Resources.Load<TextAsset>(textPath);
        if (textAsset)
        {
            KbJsonObj kbJsonObj = JsonUtility.FromJson<KbJsonObj>(textAsset.text);
            for (int i = 0; i < kbJsonObj.m_lKbTemplate.Count; i++)
            {
                KbTemplateInfo kbInfo = kbJsonObj.m_lKbTemplate[i];
                for (int j = 0; j < kbInfo.m_lKbTemplate.Count; j++)
                {
                    string kbForm = kbInfo.m_lKbTemplate[j];
                    int[,] aKb = DecodeKbTemplate(kbForm);
                    if (j == 0)
                    {
                        List<int[,]> lKb = new List<int[,]>();
                        lKb.Add(aKb);
                        m_dKbTemplate.Add(kbInfo.m_VocaLength, lKb);
                    }
                    else
                    {
                        m_dKbTemplate[kbInfo.m_VocaLength].Add(aKb);
                    }
                }
            }
        }

        // load map
        textPath = AssetPathConstant.FILE_RAW_DB_MAP;
        textAsset = Resources.Load<TextAsset>(textPath);
        if (textAsset)
        {
            MapJsonObj mapJsonObj = JsonUtility.FromJson<MapJsonObj>(textAsset.text);
            foreach (var mapInfo in mapJsonObj.m_lMapInfo)
                m_dMap.Add(mapInfo.m_TopicIndex, mapInfo);
        }

        // load fish
        textPath = AssetPathConstant.FILE_RAW_DB_FISHES;
        textAsset = Resources.Load<TextAsset>(textPath);
        if (textAsset)
        {
            FishJsonObj fishJsonObj = JsonUtility.FromJson<FishJsonObj>(textAsset.text);
            foreach (var fishInfo in fishJsonObj.m_lFishDataInfo)
                m_DictFishInfo.Add(fishInfo.m_FishID, fishInfo);
        }
    }

    private int[,] DecodeKbTemplate(string kbForm)
    {
        // format { key : row,column : index_r1,index_r1,... ; index_r2,index_r2,... ; ... (;; -> full row) }
        // ex: 5:3,3:;1;1
        int totalRowAndColIndex = 1;
        // child
        int totalRowIndex = 0;
        int totalColumnIndex = 1;
        // end child
        int btnIndex = 2;

        string line = kbForm;

        var parts = line.Split(':');
        // 2: get row,column
        Vector2 kbSize = new Vector2(int.Parse(parts[totalRowAndColIndex].Split(',')[totalRowIndex]), int.Parse(parts[totalRowAndColIndex].Split(',')[totalColumnIndex]));
        // 3: get btn index
        int[,] akeyboard = new int[(int)kbSize.x, (int)kbSize.y];
        var rows = line.Split(':')[btnIndex].Split(';');
        for (int k = 0; k < rows.Length; k++)
        {
            var row = rows[k];
            // fill all row
            if (row.Length == 0)
            {
                for (int l = 0; l < (int)kbSize.y; l++)
                    akeyboard[k, l] = 1;
            }
            else
            {
                var columnIndexes = row.Split(',');
                foreach (var columnIndex in columnIndexes)
                {
                    int parseResult = 0;
                    if (int.TryParse(columnIndex, out parseResult))
                        akeyboard[k, parseResult] = 1;
                }
            }
        }
        return akeyboard;
    }
    #endregion
}
