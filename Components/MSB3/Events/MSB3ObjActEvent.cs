﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsFormats;

[AddComponentMenu("Dark Souls 3/Events/Object Action")]
public class MSB3ObjActEvent : MSB3Event
{
    /// <summary>
    /// Unknown.
    /// </summary>
    public int ObjActEntityID;

    /// <summary>
    /// Unknown.
    /// </summary>
    public string PartName2;

    /// <summary>
    /// Unknown.
    /// </summary>
    public int ObjActParamID;

    /// <summary>
    /// Unknown.
    /// </summary>
    public MSB3.Event.ObjAct.ObjActState ObjActStateType;

    /// <summary>
    /// Unknown.
    /// </summary>
    public int EventFlagID;

    public void SetEvent(MSB3.Event.ObjAct evt)
    {
        setBaseEvent(evt);
        ObjActEntityID = evt.ObjActEntityID;
        PartName2 = evt.PartName2;
        ObjActParamID = evt.ObjActParamID;
        ObjActStateType = evt.ObjActStateType;
        EventFlagID = evt.EventFlagID;
    }

    public MSB3.Event.ObjAct Serialize(GameObject parent)
    {
        var evt = new MSB3.Event.ObjAct(ID, parent.name);
        _Serialize(evt, parent);
        evt.ObjActEntityID = ObjActEntityID;
        evt.PartName2 = (PartName2 == "") ? null : PartName2;
        evt.ObjActParamID = ObjActParamID;
        evt.ObjActStateType = ObjActStateType;
        evt.EventFlagID = EventFlagID;
        return evt;
    }
}
