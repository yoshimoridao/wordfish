using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMgr : GScene
{
	#region var
    public GameObject Pre_Tank;

	List<TankController> m_Tanks;
    List<string> m_ListTankIDs;

	int m_Quota = 1;
    bool m_IsInitTanks = false;
    bool m_ShouldUpdateTankProfile = false;
	#endregion
    

    // =================================== OVERRIDE func ===================================
    #region OVERRIDE func
    // override GObject
    public override void OnCreateObj()
    {
        base.OnCreateObj();

        if (m_ListTankIDs == null)
            m_ListTankIDs = new List<string>();

        if (m_Tanks == null)
        {
            m_Tanks = new List<TankController>();

            if (Pre_Tank != null)
            {
                for (int i = 0; i < m_Quota; ++i)
                {
                    //TankController controller = new TankController();
                    GameObject obj = Instantiate(Pre_Tank) as GameObject;
                    obj.transform.SetParent(transform);
                    TankController controller = obj.GetComponent<TankController>();
                    controller.OnCreateObj();
                    string id = Constant.TANK + (i + 1);
                    controller.SetTankID(id);
                    controller.SetActive(false);
                    //controller.PassTankManagerReference(this);
                    m_Tanks.Add(controller);
                }
            }
        }

    }

    public override void OnUpdateObj(float a_dt)
    {
        base.OnUpdateObj(a_dt);
        //if (!m_IsInitTanks)
        //{
        //    StartCoroutine(InitTank());
        //    m_IsInitTanks = true;
        //}
        //else
        //{
        //    float time = Time.deltaTime;
        //    foreach (var tank in m_Tanks)
        //        tank.OnUpdateObj(time);
        //        //tank.UpdateTank(time);
        //}
        if (m_IsInitTanks)
        {
            StartCoroutine(InitTank());
            m_IsInitTanks = false;
        }

        if (m_ShouldUpdateTankProfile)
        {
            m_ShouldUpdateTankProfile = false;
            m_IsInitTanks = true;
            m_ListTankIDs = UserProfile.GetInstane().GetListTank();
        }

        if (UserProfile.GetInstane().IsProfileChange())
        {
            UserProfile.GetInstane().UpdateProfile();
            m_ShouldUpdateTankProfile = true;
        }

        float time = Time.deltaTime;
        foreach (var tank in m_Tanks)
        {
            if (tank.IsActive())
                tank.OnUpdateObj(time);
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
    }
    #endregion

    // ================================== PRIVATE FUNCS ==================================
    #region Private Funcs
    IEnumerator InitTank ()
    {
        for (int i = 0; i < m_ListTankIDs.Count; ++i)
        {
            Debug.Log("mTanks count: " + m_Tanks.Count);
            //foreach (var tank in m_Tanks)
            for (int j = 0; j < m_Tanks.Count; ++j)
            {
                Debug.Log("mTanks id: " + m_Tanks[j].GetTankId());
                Debug.Log("List tank id: " + m_ListTankIDs[i]);
                if (string.Equals(m_Tanks[j].GetTankId(), m_ListTankIDs[i]))
                {
                    if (!m_Tanks[j].IsActive())
                        m_Tanks[j].SetActive(true);

                    List<string> fishes = UserProfile.GetInstane().GetListFishInTank(m_Tanks[j].GetTankId());
                    m_Tanks[j].SetFishList(fishes);
                    yield return m_Tanks[j].InitFishInTank();
                }
                else
                    yield return null;
            }
            //yield return m_Tanks[i].InitFishInTank();
        }
    }

    #endregion
}
