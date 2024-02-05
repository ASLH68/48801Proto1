using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : OnSliced
{
    [SerializeField] GameObject _object;
    [SerializeField] float impulse = 1;
    public override void SlicedAction()
    {
        // Calls the parent sliced action
        base.SlicedAction();

        // object specific functionality
        Debug.Log("derived sliced");

        _object.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionY;
    }
}
