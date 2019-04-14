﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsFormats;

[AddComponentMenu("Bloodborne/Events/SFX")]
public class MSBBBSFXEvent : MSBBBEvent
{
    /// <summary>
    /// The ID of the .fxr file to play in this region.
    /// </summary>
    public int FFXID;

    /// <summary>
    /// If true, the effect is off by default until enabled by event scripts.
    /// </summary>
    public bool StartDisabled;

    public void SetEvent(MSBBB.Event.SFX evt)
    {
        setBaseEvent(evt);
        FFXID = evt.FFXID;
        StartDisabled = evt.StartDisabled;
    }

    public MSBBB.Event.SFX Serialize(GameObject parent)
    {
        var evt = new MSBBB.Event.SFX(ID, parent.name);
        _Serialize(evt, parent);
        evt.FFXID = FFXID;
        evt.StartDisabled = StartDisabled;
        return evt;
    }
}
