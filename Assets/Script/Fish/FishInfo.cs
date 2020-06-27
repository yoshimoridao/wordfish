using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class FishInfo
{
    //[SerializeField]
    public string m_FishID;
    public int m_TopicID;
    public string m_Identifier;

    public Fish.FishKind m_FishKind;
    public Fish.FishRank m_FishRank;
    public int m_FishRankID;
    public Fish.FishSex m_FishSex;
    public FishShape m_FishShape;

    public FishXP m_FishXP;
    public FishHP m_FishHP;
    public MoneyGeneration m_Money;
    public FishDam m_FishDam;
    public Fish.FishState m_CurFishState;
    public List<Fish.FishState> m_lFishStates = new List<Fish.FishState>();

    public FishInfo() { }
    public FishInfo(FishDataInfo dataFishInfo)
    {
        m_FishRankID = dataFishInfo.m_FishRankID;
        //m_TopicID = dataFishInfo.m_TopicID;
        m_Identifier = dataFishInfo.m_Identifier;
        m_FishID = dataFishInfo.m_FishID;
        m_FishRank = dataFishInfo.m_FishRank;
        m_FishKind = dataFishInfo.m_FishKind;
        m_FishSex = dataFishInfo.m_FishSex;
        m_FishShape = new FishShape(dataFishInfo.m_Shape, dataFishInfo.m_Color);
        m_lFishStates = dataFishInfo.m_lFishStates;

        // default get level of baby fish
        if (dataFishInfo.m_lLevelInfo.Count >= 0)
        {
            FishLevelInfo babyInfo = dataFishInfo.m_lLevelInfo[0];
            UpdateFishLevelInfo(babyInfo);
        }
    }

    public FishInfo CloneFishInfo()
    {
        FishInfo info = new FishInfo();

        info.m_FishRankID = this.m_FishRankID;
        info.m_TopicID = this.m_TopicID;
        info.m_Identifier = this.m_Identifier;
        info.m_FishID = this.m_FishID;

        info.m_FishRank = this.m_FishRank;
        info.m_FishKind = this.m_FishKind;
        info.m_FishSex = this.m_FishSex;

        info.m_FishXP = this.m_FishXP.CloneFishXP();
        info.m_FishHP = this.m_FishHP.CloneFishHP();
        info.m_FishShape = this.m_FishShape.CloneFishShape();
        info.m_FishDam = this.m_FishDam.CloneFishDam();
        info.m_Money = this.m_Money.CloneMoneyGeneration();

        return info;
    }
    public void UpdateFishLevelInfo(FishLevelInfo fishLevelInfo)
    {
        m_FishXP = new FishXP(fishLevelInfo.m_FishLevel, fishLevelInfo.m_FishXP);
        m_FishHP = new FishHP(fishLevelInfo.m_MaxHP, fishLevelInfo.m_LifeTime);
        m_Money = new MoneyGeneration(fishLevelInfo.m_Money, fishLevelInfo.m_ProductTime);
        m_FishDam = new FishDam(fishLevelInfo.m_FishDam);
    }

    /// <summary>
    /// Merge fish condition
    /// </summary>
    public static Fish.FishRank operator +(FishInfo info1, FishInfo info2)
    {
        // check sex condition
        if (info1.m_FishSex == info2.m_FishSex)
            return Fish.FishRank.None;
        if (info1.m_FishSex == Fish.FishSex.None || info2.m_FishSex == Fish.FishSex.None)
            return Fish.FishRank.None;

        // check fish progress condition
        if (info1.m_FishXP.m_FishLevel != info2.m_FishXP.m_FishLevel)
            return Fish.FishRank.None;

        // check fish rank condition
        if (info1.m_FishRank != info2.m_FishRank)
            return Fish.FishRank.None;

        int total = (int)Fish.FishRank.Total;
        int current = (int)info1.m_FishRank;
        int newRank = current + 1;
        if (newRank >= total)
            return Fish.FishRank.None;

        Fish.FishRank rank = (Fish.FishRank)newRank;
        return rank;
    }

    public Fish.FishRank GetRank()
    {
        return m_FishRank;
    }

}


[Serializable]
public class FishXP
{
    public float m_CurrentFishXP;
    public float m_TargetLevel;
    //public float m_AdultLevel;
    public Fish.FishLevel m_FishLevel;

    public FishXP()
    {

    }
    public FishXP(Fish.FishLevel lv, float targetLevel)
    {
        m_FishLevel = lv;
        m_TargetLevel = targetLevel;
    }

    public FishXP CloneFishXP()
    {
        FishXP xp = new FishXP();

        xp.m_CurrentFishXP = m_CurrentFishXP;
        xp.m_TargetLevel = m_TargetLevel;
        //xp.m_AdultLevel = m_AdultLevel;
        xp.m_FishLevel = m_FishLevel;

        return xp;
    }
}


/// <summary>
/// Fish HP info and controller
/// </summary>
[Serializable]
public class FishHP
{
    public float m_CurrentHP;
    public float m_MaxHP;
    public float m_CurrentTime;
    public float m_MaxTime;
    public float m_WithDraw;

    public FishHP()
    {
    }

