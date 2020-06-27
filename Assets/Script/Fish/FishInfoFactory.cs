using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FishInfoFactory {

	const string FILE_FISH_INFO_SAVE = @"Assets\JsonData\" + Constant.FISH_INFO_ROOT;
	const string FILE_FISH_INFO_DIR  = @"Assets\JsonData";

	/// <summary>
	/// The fish list with all and classified by ranks
	/// </summary>
	FishInfoList m_FishList;

    public void LoadFishInfoFromResources()
    {
        TextAsset fishInfoText = Resources.Load<TextAsset>(AssetPathConstant.FOLDER_DB_PATH + "/" + AssetPathConstant.FILE_FISH_INFO_SAVE);
        if (fishInfoText)
        {
            m_FishList = JsonUtility.FromJson<FishInfoList>(fishInfoText.text);
            m_FishList.SortFishInfoList();
            m_FishList.ClassifyFish();

            FishInfo temp = m_FishList[0];
        }
    }

    /// <summary>
    /// Load data from network, currently load from local
    /// </summary>
    public void LoadFishInfo ()
	{
		if (File.Exists(FILE_FISH_INFO_SAVE))
        {
          	m_FishList = JsonUtility.FromJson<FishInfoList>(File.ReadAllText(FILE_FISH_INFO_SAVE, System.Text.Encoding.UTF8));
			m_FishList.SortFishInfoList ();
			m_FishList.ClassifyFish ();

            FishInfo temp = m_FishList[0];
        }
	}
		
	/// <summary>
	/// Fake function to save
	/// </summary>
	public void SaveFishInfo ()
	{
		 if (File.Exists(FILE_FISH_INFO_SAVE))
		     File.Delete(FILE_FISH_INFO_SAVE);

		{
		     FileStream fs = File.Create(FILE_FISH_INFO_SAVE);
		     fs.Close();
		 }

		File.WriteAllText(FILE_FISH_INFO_SAVE, JsonUtility.ToJson(m_FishList));	
	}

	/// <summary>
	/// Create directory to load/save in untiy editor
	/// </summary>
	public void CreateNecessaryDirectories ()
	{
		if (!Directory.Exists (FILE_FISH_INFO_DIR))
			Directory.CreateDirectory (Path.GetDirectoryName (FILE_FISH_INFO_DIR));
	}

	public FishInfo CloneFishInfo (int fishID)
	{
		return m_FishList[fishID].CloneFishInfo();
	}

	void OnEnable ()
	{
		LoadFishInfo ();
	}

	void OnDisable ()
	{
		SaveFishInfo ();
	}

	void Awake ()
	{
		// only for unity editor
		CreateNecessaryDirectories ();
	}
}
