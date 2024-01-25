/*****************************************************************************
// File Name :         OnSliced.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     1/25/24
//
// Brief Description : Base class that can be derived to cause some action to
                       happen when an object is sliced
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSliced : MonoBehaviour
{
    [SerializeField] string _name;
    /// <summary>
    /// What happens when the object is sliced. This should be overridden
    /// </summary>
    public virtual void SlicedAction()
    {
        Debug.Log(_name + " has been sliced");
    }
}
