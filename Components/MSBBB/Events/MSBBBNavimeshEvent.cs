﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsFormats;

[AddComponentMenu("Bloodborne/Events/Navimesh")]
public class MSBBBNavimeshEvent : MSBBBEvent
{
    /// <summary>
    /// Region for navimesh
    /// </summary>
    public string RegionName;

    public void SetEvent(MSBBB.Event.Navimesh evt)
    {
        setBaseEvent(evt);
        RegionName = evt.RegionName;
    }

    public MSBBB.Event.Navimesh Serialize(GameObject parent)
    {
        var evt = new MSBBB.Event.Navimesh(ID, parent.name);
        _Serialize(evt, parent);
        evt.RegionName = (RegionName == "") ? null : RegionName;
        return evt;
    }
}
