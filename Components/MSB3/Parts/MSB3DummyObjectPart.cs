﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsFormats;

[AddComponentMenu("Dark Souls 3/Parts/Dummy Object")]
public class MSB3DummyObjectPart : MSB3ObjectPart
{
    public new MSB3.Part.DummyObject Serialize(GameObject parent)
    {
        var part = new MSB3.Part.DummyObject(ID, parent.name);
        _Serialize(part, parent);
        part.CollisionName = (CollisionName == "") ? null : CollisionName;
        part.UnkT0C = UnkT0C;
        part.UnkT0E = UnkT0E;
        part.UnkT10 = StartAnimID;
        part.UnkT12 = UnkT12;
        part.UnkT14 = UnkT14;
        part.UnkT16 = UnkT16;
        part.UnkT18 = UnkT18;
        part.UnkT1A = UnkT1A;
        part.UnkT1C = UnkT1C;
        part.UnkT1E = UnkT1E;
        part.UnkT20 = UnkT20;
        part.UnkT24 = UnkT24;
        part.UnkT28 = UnkT28;
        part.UnkT2C = UnkT2C;
        return part;
    }
}
