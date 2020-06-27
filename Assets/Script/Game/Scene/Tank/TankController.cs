using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankController : GObject//BaseMono
{
    // ================================== VARIABLES ==================================
    #region Vars
    List<int> m_ListFishID; // fake params

    List<Fish> m_Fishes;
    List<string> m_ListFish;

    string m_TankID;
    #endregion

    // =========================== DELEGATE EVENT FUNCS ===========================
    #region Delegate Funcs
    //public delegate Fish RequestFish(int id, Transform parent);
    public delegate Fish RequestFish(string id, Transform parent);
    public static event RequestFish EventRequestFish;
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    void InitFakeParams ()
    {
        m_ListFishID = new List<int>();
        //m_ListFishID.Add(0);
        //m_ListFishID.Add(1);
        //m_ListFishID.Add(2);
    }
    #endregion

    // ================================== PUBLIC FUNCS ==================================
    #region Public Funcs
    public void SetTankID (string id)
    {
        m_TankID = id;
    }

    public string GetTankId ()
    {
        return m_TankID;
    }

    public void SetFishList (List<string> ids)
    {
        m_ListFish = ids;
    }

    public IEnumerator InitFishInTank()
    {
        //for (int i = 0; i < m_ListFishID.Count; ++i)
        //{
        //    Fish fish = EventRequestFish(m_ListFishID[i], transform);
        //    m_Fishes.Add(fish);
        //    yield return null;
        //}
        Debug.Log("Fish count: " + m_ListFish.Count);
        for (int i = 0; i < m_ListFish.Count; ++i)
        {
            Fish fish = EventRequestFish(m_ListFish[i], transform);
            m_Fishes.Add(fish);
            yield return null;
        }
    }
    #endregion

    public void UpdateListFish (List<string> ids)
    {
        SelfDestroy();
        m_ListFish = ids;
    }
    // =================================== OVERRIDE func ===================================
    #region override methods
    public override void OnCreateObj()
    {
        base.OnCreateObj();

        InitFakeParams();

        if (m_Fishes == null)
            m_Fishes = new List<Fish>();

        if (m_ListFish == null)
            m_ListFish = new List<string>();
    }

    public override void OnUpdateObj(float a_dt)
    {
        //base.OnUpdateObj(a_dt);
        foreach (Fish fish in m_Fishes)
            fish.OnUpdateObj(a_dt);
            //fish.UpdateFish(a_dt);
    }

    public override void SelfDestroy()
    {
        for (int i = 0; i < m_Fishes.Count; ++i)
            m_Fishes[i].SelfDestroy();

        m_Fishes.Clear();
        base.SelfDestroy();
    }
    #endregion
}
