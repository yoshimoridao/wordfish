using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishSpritesMgr : Entity
{
    // ================================== VARIABLES ==================================
    #region Vars
    // private vars
    // dictionary contains sprites of fish
    private Dictionary<Fish.FishState, Sprite[]> m_dFishSprites = new Dictionary<Fish.FishState, Sprite[]>();
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    // ========== OVERRIDE func ==========
    #region OVERRIDE func
    public void Init(SpriteRenderer a_sr, FishInfo a_FishInfo)
    {
        base.Init(a_sr);
        Fish.LoadDictSprites(a_FishInfo, ref m_dFishSprites);
    }

    public override bool IsLoadedSprites()
    {
        return base.IsLoadedSprites() && (m_dFishSprites.Count > 0);
    }
    #endregion

    public void UpdateSprites(Fish.FishState a_FishState)
    {
        LoadSprites(m_dFishSprites[a_FishState]);
    }
    #endregion
}
