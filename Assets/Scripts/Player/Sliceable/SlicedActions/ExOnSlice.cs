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
        Debug.Log("derived sliced");
    }
}
