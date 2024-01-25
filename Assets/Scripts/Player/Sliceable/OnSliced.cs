using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSliced : MonoBehaviour
{
    [SerializeField] string _name;
    /// <summary>
    /// What happens when the object is sliced. This must be overridden
    /// </summary>
    public virtual void SlicedAction()
    {
        Debug.Log(_name + " has been sliced");
    }
}
