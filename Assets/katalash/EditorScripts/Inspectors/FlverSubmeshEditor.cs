﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlverSubmesh))]
// Note that this doesn't edit multiple objects at a time because it's used for unwrapping UVs, which crashes
// Unity if you try and unwrap more than one submesh at a time :trashcat:
class FlverSubmeshEditor : Editor
{
    void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        FlverSubmesh submesh = (FlverSubmesh)target;
        // Long check to see if generating lightmap UVs is safe
        if (submesh.Link != null)
        {
            if (submesh.Link.Submeshes.Count > submesh.SubmeshIdx)
            {
                if (submesh.Link.Submeshes[submesh.SubmeshIdx].Mtd != null)
                {
                    if (submesh.Link.Submeshes[submesh.SubmeshIdx].Mtd.LightmapUVIndex != -1)
                    {
                        if (GUILayout.Button("Generate Lightmap UVs"))
                        {
                            submesh.Link.GenerateLightmapUVSForSubmesh(submesh.SubmeshIdx);
                        }
                    }
                }
            }
        }

        if (GUILayout.Button("Dump Normals"))
        {
            var dump = CreateInstance<NormalDump>();
            var mesh = ((FlverSubmesh)target).gameObject.GetComponent<MeshFilter>().sharedMesh;
            dump.Normals = mesh.normals;
            AssetDatabase.CreateAsset(dump, "Assets/normaldump.asset");
        }

        if (submesh.Link != null)
        {
            if (submesh.Link.Submeshes.Count > submesh.SubmeshIdx)
            {
                if (submesh.Link.Submeshes[submesh.SubmeshIdx].Mtd != null)
                {
                    var sub = submesh.Link.Submeshes[submesh.SubmeshIdx];
                    GUILayout.Label($@"Material Name: {sub.Name}");
                    GUILayout.Label($@"MTD: {sub.Mtd}");
                    GUILayout.Label($@"Lightmap Index: {sub.Mtd.LightmapUVIndex}");
                }
            }
        }
    }
}