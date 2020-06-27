using UnityEngine;
using System.Collections;

public class FishFactory : MonoBehaviour {

	//public delegate Fish GenerateFish (int id);
	#region fish param
	//FishInfoFactory m_InfoFactory; //no longer use
	#endregion

    #region reference
    public GameObject FishPrefab;
    #endregion

    void Awake()
    {
        InitFishInfo();
    }

	/*public*/private void InitFishInfo ()
	{
		//m_InfoFactory = new FishInfoFactory ();
        //m_InfoFactory.CreateNecessaryDirectories (); // only for unity editor
        //m_InfoFactory.LoadFishInfo();

        // load text file from resources
        //m_InfoFactory.LoadFishInfoFromResources();
	}

	//public Fish GenerateFish (int id, Transform parent)
    public Fish GenerateFish (string id, Transform parent)
	{
		//Fish fish = new Fish ();
        GameObject obj = Instantiate(FishPrefab, parent) as GameObject;
        //obj.transform.SetParent(transform);

        Fish fish = obj.GetComponent<Fish>();
		//FishInfo info = m_InfoFactory.CloneFishInfo (fishID: id);
        FishInfo info = DbMgr.s_Instance.GetFishInfo(id);
		fish.InitFish (info);
        fish.InitFishUI();

        if (info.m_FishID != "")
        {
            Sprite newsprite = Resources.Load<Sprite>(AssetPathConstant.FOLDER_FISH_PATH + "/" + info.m_FishID);
            SpriteRenderer render = obj.GetComponent<SpriteRenderer>();
            render.sprite = newsprite;

            Vector2 pixelSize = render.sprite.rect.size;
            Vector2 units = pixelSize / render.sprite.pixelsPerUnit;

            fish.SetUIOffetY(units.y / 2 + Constant.FISH_UI_OFFSETY);
        }

        fish.ActivateUI(true);
		return fish;
		//FishInfo info = 
	}

	/// <summary>
	/// Merge fish 
	/// </summary>
	public Fish MergeFish (Fish fish1, Fish fish2)
	{
		Fish.FishRank rank = fish1 + fish2;

		return null;
	}

    void OnEnable ()
    {
        TankController.EventRequestFish += GenerateFish;
    }

    void OnDisable ()
    {
        TankController.EventRequestFish -= GenerateFish;
    }
}
