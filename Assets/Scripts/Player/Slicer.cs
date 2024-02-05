/*****************************************************************************
// File Name :         Slicer.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     1/22/24
//
// Brief Description : Slices a mesh
// Reference: https://www.youtube.com/watch?v=BVCNDUcnE1o
*****************************************************************************/

using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Slicer : MonoBehaviour
{
    public static Slicer Instance;
    public GameObject PositiveObj { get; private set; }
    public GameObject NegativeObj { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    /// <summary>
    /// Slice the object by the plane 
    /// </summary>
    /// <param name="plane"></param>
    /// <param name="objectToCut"></param>
    /// <returns> Array of cut objects </returns>
    public static GameObject[] Slice(Plane plane, GameObject objectToCut)
    {
        //Get the current mesh and its verts and tris
        Mesh mesh = objectToCut.GetComponent<MeshFilter>().mesh;
        var a = mesh.GetSubMesh(0);
        Sliceable sliceable = objectToCut.GetComponent<Sliceable>();

        if (sliceable == null)
        {
            throw new NotSupportedException("Cannot slice non sliceable " +
                "object, add the sliceable script to the object or inherit " +
                "from sliceable to support slicing");
        }

        //Create left and right slice of hollow object
        SlicesMetadata slicesMeta = 
            new SlicesMetadata(plane, mesh, sliceable.IsSolid, 
            sliceable.ReverseWireTriangles, sliceable.ShareVertices, 
            sliceable.SmoothVertices);

        GameObject PositiveObj = CreateMeshGameObject(objectToCut);
        PositiveObj.name = string.Format("{0}_positive", objectToCut.name);

        //PositiveObj.transform.parent = objectToCut.transform.parent;
       
        GameObject NegativeObj = CreateMeshGameObject(objectToCut);
        NegativeObj.name = string.Format("{0}_negative", objectToCut.name);

        //NegativeObj.transform.parent = objectToCut.transform.parent;

        var positiveSideMeshData = slicesMeta.PositiveSideMesh;
        var negativeSideMeshData = slicesMeta.NegativeSideMesh;

        PositiveObj.GetComponent<MeshFilter>().mesh = positiveSideMeshData;
        NegativeObj.GetComponent<MeshFilter>().mesh = negativeSideMeshData;

        SetupCollidersAndRigidBodys(ref PositiveObj, positiveSideMeshData, 
            sliceable.UseGravity);
        SetupCollidersAndRigidBodys(ref NegativeObj, negativeSideMeshData, 
            sliceable.UseGravity);

        return new GameObject[] { PositiveObj, NegativeObj };
    }

    /// <summary>
    /// Creates the default mesh game object.
    /// </summary>
    /// <param name="originalObject">The original object.</param>
    /// <returns></returns>
    private static GameObject CreateMeshGameObject(GameObject originalObject)
    {
        var originalMaterial =
            originalObject.GetComponent<MeshRenderer>().materials;

        GameObject meshGameObject = new GameObject();
        meshGameObject.transform.parent = originalObject.transform.parent;

        Sliceable originalSliceable = originalObject.GetComponent<Sliceable>();

        meshGameObject.AddComponent<MeshFilter>();
        meshGameObject.AddComponent<MeshRenderer>();
        Sliceable sliceable = meshGameObject.AddComponent<Sliceable>();

        sliceable.IsSolid = originalSliceable.IsSolid;
        sliceable.ReverseWireTriangles =
            originalSliceable.ReverseWireTriangles;
        sliceable.UseGravity = originalSliceable.UseGravity;

        meshGameObject.GetComponent<MeshRenderer>().materials =
            originalMaterial;

        meshGameObject.transform.localScale =
            originalObject.transform.localScale;
        meshGameObject.transform.rotation = originalObject.transform.rotation;
        meshGameObject.transform.position = originalObject.transform.position;

        meshGameObject.tag = originalObject.tag;
        
        return meshGameObject;
    }

    /// <summary>
    /// Add mesh collider and rigid body to game object
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="mesh"></param>
    private static void SetupCollidersAndRigidBodys(ref GameObject gameObject, 
        Mesh mesh, bool useGravity)
    {
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;

        var rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = useGravity;
    }
}
