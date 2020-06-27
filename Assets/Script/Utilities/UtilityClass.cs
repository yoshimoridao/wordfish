using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

public static class UtilityClass
{
    public class SortSpriteNameAscending : IComparer<Sprite>
    {
        public int Compare(Sprite a, Sprite b)
        {
            int iA = int.Parse(a.name);
            int iB = int.Parse(b.name);
            if (iA > iB)
                return 1;
            else if (iA < iB)
                return -1;
            else
                return 0;
        }
    }

    static CenterSpace.Free.MersenneTwister m_Random;

	static UtilityClass ()
	{
		m_Random = new CenterSpace.Free.MersenneTwister ();
	}

	public static float GetRandomFloat (float min, float max)
	{
		return UnityEngine.Random.Range (min, max);
	}

	public static int GetRandomFromRange (int min, int max)
	{
		return m_Random.Next (min, max);
	}

    // === TRANSFORM ===
    public static void SetLossyScale(ref GameObject a_Obj, Vector3 a_Scale)
    {
        Transform parent = a_Obj.transform.parent;
        a_Obj.transform.parent = null;
        a_Obj.transform.localScale = a_Scale;
        a_Obj.transform.parent = parent;
    }

    // === EVENT BTN ===
    public static void ResetSelectedGameObject()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    // === ENUM ===
    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static string GetPathOfObj(GameObject obj)
    {
        UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = path.Replace("Assets/Resources/", "");
        path = path.Replace(".prefab", "");
        return path;
    }

    public static void ShuffleList<T>(ref List<T> a_list)
    {
        // Shuffle vocas list
        for (int i = 0; i < a_list.Count; i++)
        {
            int swapIndex = UnityEngine.Random.RandomRange(0, a_list.Count);
            var temp = a_list[i];
            a_list[i] = a_list[swapIndex];
            a_list[swapIndex] = temp;
        }
    }
}
