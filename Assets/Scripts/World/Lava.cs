using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    public Dictionary<int, LavaWaypoint> waypointDict = new Dictionary<int, LavaWaypoint>();

    [SerializeField] private int currentWaypoint = 1;
    [SerializeField] private float defaultSpeed;

    public float lavaHeight = 0;
    public float playerDistanceFromLava;

    public GameObject Player;

    void Update()
    {
        //Debug.Log("Player pos" + Mathf.Round(Player.transform.position.y * 100f) / 100f);
        lavaHeight = Mathf.Round(transform.position.y * 100f) / 100f;
        //Debug.Log("Lava pos" + Mathf.Round(Player.transform.position.y * 100f) / 100f);
        playerDistanceFromLava = Mathf.Round((Player.transform.position.y - lavaHeight - 0.85f) * 100f) / 100f;

        if (transform.position.y >= waypointDict[currentWaypoint].transform.position.y)
        {
            if (currentWaypoint < waypointDict.Count)
            {
                currentWaypoint++;
            }
            else
            {
                //Debug.Log("max lava height reached");
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(new Vector3(0, transform.position.y, 0), new Vector3(0, waypointDict[currentWaypoint].transform.position.y, 0), defaultSpeed * Time.deltaTime);
        }
    }

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    public void AddNewWaypoint(int waypointId, LavaWaypoint thisWaypoint)
    {
        //called by each waypoint to add itself to this dictionary
        waypointDict.Add(waypointId, thisWaypoint);
    }
}