using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatSpawner : MonoBehaviour
{
    public int numRatsToSpawn = 5;

    //public  float minSpawnDistance = 20f;

    public GameObject ratPrefab;
    public PlayerMovement player;
    public Tracker tracker;
    public Snake snake;

    // Start is called before the first frame update
    void Start()
    {
        SpawnRats();
    }

    void SpawnRats()
    {
        for (int i = 0; i < numRatsToSpawn; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 200f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 200f, 1);

            RatController rat = Instantiate(ratPrefab, hit.position, Quaternion.identity).GetComponent<RatController>();
            rat.player = player;
            rat.tracker = tracker;
            rat.snake = snake;
        }
    }
}
