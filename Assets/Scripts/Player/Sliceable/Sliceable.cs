using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Will the new shapes be hollow")]
    private bool _isSolid = true;

    [SerializeField]
    private bool _reverseWindTriangles = false;

    [SerializeField]
    [Tooltip("Do the new shapes have gravity?")] 
    private bool _useGravity = true;

    [SerializeField]
    private bool _shareVertices = false;

    [SerializeField]
    private bool _smoothVertices = false;

    [SerializeField]
    [Tooltip("Force applied when cut")]
    public float _forceApplied;

    [SerializeField]
    [Tooltip("Despawn when cut")]
    private bool _despawn;

    [SerializeField]
    [Tooltip("Duration before despawning")]
    private float _despawnAfter;

    [SerializeField]
    [Tooltip("Remove colliders when cut")]
    private bool _removeColliders;


    public bool IsSolid
    {
        get
        {
            return _isSolid;
        }
        set
        {
            _isSolid = value;
        }
    }

    public bool ReverseWireTriangles
    {
        get
        {
            return _reverseWindTriangles;
        }
        set
        {
            _reverseWindTriangles = value;
        }
    }

    public bool UseGravity
    {
        get
        {
            return _useGravity;
        }
        set
        {
            _useGravity = value;
        }
    }

    public bool ShareVertices
    {
        get
        {
            return _shareVertices;
        }
        set
        {
            _shareVertices = value;
        }
    }

    public bool SmoothVertices
    {
        get
        {
            return _smoothVertices;
        }
        set
        {
            _smoothVertices = value;
        }
    }

    public float ForceApplied
    {
        get
        {
            if(Beam.Instance.DefaultForce != _forceApplied)
            {
                return _forceApplied;
            }
            else
            {
                return Beam.Instance.DefaultForce;
            }
        }
        set
        {
            _forceApplied = value;
        }
    }

    public bool Despawn
    {
        get
        {
            return _despawn;
        }
        set
        {
            _despawn = value;
        }
    }

    public bool RemoveColliders
    {
        get
        {
            return _removeColliders;
        }
        set
        {
            _removeColliders = value;
        }
    }

    public float DespawnAfter
    { 
        get
        {
            return _despawnAfter;
        }
        set
        {
            _despawnAfter = value;
        }
    }


    public void DespawnSelf()
    {
        if(_despawn)
        {
            StartCoroutine(DespawnOverTime());
        }
    }

    private IEnumerator DespawnOverTime()
    {
        yield return new WaitForSeconds(_despawnAfter);
        Destroy(gameObject);
    }

    public void DisableColliders()
    {
        if (_removeColliders)
        {
            Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), GetComponent<Collider>());
            
        }
    }
}
