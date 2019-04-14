﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsFormats;

[AddComponentMenu("Bloodborne/Events/Invasion")]
public class MSBBBInvasionEvent : MSBBBEvent
{
    /// <summary>
    /// Unknown.
    /// </summary>
    public int HostEventEntityID;

    /// <summary>
    /// Unknown.
    /// </summary>
    public int InvasionEventEntityID;

    /// <summary>
    /// Unknown.
    /// </summary>
    public int InvasionRegionIndex;

    /// <summary>
    /// Unknown.
    /// </summary>
    public int SoundIDMaybe;

    /// <summary>
    /// Unknown.
    /// </summary>
    public int MapEventIDMaybe;

    /// <summary>
    /// Unknown.
    /// </summary>
    public int FlagsMaybe;

    /// <summary>
    /// Unknown.
    /// </summary>
    public int UnkT18;

    public void SetEvent(MSBBB.Event.Invasion evt)
    {
        setBaseEvent(evt);
        HostEventEntityID = evt.HostEventEntityID;
        InvasionEventEntityID = evt.InvasionEventEntityID;
        InvasionRegionIndex = evt.InvasionRegionIndex;
        SoundIDMaybe = evt.SoundIDMaybe;
        MapEventIDMaybe = evt.MapEventIDMaybe;
        FlagsMaybe = evt.FlagsMaybe;
        UnkT18 = evt.UnkT18;
    }

    public MSBBB.Event.Invasion Serialize(GameObject parent)
    {
        var evt = new MSBBB.Event.Invasion(ID, parent.name);
        _Serialize(evt, parent);
        evt.HostEventEntityID = HostEventEntityID;
        evt.InvasionEventEntityID = InvasionEventEntityID;
        evt.InvasionRegionIndex = InvasionRegionIndex;
        evt.SoundIDMaybe = SoundIDMaybe;
        evt.MapEventIDMaybe = MapEventIDMaybe;
        evt.FlagsMaybe = FlagsMaybe;
        evt.UnkT18 = UnkT18;
        return evt;
    }
}
