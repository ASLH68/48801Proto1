using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : OnSliced
{
    [SerializeField] GameObject _object;
    [SerializeField] float _xImpulse = 0f;
    [SerializeField] float _yImpulse = 0f;
    [SerializeField] float _zImpulse = 0f;

    public override void SlicedAction()
    {
        // Calls the parent sliced action
        base.SlicedAction();

        // object specific functionality
        Debug.Log("derived sliced");

        _object.GetComponent<Rigidbody>().AddForce(new Vector3(_xImpulse, _yImpulse, _zImpulse), ForceMode.Impulse);
    }
}
