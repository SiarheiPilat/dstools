﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsFormats;

[AddComponentMenu("Dark Souls 3/Regions/Message")]
public class MSB3MessageRegion : MSB3Region
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

    public void SetRegion(MSB3.Region.Message region)
    {
        setBaseRegion(region);
        MessageID = region.MessageID;
        UnkT02 = region.UnkT02;
        Hidden = region.Hidden;
    }

    public MSB3.Region.Message Serialize(GameObject parent)
    {
        var region = new MSB3.Region.Message(ID, parent.name);
        _Serialize(region, parent);
        region.MessageID = MessageID;
        region.UnkT02 = UnkT02;
        region.Hidden = Hidden;
        return region;
    }
}
