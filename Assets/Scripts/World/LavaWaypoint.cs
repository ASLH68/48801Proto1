using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWaypoint : MonoBehaviour
{
    public int waypointId; //key (this is really important, it has to be unique from other waypoints and in numeric order with no gaps)

    [SerializeField] public int lavaSpeedChange; //changes the lava speed by this amount when this waypoint is used

    void Awake()
    {
        AddNewWaypoint();
    }

    //add this waypoint to the dictionary
    public void AddNewWaypoint()
    {
        //call function in Lava that adds this to dictionary
        //Debug.Log(transform.parent);
        Lava lava = GameObject.Find("Beans").GetComponent<Lava>();
        lava.AddNewWaypoint(waypointId, this);
    }
}
