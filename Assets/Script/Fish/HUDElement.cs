using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDElement : GObject//BaseMono 
{
    protected HUDContainer m_HudContainer;

    public virtual void OnHUDCreate(HUDContainer hudContainer)
    {
        m_HudContainer = hudContainer;
    }
    public virtual void OnHUDDestroy() { }

    public virtual void OnHUDOpen() { }
    public virtual void OnHUDClose() { }

}
