using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GObject : MonoBehaviour
{
    public virtual void OnCreateObj()
    {

    }

    public virtual void OnUpdateObj(float a_dt)
    {

    }

    public virtual void OnDestroyObj()
    {
        SelfDestroy();
    }

    public virtual void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public virtual bool IsActive()
    {
        return gameObject.activeSelf;
    }

    /// <summary>
    /// This is a temporary method, we need to change
    /// </summary>
    public virtual void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
