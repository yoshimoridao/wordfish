using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ============================== DATA CLASSES ==============================
// ============= SCENE =============
#region Scene
[System.Serializable]
public class ObjLocation
{
    /// <summary>
    /// name of objs
    /// </summary>
    public string m_ObjName;
    /// <summary>
    /// x: position x (% x ratio vs screen width), y: position y (% y ratio vs screen height)
    /// ,width: % width (ratio vs screen width), height: % height (ratio vs screen height)
    /// </summary>
    public Rect m_Rect;
    /// <summary>
    /// value to scale same for width and height
    /// </summary>
    public float m_ScaleSameByY;

    public ObjLocation() { }
    public ObjLocation(ObjLocation a_Copy)
    {
        m_ObjName = a_Copy.m_ObjName;
        m_Rect = a_Copy.m_Rect;
    }
}

[System.Serializable]
public class SceneInfo
{
    public SceneMgr.SceneType m_Type;
    public string m_PrefPath;
    /// <summary>
    /// List of location of elements
    /// </summary>
    public List<ObjLocation> m_lElementLoc = new List<ObjLocation>();

    public SceneInfo() { }
    public SceneInfo(SceneInfo a_Copy)
    {
        m_Type = a_Copy.m_Type;
        m_PrefPath = a_Copy.m_PrefPath;
        m_lElementLoc = new List<ObjLocation>(a_Copy.m_lElementLoc);
    }
}

[System.Serializable]
public class SceneJsonObj
{
    public List<SceneInfo> m_lSceneInfo = new List<SceneInfo>();
}
#endregion

// ============= HUD =============
#region HUD
[System.Serializable]
public class HUDInfo
{
    public HUDMgr.HUDType m_HUDType;
    public string m_PrefPath;
    /// <summary>
    /// List of location of elements
    /// </summary>
    public List<ObjLocation> m_lElementLoc = new List<ObjLocation>();

    public HUDInfo() { }
    public HUDInfo(HUDInfo a_Copy)
    {
        m_HUDType = a_Copy.m_HUDType;
        m_PrefPath = a_Copy.m_PrefPath;
        m_lElementLoc = new List<ObjLocation>(a_Copy.m_lElementLoc);
    }
}

[System.Serializable]
public class HUDJsonObj
{
    public List<HUDInfo> m_lHUDInfo = new List<HUDInfo>();
}
#endregion

// ============= TOPIC =============
#region Topic
[System.Serializable]
public class TopicInfo
{
    public string m_Topic;

    public TopicInfo() { }
    public TopicInfo(TopicInfo a_Copy)
    {
        m_Topic = a_Copy.m_Topic;
    }
}

[System.Serializable]
public class TopicJsonObj
{
    public List<TopicInfo> m_lTopicInfo = new List<TopicInfo>();
}
#endregion

// ============= VOCABULARY =============
#region Vocabulary
[System.Serializable]
public class VocaInfo
{
    public string m_Voca;
    /// <summary>
    /// Definition of vocabulary
    /// </summary>
    public string m_Def;

    public VocaInfo() { }
    public VocaInfo(VocaInfo a_Copy)
    {
        m_Voca = a_Copy.m_Voca;
        m_Def = a_Copy.m_Def;
    }
}

[System.Serializable]
public class VocasInfo
{
    public int m_TopicIndex;
    public List<VocaInfo> m_lVocas = new List<VocaInfo>();

    public VocasInfo() { }
    public VocasInfo(VocasInfo a_Copy)
    {
        m_TopicIndex = a_Copy.m_TopicIndex;
        m_lVocas = new List<VocaInfo>(a_Copy.m_lVocas);
    }
}

[System.Serializable]
public class VocasJsonObj
{
    public List<VocasInfo> m_lVocasInfo = new List<VocasInfo>();
}
#endregion

// ============= KEYBOARD TEMPLATE INFO =============
#region Keyboard Template Info
[System.Serializable]
public class KbTemplateInfo
{
    /// <summary>
    /// Length of vocabularies
    /// </summary>
    public int m_VocaLength;
    /// <summary>
    /// List of template of keyboards
    /// </summary>
    public List<string> m_lKbTemplate = new List<string>();
}

[System.Serializable]
public class KbJsonObj
{
    public List<KbTemplateInfo> m_lKbTemplate = new List<KbTemplateInfo>();
}
#endregion

// ============= MAP INFO =============
#region Node Info
[System.Serializable]
public class NodeSaveInfo
{
    public int m_TopicIndex;
    public int m_NodeIndex;
    public Vector2 m_Progress;

