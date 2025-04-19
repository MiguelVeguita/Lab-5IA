using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMover : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float speed;
    public List<Transform> waypoints; 
    public bool loopPath = true; 

    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    public void StartPathMovement()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;
        currentWaypointIndex = 0;
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveAlongPath());
        }
    }

    public void StopPathMovement()
    {
        isMoving = false;
        StopAllCoroutines();
    }

    private IEnumerator MoveAlongPath()
    {
        while (isMoving)
        {
            Transform target = waypoints[currentWaypointIndex];
            // Mientras no se acerque lo suficiente al waypoint
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                yield return null;
            }
            // Una vez alcanzado, pasa al siguiente waypoint
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                if (loopPath)
                    currentWaypointIndex = 0;
                else
                {
                    isMoving = false;
                    break;
                }
            }
            yield return null;
        }
    }
}
