﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsFormats;

[AddComponentMenu("Bloodborne/Events/Message")]
public class MSBBBMessageEvent : MSBBBEvent
{
    /// <summary>
    /// ID of the message's text in the FMGs.
    /// </summary>
    public short MessageID;

    /// <summary>
    /// Unknown. Always 0 or 2.
    /// </summary>
    public short UnkT02;

    /// <summary>
    /// Whether the message requires Seek Guidance to appear.
    /// </summary>
    public bool Hidden;

    public void SetEvent(MSBBB.Event.Message evt)
    {
        setBaseEvent(evt);
        MessageID = evt.MessageID;
        UnkT02 = evt.UnkT02;
        Hidden = evt.Hidden;
    }

    public MSBBB.Event.Message Serialize(GameObject parent)
    {
        var evt = new MSBBB.Event.Message(ID, parent.name);
        _Serialize(evt, parent);
        evt.MessageID = MessageID;
        evt.UnkT02 = UnkT02;
        evt.Hidden = Hidden;
        return evt;
    }
}