    public FishHP(float maxHp, float maxTime)
    {
        m_MaxHP = maxHp;
        m_MaxTime = maxTime;
    }
    public FishHP CloneFishHP()
    {
        FishHP hp = new FishHP();

        hp.m_CurrentHP = m_CurrentHP;
        hp.m_MaxHP = m_MaxHP;
        hp.m_CurrentTime = m_CurrentTime;
        hp.m_MaxTime = m_MaxTime;
        hp.m_WithDraw = m_WithDraw;

        return hp;
    }
}


/// <summary>
/// Fish shape and controller
/// </summary>
[Serializable]
public class FishShape
{
    //public Vector3 m_Color;
    public Fish.ColorOfFish m_Color;
    public Fish.ShapeOfFish m_Shape;

    public FishShape()
    {

    }
    public FishShape(Fish.ShapeOfFish shape, Fish.ColorOfFish color)
    {
        m_Shape = shape;
        m_Color = color;
    }

    public FishShape CloneFishShape()
    {
        FishShape shape = new FishShape();
        shape.m_Color = this.m_Color;
        shape.m_Shape = this.m_Shape;
        return shape;
    }
}


/// <summary>
/// Fish damage calculation
/// </summary>
[Serializable]
public class FishDam
{
    public float m_FishDam;

    public FishDam() { }
    public FishDam(float fishDam)
    {
        m_FishDam = fishDam;
    }
    public FishDam CloneFishDam()
    {
        FishDam dam = new FishDam();
        dam.m_FishDam = m_FishDam;

        return dam;
    }
}



/// <summary>
/// money generation from fish
/// </summary>
[Serializable]
public class MoneyGeneration
{
    public int m_Money;
    public float m_CurrentTime;
    public float m_ProductTime;

    public MoneyGeneration()
    {

    }

    public MoneyGeneration(int money, float productime)
    {
        m_Money = money;
        m_ProductTime = productime;

    }
    public MoneyGeneration CloneMoneyGeneration()
    {
        MoneyGeneration money = new MoneyGeneration();
        money.m_Money = m_Money;
        money.m_CurrentTime = m_CurrentTime;
        money.m_ProductTime = m_ProductTime;
        return money;
    }
}



[Serializable]
class FishInfoList
{
    public List<FishInfo> FishList;

    //	public FishInfoList ()
    //	{
    //		FishList = new List<FishInfo> ();
    //	}
    List<FishInfo> m_CommonList;
    List<FishInfo> m_MediumList;
    List<FishInfo> m_HighList;
    List<FishInfo> m_LegendList;

    public FishInfo this[int index]
    {
        get
        {
            return FishList[index];
        }
        set
        {
            FishList[index] = value;
        }
    }

    private static int SortIncrementByFishID(FishInfo info1, FishInfo info2)
    {
        if (info1.m_FishRankID < info2.m_FishRankID)
            return -1;

        if (info1.m_FishRankID == info2.m_FishRankID)
            return 0;

        return 1;
    }

    private static int SortDecrementByFishID(FishInfo info1, FishInfo info2)
    {
        if (info1.m_FishRankID > info2.m_FishRankID)
            return -1;

        if (info1.m_FishRankID == info2.m_FishRankID)
            return 0;

        return 1;
    }

    public void SortFishInfoList(bool increment = true)
    {
        if (increment)
            FishList.Sort(SortIncrementByFishID);
        else
            FishList.Sort(SortDecrementByFishID);
    }

    public void ClassifyFish()
    {
        if (m_CommonList == null)
            m_CommonList = new List<FishInfo>();

        if (m_MediumList == null)
            m_MediumList = new List<FishInfo>();

        if (m_HighList == null)
            m_HighList = new List<FishInfo>();

        if (m_LegendList == null)
            m_LegendList = new List<FishInfo>();

        foreach (FishInfo fish in FishList)
        {
            switch (fish.m_FishRank)
            {
                case Fish.FishRank.Common:
                    m_CommonList.Add(fish);
                    break;
                case Fish.FishRank.Medium:
                    m_MediumList.Add(fish);
                    break;
                case Fish.FishRank.High:
                    m_HighList.Add(fish);
                    break;
                case Fish.FishRank.Legend:
                    m_LegendList.Add(fish);
                    break;
            }
        }
    }

    public FishInfo GetRandomFromRank(Fish.FishRank rank)
    {
        int index = 0;
        FishInfo info = null;

        switch (rank)
        {
            case Fish.FishRank.Common:
                index = UtilityClass.GetRandomFromRange(0, m_CommonList.Count - 1);
                info = m_CommonList[index];
                break;

            case Fish.FishRank.Medium:
                index = UtilityClass.GetRandomFromRange(0, m_MediumList.Count - 1);
                info = m_MediumList[index];
                break;

            case Fish.FishRank.High:
                index = UtilityClass.GetRandomFromRange(0, m_HighList.Count - 1);
                info = m_HighList[index];
                break;

            case Fish.FishRank.Legend:
                index = UtilityClass.GetRandomFromRange(0, m_LegendList.Count - 1);
                info = m_LegendList[index];
                break;
        }

        return info;
    }

}