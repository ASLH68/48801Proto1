/*****************************************************************************
// File Name :         ExOnSlice.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     01/25/24
//
// Brief Description : Example for how OnSliced is used
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExOnSlice : OnSliced
{
    public override void SlicedAction()
    {
        // Calls the parent sliced action
        base.SlicedAction();

        // object specific functionality
        Debug.Log("derived sliced");
    }
}
