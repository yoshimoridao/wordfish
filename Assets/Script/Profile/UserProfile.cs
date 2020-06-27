using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json.Linq;

public class UserProfile
{

    #region profile fields
    string m_OldProfile;
    string m_NewProfile;

    JObject m_JSProfile;

    static UserProfile m_Instance;
    #endregion

    #region properties
    public string PProfileString
    {
        get
        {
            if (m_OldProfile == null)
            {
                //m_OldProfile = "{\"ListTank\":[{\"Tank1\":[\"010106\",\"010203\",\"030102\"]}]}";
                m_OldProfile = "{\"ListTank\":[{\"Tank1\":[\"010106\",\"010203\",\"030102\"]}]}";
            }
            return m_OldProfile;
        }

        set
        {

        }
    }
    #endregion

    #region public methods
    public List<string> GetListTank()
    {
        List<string> tanks = new List<string>();

        if (m_JSProfile != null)
        {
            if (m_JSProfile[Constant.JS_LISTTANK].HasValues)
            {
               // JObject o = m_JSProfile[Constant.JS_LISTTANK].ToObject<JObject>();
                //JArray a = JArray.FromObject(o);
                JArray a = (JArray)m_JSProfile[Constant.JS_LISTTANK];
                if (a != null)
                {
                    foreach (JObject temp in a.Children<JObject>())
                    {
                        foreach (JProperty property in temp.Properties())
                        {
                            string tank = string.Copy(property.Name);
                            tanks.Add(tank);
                        }
                    }
                }
            }
        }

        return tanks;
    }

    public List<string> GetListFishInTank (string tankid)
    {
        List<string> fishes = new List<string>();

        if (m_JSProfile != null)
        {
            //JObject o = m_JSProfile[Constant.JS_LISTTANK].ToObject<JObject>();
            //JObject o = m_JSProfile[tankid].ToObject<JObject>();
            JArray a = (JArray)m_JSProfile[Constant.JS_LISTTANK];
            bool shouldBreak = false;
            if (a != null)
            {
                foreach (JObject o in a.Children<JObject>())
                {
                    foreach (JProperty property in o.Properties())
                    {
                        if (string.Compare(tankid, property.Name) == 0)
                        {
                            JArray temp = (JArray)o[tankid];
                            for (int i = 0; i < temp.Count; ++i)
                            {
                                string value = (string)temp[i];
                                fishes.Add(value);
                            }
                            shouldBreak = true;
                            break;
                        }
                    }
                    if (shouldBreak)
                        break;
                }
            }
        }

        return fishes;
    }

    public void RefreshProfile()
    {
        if (m_OldProfile != null)
            m_JSProfile = JObject.Parse(m_OldProfile);
    }

    public static UserProfile GetInstane()
    {
        if (m_Instance == null)
            m_Instance = new UserProfile();
        return m_Instance;
    }

    public bool IsProfileChange()
    {
        return !string.Equals(m_OldProfile, m_NewProfile);
    }

    public void UpdateProfile()
    {
        m_NewProfile = string.Copy(m_OldProfile);
        RefreshProfile();
    }
    #endregion


    #region private methods
    UserProfile()
    {
        m_OldProfile = "{\"ListTank\":[{\"Tank1\":[\"010106\",\"010203\",\"030102\"]}]}";
    }
    #endregion
}
