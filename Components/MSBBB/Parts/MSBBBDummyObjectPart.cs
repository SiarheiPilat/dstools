﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsFormats;

[AddComponentMenu("Bloodborne/Parts/Dummy Object")]
public class MSBBBDummyObjectPart : MSBBBObjectPart
{
    public new MSBBB.Part.DummyObject Serialize(GameObject parent)
    {
        var part = new MSBBB.Part.DummyObject(ID, parent.name);
        _Serialize(part, parent);
        part.CollisionName = (CollisionName == "") ? null : CollisionName;
        part.UnkT04 = UnkT04;
        part.UnkT06 = UnkT06;
        part.UnkT07 = UnkT07;
        part.UnkT08 = UnkT08;
        part.UnkT09 = UnkT09;
        part.UnkT10 = UnkT10;
        part.UnkT02a = UnkT02a;
        part.UnkT02b = UnkT02b;
        part.UnkT03a = UnkT03a;
        part.UnkT03b = UnkT03b;
        part.UnkT05a = UnkT05a;
        part.UnkT05b = UnkT05b;
        return part;
    }
}
