using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json.Linq;

public class BaseMono : MonoBehaviour {

	public virtual void SetActive (bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public virtual bool IsActive ()
    {
        return gameObject.activeSelf;
    }

    /// <summary>
    /// This is a temporary method, we need to change
    /// </summary>
    public virtual void SelfDestroy ()
    {
        Destroy(gameObject);
    }
}
