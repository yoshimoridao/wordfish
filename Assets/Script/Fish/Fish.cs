using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Fish : GObject//BaseMono
{
    #region Enum param for Fish
    public enum FishRank
    {
		None,
        Common,
        Medium,
        High,
        Legend,
		Total
    }

    public enum FishKind
    {
        Node,
        Boss
    }

    public enum FishSex
    {
		None,
        Male,
        Female
    }

    public enum FishLevel
    {
        Baby,
        Youth,
        Adult,
        Total
    }

    public enum ShapeOfFish
    {
        Normal,
        Rect,
        Angel,
        Circle
    }

    public enum ColorOfFish
    {
        Red,
        Yellow,
        Blue,
        BluePink,
        Pink,
        Purple,
        Green,
        Cyan
    }

    public enum FishState
    {
        Stand,
        Move,
        Attack,
        Hit,
        Die
    }
    #endregion 

    #region Fish info and handler

	int m_FishID;

    FishInfo m_Info;
    FishHandler m_Handler;

    #endregion

	#region Fish UIComponent

    HPComponent m_HP;
	
	#endregion

    #region Delegate Events
    public delegate void ProcessUI(Transform follow, HUDTankMgr.UITypes type, out MonoBehaviour controller);
    public static event ProcessUI EventProcessUI;
    #endregion

    // ================================== STATIC FUNCS ==================================
    #region Static Funcs
    /// <summary>
    /// Merge fish with conditions
    /// </summary>
    public static Fish.FishRank operator +(Fish fish1, Fish fish2)
    {
        return fish1.m_Info + fish2.m_Info;
    }

    public static string GetFishId(FishInfo a_fishInfo)
    {
        // fishes (Key in DB = FishKind_FishRank_FishID_FishSex_ShapeOfFish_FishColor)
        string spriteName = ((int)a_fishInfo.m_FishKind).ToString()
            + ((int)a_fishInfo.m_FishRank).ToString()
            + a_fishInfo.m_FishRankID
            + ((int)a_fishInfo.m_FishSex).ToString()
            + ((int)a_fishInfo.m_FishShape.m_Shape).ToString()
            + ((int)a_fishInfo.m_FishShape.m_Color).ToString();

        return spriteName;
    }

    public static string GetFishId(FishDataInfo a_fishInfo)
    {
        // fishes (Key in DB = FishKind_FishRank_FishID_FishSex_ShapeOfFish_FishColor)
        string spriteName = ((int)a_fishInfo.m_FishKind).ToString()
            + ((int)a_fishInfo.m_FishRank).ToString()
            + a_fishInfo.m_FishRankID
            + ((int)a_fishInfo.m_FishSex).ToString()
            + ((int)a_fishInfo.m_Shape).ToString()
            + ((int)a_fishInfo.m_Color).ToString();

        return spriteName;
    }
    
    public static string GetFolderSpritesPath(string a_FishId, FishState a_FishState)
    {
        string folderFishPath = AssetPathConstant.FOLDER_FISH_PATH + "/" + a_FishId + "/" + a_FishState.ToString().ToLower();
        return folderFishPath;
    }

    public static void LoadDictSprites(FishInfo a_FishInfo, ref Dictionary<FishState, Sprite[]> a_dSprites)
    {
        for (int i = 0; i < a_FishInfo.m_lFishStates.Count; i++)
        {
            FishState fishState = a_FishInfo.m_lFishStates[i];
            string folderPath = GetFolderSpritesPath(a_FishInfo.m_FishID, fishState);
            Sprite[] aSprites = Resources.LoadAll<Sprite>(folderPath);
            if (aSprites.Length > 0)
            {
                a_dSprites.Add(fishState, aSprites);
            }
        }
    }

    public static Sprite GetFishCard(string a_FishID)
    {
        string cardPath = AssetPathConstant.FOLDER_FISH_CARD_PATH + "/" + a_FishID;
        Sprite fishCard = Resources.Load<Sprite>(cardPath);
        return fishCard;
    }
    #endregion

    #region public methods
    public void InitFish (FishInfo info)
	{
		m_Info = info;

		InitFishHandler ();
	}

    public void InitFishUI()
    {
        //DelegateEvent.EventProcessUI(transform, HUDController.UITypes.HPComponent, out m_Component);
        MonoBehaviour temp = m_HP as MonoBehaviour;
        if (Fish.EventProcessUI != null)
            Fish.EventProcessUI(transform, HUDTankMgr.UITypes.HPComponent, out temp);
        m_HP = temp as HPComponent;
    }

    public void SetUIOffetY (float value)
    {
        if (m_HP != null)
            m_HP.OffsetY = value;
    }

    public void ActivateUI (bool value)
    {
        if (m_HP != null)
			m_HP.SetActive(value);
    }

    //public void UpdateFish(float delta)
    //{
    //    //float deltatime = Time.deltaTime;

    //    m_Handler.UpdateFishStatus(delta);

    //    if (m_HP != null)
    //        m_HP.UpdatePosition();
    //}

    //public void FixedUpdateFish(float delta)
    //{
    //    m_Handler.FixedUpdateFishStatus(delta);
    //}
    #endregion

    #region private methods
    private void InitFishHandler ()
	{
		m_Handler = new FishHandler (m_Info, transform);
		//Transform
    }
    #endregion

    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
    public override void OnUpdateObj(float a_dt)
    {
        //base.OnUpdateObj(a_dt);
        //m_Handler.UpdateFishStatus(a_dt);
        m_Handler.UpdateHandler(a_dt);

        if (m_HP != null)
            m_HP.UpdatePosition();
    }
    #endregion
}
