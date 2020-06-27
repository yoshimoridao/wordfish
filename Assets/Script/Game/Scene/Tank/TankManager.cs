using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankManager : MonoBehaviour {
	
	#region fish param
	//FishFactory m_FishFactory;

	/// Temporarily manually add these scripts for tank controller
	List<TankController> m_Tanks;
	int m_Quota = 1;
	/// end
	#endregion

    #region Prefabs or object reference
    public GameObject TankPrefab;
    //public GameObject FishFactoryRef;
    #endregion

    bool m_IsInitTanks = false;

    void Awake ()
	{
		///Fake scripts
        if (m_Tanks == null)
        {
            m_Tanks = new List<TankController>();

            for (int i = 0; i < m_Quota; ++i)
            {
                //TankController controller = new TankController();
                GameObject obj = Instantiate(TankPrefab) as GameObject;
                obj.transform.SetParent(transform);
                TankController controller = obj.GetComponent<TankController>();
                //controller.PassTankManagerReference(this);
                m_Tanks.Add(controller);
            }
        }
		/// end 

		/// Load fish info
        //if (m_FishFactory == null) {
        //    //m_FishFactory = new FishFactory ();
        //    m_FishFactory = FishFactoryRef.GetComponent<FishFactory>();
			//m_FishFactory.InitFishInfo ();
		//}
		//m_FishFactory
        
        /// Init Tank
        //StartCoroutine(InitTank());
	}

    IEnumerator InitTank ()
    {
        for (int i = 0; i < m_Tanks.Count; ++i)
        {
            yield return m_Tanks[i].InitFishInTank();
        }
    }

    public void Update ()
    {
        if (!m_IsInitTanks)
        {
            StartCoroutine(InitTank());
            m_IsInitTanks = true;
        }
        else
        {
            //float time = Time.deltaTime;
            //foreach (var tank in m_Tanks)
            //    tank.UpdateTank(time);
        }
    }
}
