using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Video used as reference: https://www.youtube.com/watch?v=hH0OYz7YtKk

public class Lava : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float checkDistance = 0.05f;

    [SerializeField] private Transform targetWaypoint;
    [SerializeField] private int currentWaypointIndex = 0;

    public float distanceFromLava;

    public GameObject playerReference;

    void Start()
    {
        targetWaypoint = waypoints[0];
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, defaultSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < checkDistance )
        {
            targetWaypoint = GetNextWaypoint();
        }

        //how far the player is from the lava
        //Debug.Log("distance from death " + (playerReference.transform.position.y - transform.position.y - 0.75f).ToString("F2"));
        distanceFromLava = Mathf.Round((playerReference.transform.position.y - transform.position.y - 0.75f) * 100f) / 100f;

        //Debug.Log("lava pos y" + transform.position.y);
        //Debug.Log(playerReference.transform.position.y);

        if (transform.position.y > playerReference.transform.position.y - 0.75f)
        {
            //Debug.Log("dead, dead as hell");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private Transform GetNextWaypoint() 
    {
        //Debug.Log(waypoints.Length);
        if (currentWaypointIndex >= waypoints.Length)
        {
            //Debug.Log("lava is at max height");
            return waypoints[waypoints.Length - 1];
        }
        else if(currentWaypointIndex < waypoints.Length - 1)
        {
            currentWaypointIndex++;
        }
        //Debug.Log(currentWaypointIndex);
        return waypoints[currentWaypointIndex];
    }
}