    // ===== CONSTRUCTOR =====
    public NodeSaveInfo() { }
    public NodeSaveInfo(int m_TopicIndex, int m_NodeIndex, Vector2 m_Progress)
    {
        this.m_TopicIndex = m_TopicIndex;
        this.m_NodeIndex = m_NodeIndex;
        this.m_Progress = m_Progress;
    }
    public NodeSaveInfo(NodeSaveInfo a_Copy)
    {
        m_TopicIndex = a_Copy.m_TopicIndex;
        m_NodeIndex = a_Copy.m_NodeIndex;
        m_Progress = a_Copy.m_Progress;
    }

    // ===== OVERRIDE FUNC =====
    public static bool operator ==(NodeSaveInfo a_NodeA, NodeSaveInfo a_NodeB)
    {
        if (a_NodeA.m_NodeIndex == a_NodeB.m_NodeIndex &&
            a_NodeA.m_TopicIndex == a_NodeB.m_TopicIndex &&
            a_NodeA.m_Progress == a_NodeB.m_Progress)
            return true;
        return false;
    }
    public static bool operator !=(NodeSaveInfo a_NodeA, NodeSaveInfo a_NodeB)
    {
        if (a_NodeA.m_NodeIndex == a_NodeB.m_NodeIndex &&
            a_NodeA.m_TopicIndex == a_NodeB.m_TopicIndex &&
            a_NodeA.m_Progress == a_NodeB.m_Progress)
            return false;
        return true;
    }
}

[System.Serializable]
public class NodeInfo
{
    public int m_TopicIndex;
    public string m_FishIndex;
    public int m_KbIndex;
    /// <summary>
    /// list index of vocabularies
    /// </summary>
    public List<int> m_lVocaIndex = new List<int>();

    public NodeInfo() { }
    public NodeInfo(NodeInfo a_Copy)
    {
        m_TopicIndex = a_Copy.m_TopicIndex;
        m_FishIndex = a_Copy.m_FishIndex;
        m_KbIndex = a_Copy.m_KbIndex;
        m_lVocaIndex = new List<int>(a_Copy.m_lVocaIndex);
    }
}

[System.Serializable]
public class MapInfo
{
    public int m_TopicIndex;
    public List<NodeInfo> m_lNodes = new List<NodeInfo>();

    public MapInfo() { }
    public MapInfo(MapInfo a_Copy)
    {
        m_TopicIndex = a_Copy.m_TopicIndex;
        m_lNodes = new List<NodeInfo>(a_Copy.m_lNodes);
    }
}

[System.Serializable]
public class MapJsonObj
{
    public List<MapInfo> m_lMapInfo = new List<MapInfo>();
}
#endregion

// ============= FISH INFO =============
#region Fish Info
[System.Serializable]
public class FishLevelInfo
{
    public Fish.FishLevel m_FishLevel;
    public float m_FishXP;
    public float m_MaxHP;
    public float m_LifeTime;
    public int m_Money;
    public float m_ProductTime;
    public float m_FishDam;

    public FishLevelInfo() { }
    public FishLevelInfo(FishLevelInfo a_Copy)
    {
        m_FishLevel = a_Copy.m_FishLevel;
        m_FishXP = a_Copy.m_FishXP;
        m_MaxHP = a_Copy.m_MaxHP;
        m_LifeTime = a_Copy.m_LifeTime;
        m_Money = a_Copy.m_Money;
        m_ProductTime = a_Copy.m_ProductTime;
        m_FishDam = a_Copy.m_FishDam;
    }
}

[System.Serializable]
public class FishDataInfo
{
    public string m_Identifier;
    public string m_FishID;
    public Fish.FishKind m_FishKind;
    public Fish.FishRank m_FishRank;
    public int m_FishRankID;
    public Fish.FishSex m_FishSex;
    public Fish.ShapeOfFish m_Shape;
    public Fish.ColorOfFish m_Color;
    public List<Fish.FishState> m_lFishStates;

    public List<FishLevelInfo> m_lLevelInfo = new List<FishLevelInfo>();

    public FishDataInfo() { }
    public FishDataInfo(FishDataInfo a_Copy)
    {
        m_FishKind = a_Copy.m_FishKind;
        m_FishRank = a_Copy.m_FishRank;
        m_FishSex = a_Copy.m_FishSex;
        m_FishRankID = a_Copy.m_FishRankID;
        m_Shape = a_Copy.m_Shape;
        m_Color = a_Copy.m_Color;
        m_Identifier = a_Copy.m_Identifier;
        m_FishID = a_Copy.m_FishID;
        m_lLevelInfo = new List<FishLevelInfo>(a_Copy.m_lLevelInfo);
    }
}

[System.Serializable]
public class FishJsonObj
{
    public List<FishDataInfo> m_lFishDataInfo = new List<FishDataInfo>();
}
#endregion