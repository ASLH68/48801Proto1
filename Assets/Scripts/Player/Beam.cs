using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
//using UnityEditor.PackageManager;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private static Beam _instance;
    //The number of vertices to create per frame
    private const int NUM_VERTICES = 12;

    [SerializeField]
    [Tooltip("The blade object")]
    private GameObject _blade = null;

    [SerializeField]
    [Tooltip("The empty game object located at the tip of the blade")]
    private GameObject _tip = null;

    [SerializeField]
    [Tooltip("The empty game object located at the base of the blade")]
    private GameObject _base = null;

    [SerializeField]
    [Tooltip("The mesh object with the mesh filter and mesh renderer")]
    private GameObject _meshParent = null;

    [SerializeField]
    [Tooltip("The number of frame that the trail should be rendered for")]
    private int _trailFrameLength = 3;

    [SerializeField]
    [ColorUsage(true, true)]
    [Tooltip("The colour of the blade and trail")]
    private Color _colour = Color.red;

    [SerializeField] PhysicMaterial _slipperyMat;
    /*    [SerializeField]
        [Tooltip("The amount of force applied to each side of a slice")]
        private float _forceAppliedToCut = 3f;*/

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private int _frameCount;
    private Vector3 _previousTipPosition;
    private Vector3 _previousBasePosition;
    private Vector3 _triggerEnterTipPosition;
    private Vector3 _triggerEnterBasePosition;
    private Vector3 _triggerExitTipPosition;

    private bool _ableToSliceObj;

    [SerializeField]
    [Tooltip("Force applied when cut")]
    private float _defaultForce;

    #region Getters/Setters
    public float DefaultForce
    {
        get
        {
            return _defaultForce;
        }
        set
        {
            _defaultForce = value;
        }
    }
    #endregion

    public static Beam Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Instance of Beam is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //Init mesh and triangles
        //_meshParent.transform.position = Vector3.zero;
        /*_mesh = new Mesh();
        _meshParent.GetComponent<MeshFilter>().mesh = _mesh;*/

        /*Material trailMaterial = Instantiate(_meshParent.GetComponent<MeshRenderer>().sharedMaterial);
        trailMaterial.SetColor("Color_8F0C0815", _colour);
        _meshParent.GetComponent<MeshRenderer>().sharedMaterial = trailMaterial;

        Material bladeMaterial = Instantiate(_blade.GetComponent<MeshRenderer>().sharedMaterial);
        bladeMaterial.SetColor("Color_AF2E1BB", _colour);
        _blade.GetComponent<MeshRenderer>().sharedMaterial = bladeMaterial;*/

        _vertices = new Vector3[_trailFrameLength * NUM_VERTICES];
        _triangles = new int[_vertices.Length];

        //Set starting position for tip and base
        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
    }

    void LateUpdate()
    {
        //Reset the frame count one we reach the frame length
        if (_frameCount == (_trailFrameLength * NUM_VERTICES))
        {
            _frameCount = 0;
        }

        //Draw first triangle vertices for back and front
        /*        _vertices[_frameCount] = _base.transform.position;
                _vertices[_frameCount + 1] = _tip.transform.position;
                _vertices[_frameCount + 2] = _previousTipPosition;
                _vertices[_frameCount + 3] = _base.transform.position;
                _vertices[_frameCount + 4] = _previousTipPosition;
                _vertices[_frameCount + 5] = _tip.transform.position;

                //Draw fill in triangle vertices
                _vertices[_frameCount + 6] = _previousTipPosition;
                _vertices[_frameCount + 7] = _base.transform.position;
                _vertices[_frameCount + 8] = _previousBasePosition;
                _vertices[_frameCount + 9] = _previousTipPosition;
                _vertices[_frameCount + 10] = _previousBasePosition;
                _vertices[_frameCount + 11] = _base.transform.position;

                //Set triangles
                _triangles[_frameCount] = _frameCount;
                _triangles[_frameCount + 1] = _frameCount + 1;
                _triangles[_frameCount + 2] = _frameCount + 2;
                _triangles[_frameCount + 3] = _frameCount + 3;
                _triangles[_frameCount + 4] = _frameCount + 4;
                _triangles[_frameCount + 5] = _frameCount + 5;
                _triangles[_frameCount + 6] = _frameCount + 6;
                _triangles[_frameCount + 7] = _frameCount + 7;
                _triangles[_frameCount + 8] = _frameCount + 8;
                _triangles[_frameCount + 9] = _frameCount + 9;
                _triangles[_frameCount + 10] = _frameCount + 10;
                _triangles[_frameCount + 11] = _frameCount + 11;*/

        /* _meshParent.GetComponent<MeshFilter>().mesh.vertices = _vertices;
         _meshParent.GetComponent<MeshFilter>().mesh.triangles = _triangles;*/

        //Track the previous base and tip positions for the next frame
        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
        _frameCount += NUM_VERTICES;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter");
        _triggerEnterTipPosition = _tip.transform.position;
        _triggerEnterBasePosition = _base.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        _triggerExitTipPosition = _tip.transform.position;

        //Create a triangle between the tip and base so that we can get the
        //normal
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;

        //Get the point perpendicular to the triangle above which is the normal
        //https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        //Transform the normal so that it is aligned with the object we are
        //slicing's transform.
        Vector3 transformedNormal =
            ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose
            * normal)).normalized;

        //Get the enter position relative to the object we're cutting's local
        //transform
        Vector3 transformedStartingPoint =
            other.gameObject.transform.InverseTransformPoint(_triggerEnterTipPosition);

        Plane plane = new Plane();

        plane.SetNormalAndPosition(
                transformedNormal,
                transformedStartingPoint);

        var direction = Vector3.Dot(Vector3.up, transformedNormal);

        //Flip the plane so that we always know which side the positive mesh is on
        if (direction < 0)
        {
            plane = plane.flipped;
        }

        float force;
        if (other.TryGetComponent<Sliceable>(out Sliceable T))
        {
            force = T.ForceApplied;
        }
        else
        {
            force = DefaultForce;
        }

        if (_ableToSliceObj)
        {
            GameObject[] slices = Slicer.Slice(plane, other.gameObject);
            foreach (GameObject slice in slices)
            {
                Sliceable sliceable = slice.GetComponent<Sliceable>();
                // sliced halves can be sliced again
                sliceable.UseGravity = T.UseGravity;
                sliceable.SmoothVertices = T.SmoothVertices;
                sliceable.ShareVertices = T.ShareVertices;
                sliceable.Despawn = T.Despawn;
                sliceable.RemoveColliders = T.RemoveColliders;
                sliceable.TopHalfLock = T.TopHalfLock;
                sliceable.BotHalfLock = T.BotHalfLock;
                sliceable.BotHalfDespawn = T.BotHalfDespawn;
                sliceable.TopHalfDespawn = T.TopHalfDespawn;
                sliceable.TopHalfRotation = T.TopHalfRotation;
                sliceable.BotHalfRotation = T.BotHalfRotation;

                // Adds slippery mat
                //slice.GetComponent<MeshCollider>().material = _slipperyMat;

                if (sliceable.RemoveColliders)
                {
                    sliceable.DisableColliders();
                }

                sliceable.gameObject.layer = LayerMask.NameToLayer("Ground");
                //slices[0].layer = LayerMask.NameToLayer("Ground");
                //slices[1].layer = LayerMask.NameToLayer("Ground");

                Sliceable otherSliceable = other.GetComponent<Sliceable>();
                if (otherSliceable.TopHalfDespawn && otherSliceable.GetComponent<Sliceable>().BotHalfDespawn || otherSliceable.GetComponent<Sliceable>().Despawn)
                {
                    Debug.Log("test");
                    sliceable.DespawnAfter = T.DespawnAfter;
                    sliceable.DespawnSelf();
                }
                else if (otherSliceable.TopHalfDespawn)
                {
                    Debug.Log(slices[0].name);
                    slices[0].GetComponent<Sliceable>().DespawnAfter = T.DespawnAfter;
                    slices[0].GetComponent<Sliceable>().DespawnSelf();
                }
                else if (otherSliceable.BotHalfDespawn)
                {
                    Debug.Log(slices[1].name);
                    slices[1].GetComponent<Sliceable>().DespawnAfter = T.DespawnAfter;
                    slices[1].GetComponent<Sliceable>().DespawnSelf();
                }

                if (otherSliceable.TopHalfLock)
                {
                    slices[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
                if (otherSliceable.BotHalfLock)
                {
                    slices[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }

                if (otherSliceable.TopHalfRotation)
                {
                    slices[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }
                if (otherSliceable.BotHalfRotation)
                {
                    slices[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }
            }

            // Calls any actions that happen when an object is sliced
            if (other.TryGetComponent<OnSliced>(out OnSliced Sliced))
            {
                Sliced.SlicedAction();
            }

            //other.GetComponent<Sliceable>().UseGravity = false;

            Destroy(other.gameObject);

            Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
            Vector3 newNormal = transformedNormal + Vector3.up *
                force;

            rigidbody.AddForce(newNormal, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 4.75f;

        if (Physics.Raycast(GameObject.Find("Base").transform.position, forward, out RaycastHit hit, 4.75f))
        {
            //Debug.Log(hit.transform.gameObject);
            GameObject tip = GameObject.Find("Tip");
            Transform ogParent = tip.transform.parent;
            tip.transform.parent = null;
            tip.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            tip.transform.parent = ogParent;
            
            if(hit.transform.GetComponent<Sliceable>() != null)
            {
                _ableToSliceObj = true;
            }
            //return (hit.transform.TryGetComponent<Sliceable>(out Sliceable P));
        }
        //return false;
    }
}
