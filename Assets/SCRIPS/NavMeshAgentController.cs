using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavMeshAgentController : MonoBehaviour
{
    private NavMeshAgent agent;

    // Referencias públicas para que puedas asignarlas desde el Inspector
    public Transform sala;
    public Transform comedor;
    public Transform cocina;
    public Transform dormitorio;
    public Transform baño;
    public Transform jardin;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        HandleKeyboardInput();
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) MoveToDestination(sala);
        if (Input.GetKeyDown(KeyCode.Alpha2)) MoveToDestination(comedor);
        if (Input.GetKeyDown(KeyCode.Alpha3)) MoveToDestination(cocina);
        if (Input.GetKeyDown(KeyCode.Alpha4)) MoveToDestination(dormitorio);
        if (Input.GetKeyDown(KeyCode.Alpha5)) MoveToDestination(baño);
        if (Input.GetKeyDown(KeyCode.Alpha6)) MoveToDestination(jardin);
    }

    private void MoveToDestination(Transform destination)
    {
        if (destination != null)
        {
            Vector3 targetPosition = destination.position;
            MoveToTargetPosition(targetPosition);
        }
    }

    public bool SamplePosition(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public void MoveToTargetPosition(Vector3 position)
    {
        if (SamplePosition(position, 2.0f, out Vector3 sampledPosition))
        {
            agent.SetDestination(sampledPosition);
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar una posición válida en el NavMesh cerca de " + position);
        }
    }

    public NavMeshPath CalculatePath(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(targetPosition, path))
        {
            return path;
        }
        return null;
    }

    public Vector3 FindClosestEdge(Vector3 position)
    {
        if (NavMesh.FindClosestEdge(position, out NavMeshHit hit, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }

    public bool RayCast(Vector3 startPosition, Vector3 endPosition)
    {
        NavMeshHit hit;
        if (NavMesh.Raycast(startPosition, endPosition, out hit, NavMesh.AllAreas))
        {
            Debug.DrawLine(startPosition, hit.position, Color.red, 2.0f);
            return true; // Hubo colisión
        }
        Debug.DrawLine(startPosition, endPosition, Color.green, 2.0f);
        return false; // No hubo colisión
    }
}