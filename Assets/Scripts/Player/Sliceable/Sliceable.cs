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
    private bool _despawnEntireObj;

    [SerializeField] 
    [Tooltip("Despawn only top half")]
    private bool _topHalfDespawn;
    
    [SerializeField]
    [Tooltip("Despawn only bot half")] 
    private bool _botHalfDespawn;

    [SerializeField]
    [Tooltip("Duration before despawning")]
    private float _despawnAfter;

    [SerializeField]
    [Tooltip("Remove colliders when cut")]
    private bool _removeColliders;

    [Header("Movement Locking")]
    [SerializeField]
    [Tooltip("Lock movement of top half")]
    private bool _topHalfLock;

    [SerializeField]
    [Tooltip("Lock movement of bot half")]
    private bool _botHalfLock;

    [Header("Rotation Locking")]
    [SerializeField]
    [Tooltip("Lock rotation of top half")]
    private bool _topHalfRotation;

    [SerializeField]
    [Tooltip("Lock rotation of bot half")]
    private bool _botHalfRotation;

    [SerializeField]
    private string _tagName;

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
            return _despawnEntireObj;
        }
        set
        {
            _despawnEntireObj = value;
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

    public bool TopHalfDespawn
    {
        get
        {
            return _topHalfDespawn;
        }
        set
        {
            _topHalfDespawn = value;
        }
    }

    public bool BotHalfDespawn
    {
        get
        {
            return _botHalfDespawn;
        }
        set
        {
            _botHalfDespawn = value;
        }
    }

    public bool TopHalfLock
    {
        get
        {
            return _topHalfLock;
        }
        set
        {
            _topHalfLock = value;
        }
    }

    public bool BotHalfLock
    {
        get
        {
            return _botHalfLock;
        }
        set
        {
            _botHalfLock = value;
        }
    }

    public bool TopHalfRotation
    {
        get
        {
            return _topHalfRotation;
        }
        set
        {
            _topHalfRotation = value;
        }
    }

    public bool BotHalfRotation
    {
        get
        {
            return _botHalfRotation;
        }
        set
        {
            _botHalfRotation = value;
        }
    }

    public string TagName
    {
        get
        {
            return _tagName;
        }
        set
        {
            _tagName = value;
        }
    }


    public void DespawnSelf()
    {
        StartCoroutine(DespawnOverTime());
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
