using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
    public GameObject target;
    public int numberOfPaths = 10;
    public float pathDuration = 3f;

    private NavMeshAgent agent;
    private List<Vector3> pathPoints = new List<Vector3>();

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GeneratePaths();
    }

    private void GeneratePaths()
    {
        for (int i = 0; i < numberOfPaths; i++)
        {
            Vector3 randomPoint = RandomPointOnNavMesh();
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(randomPoint, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                StartCoroutine(ShowPath(path));
            }
        }
    }

    private IEnumerator<WaitForSeconds> ShowPath(NavMeshPath path)
    {
        pathPoints.Clear();
        foreach (var corner in path.corners)
        {
            pathPoints.Add(corner);
        }

        float startTime = Time.time;
        while (Time.time - startTime < pathDuration)
        {
            DrawPath();
            yield return new WaitForSeconds(0.05f);
        }

        pathPoints.Clear();
    }

    private Vector3 RandomPointOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 20f; // Adjust the radius as needed
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 100f, NavMesh.AllAreas); // Increase the distance to 100f or more
        return hit.position;
    }

    private void DrawPath()
    {
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Debug.DrawLine(pathPoints[i], pathPoints[i + 1], Color.red, pathDuration);
        }
    }
}