using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : OnSliced
{
    [SerializeField] GameObject _object;
    [SerializeField] float _xImpulse = 0f;
    [SerializeField] float _yImpulse = 0f;
    [SerializeField] float _zImpulse = 0f;
    [SerializeField] float _timeRemaining = 0f;
    public override void SlicedAction()
    {
        // Calls the parent sliced action
        base.SlicedAction();

        // object specific functionality
        Debug.Log("derived sliced");

        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
        }
        else
        {
            _timeRemaining = 0f;
            _object.GetComponent<Rigidbody>().AddForce(new Vector3(_xImpulse, _yImpulse, _zImpulse), ForceMode.Impulse);
        }
    }

    /*private void Update()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
        }
        else
        {
            _timeRemaining = 0f;
            _object.GetComponent<Rigidbody>().AddForce(new Vector3(_xImpulse, _yImpulse, _zImpulse), ForceMode.Impulse);
        }
    }*/
}
