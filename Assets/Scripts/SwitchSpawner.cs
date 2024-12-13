using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSpawner : MonoBehaviour
{
    public GameObject switchPrefab;
    public GameObject exitDoor;
    public ObjectiveSystem objectiveSystem;
    private int numToSpawn = 4;
    public float spawnHieght = 1.63f;
    public float minDistanceBetweenSpawns = 50f;
    public float minDistanceBetweenSpawnsVeryHardMode = 27f;

    [Header("Modes")]
    public int EMnumToSpawn = 4;
    public int NMnumToSpawn = 4;
    public int HMnumToSpawn = 5;
    public int VHnumToSpawn = 7;
    public int XMnumToSpawn = 8;

    private float minDistance;

    private GameObject[] possibleLocations;
    private List<Vector3> spawnedItemPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        possibleLocations = GameObject.FindGameObjectsWithTag("Switch Spawn Point");

        // Update based on mode
        switch(MainMenu.mode)
        {
            case 0:
                numToSpawn = EMnumToSpawn;
                minDistance = minDistanceBetweenSpawns;
                break;
            case 1:
                numToSpawn = NMnumToSpawn;
                minDistance = minDistanceBetweenSpawns;
                break;
            case 2:
                numToSpawn = HMnumToSpawn;
                minDistance = minDistanceBetweenSpawns;
                break;
            case 3:
                numToSpawn = VHnumToSpawn;
                minDistance = minDistanceBetweenSpawnsVeryHardMode;
                break;
            case 4:
                numToSpawn = XMnumToSpawn;
                minDistance = minDistanceBetweenSpawnsVeryHardMode;
                break;
        }

        // Update exit door required switches
        exitDoor.GetComponent<ExitDoor>().requiredAmountOfSwitches = numToSpawn;

        objectiveSystem.UpdateObjective();

        for (int i = 0; i < numToSpawn; i++)
        {
            Vector3 location = new Vector3();
            Quaternion rotation = new Quaternion();

            bool isValidLocation = false;

            while (isValidLocation == false)
            {
                isValidLocation = true;

                int rand = Random.Range(0, possibleLocations.Length - 1);
                location = possibleLocations[rand].transform.position;
                rotation = possibleLocations[rand].transform.rotation;

                for (int x = 0; x < spawnedItemPositions.Count; x++)
                {
                    if (Vector3.Distance(location, spawnedItemPositions[x]) < minDistance)
                    {
                        isValidLocation = false;
                        break;
                    }
                }
            }

            spawnedItemPositions.Add(location);
            GameObject lever = Instantiate(switchPrefab, location + new Vector3(0f, spawnHieght, 0f), rotation);
            lever.GetComponent<Switch>().exitDoor = exitDoor;
            lever.GetComponent<Switch>().switchSpawner = this;
        }
    }
}