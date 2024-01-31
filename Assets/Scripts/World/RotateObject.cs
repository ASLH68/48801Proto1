/*
 * This class causes the attached object to rotate on the Y axis at over a duration of
 * [_rotationLength] seconds.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
 
    // Vars
    [SerializeField] int _rotationLength;
    [SerializeField] bool _isCounterClockwise;
    float _rotationSpeed;

    private void Start()
    {
        // Sets rotation speed and direction
        _rotationSpeed = (360 / _rotationLength);
        if (_isCounterClockwise)
            _rotationSpeed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotates object
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
}
