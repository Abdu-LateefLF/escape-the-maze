using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    public GameObject exitDoor;
    private GameObject[] possibleSpawns;

    // Start is called before the first frame update
    void Awake()
    {
        possibleSpawns = GameObject.FindGameObjectsWithTag("Outer Wall");

        SpawnDoor();
    }

    void SpawnDoor()
    {
        int rand = Random.Range(0, possibleSpawns.Length - 1);

        possibleSpawns[rand].SetActive(false);

        exitDoor.transform.position = new Vector3(possibleSpawns[rand].transform.position.x, 0f, possibleSpawns[rand].transform.position.z);
        exitDoor.transform.rotation = possibleSpawns[rand].transform.rotation;

        if (possibleSpawns[rand].name == "Left Wall" || possibleSpawns[rand].name == "Front Wall")
        {
            exitDoor.transform.rotation = Quaternion.Euler(exitDoor.transform.rotation.eulerAngles.x, exitDoor.transform.rotation.eulerAngles.y + 180, exitDoor.transform.rotation.eulerAngles.z);
        }
    }
}
