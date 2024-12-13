using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SojaExiles;

public class ItemSpawner : MonoBehaviour
{
    public GameObject player;
    public Tracker tracker;
    public GameObject batteryPrefab;
    private int batteryCount = 6;
    public int minBatteryCount = 3;
    public GameObject pillPrefab;
    private int pillCount = 6;
    public int minPillCount = 3;

    [Header("Spawn Locations")]
    public float clumpCheckRadius = 0.6f;
    public int maxClumpAllowance = 2;

    [Header("Reset Spawn Locations")]
    public float checkRadius = 5f;

    [Header("Modes/Easy")]
    public int EMbatteryCount = 10;
    public int EMpillCount = 10;
    [Header("Modes/Normal")]
    public int NMbatteryCount = 6;
    public int NMpillCount = 8;
    [Header("Modes/Hard")]
    public int HMbatteryCount = 5;
    public int HMpillCount = 6;
    [Header("Modes/Very Hard")]
    public int VHMbatteryCount = 3;
    public int VHMpillCount = 4;

    private GameObject[] spawnPoints;

    [HideInInspector]
    public int numBatteries = 0;
    [HideInInspector]
    public int numPills = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Update according to mode
        switch (MainMenu.mode)
        {
            case 0:
                batteryCount = EMbatteryCount;
                pillCount = EMpillCount;
                break;
            case 1:
                batteryCount = NMbatteryCount;
                pillCount = NMpillCount;
                break;
            case 2:
                batteryCount = HMbatteryCount;
                pillCount = HMpillCount;
                break;
            case 3:
                batteryCount = VHMbatteryCount;
                pillCount = VHMpillCount;
                break;
            case 4:
                batteryCount = VHMbatteryCount;
                pillCount = VHMpillCount;
                break;
        }

        spawnPoints = GameObject.FindGameObjectsWithTag("Item Spawn Point");

        for (int i = 0; i < batteryCount; i++)
        {
            SpawnBattery();
        }

        for (int i = 0; i < pillCount; i++)
        {
            SpawnPill();
        }
    }

    public void SpawnBattery()
    {
        int rand;

        // Prevent Item Clumping
        bool validate;
        do
        {
            rand = Random.Range(0, spawnPoints.Length - 1);

            Collider[] colliders = Physics.OverlapSphere(spawnPoints[rand].transform.position, checkRadius);

            int numNearby = 0;
            validate = true;
 
            foreach (Collider collider in colliders)
            {
                if (Vector3.Distance(collider.transform.position, spawnPoints[rand].transform.position) < clumpCheckRadius)
                    numNearby++;
            }

            if (numNearby > maxClumpAllowance - 1)
            {
                validate = false;
            }
        } while (!validate);

        GameObject battery = Instantiate(batteryPrefab);
        battery.transform.position = spawnPoints[rand].transform.position;
        battery.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

        bool attach = spawnPoints[rand].gameObject.GetComponent<ItemSpawnPoint>().ShouldParent;
        if (attach)
        {
            battery.transform.SetParent(spawnPoints[rand].transform, false);
            battery.transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        battery.GetComponent<Battery>().tracker = player.GetComponent<Tracker>();
        battery.GetComponent<Battery>().itemSpawner = this;

        ResetSpawnLocation(spawnPoints[rand].transform.position);

        numBatteries++;
    }

    void SpawnPill()
    {
        int rand;

        // Prevent Item Clumping
        bool validate;
        do
        {
            rand = Random.Range(0, spawnPoints.Length - 1);

            Collider[] colliders = Physics.OverlapSphere(spawnPoints[rand].transform.position, checkRadius);

            int numNearby = 0;
            validate = true;

            foreach (Collider collider in colliders)
            {
                if (Vector3.Distance(collider.transform.position, spawnPoints[rand].transform.position) < clumpCheckRadius)
                    numNearby++;
            }

            if (numNearby > maxClumpAllowance - 1)
            {
                validate = false;
            }
        } while (!validate);

        GameObject pill = Instantiate(pillPrefab);
        pill.transform.position = spawnPoints[rand].transform.position;
        pill.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

        bool attach = spawnPoints[rand].gameObject.GetComponent<ItemSpawnPoint>().ShouldParent;
        if (attach)
        {
            pill.transform.SetParent(spawnPoints[rand].transform, false);
            pill.transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        pill.GetComponent<Pill>().player = player.GetComponent<PlayerMovement>();
        pill.GetComponent<Pill>().itemSpawner = this;

        ResetSpawnLocation(spawnPoints[rand].transform.position);

        numPills++;
    }

    void ResetSpawnLocation(Vector3 location)
    {
        Collider[] colliders = Physics.OverlapSphere(location, checkRadius);
        foreach(Collider collider in colliders)
        {
            GameObject gameObject = collider.gameObject;

            var drawer = gameObject.GetComponent<Drawer_Pull_X>();
            if (drawer != null)
            {
                if (drawer.open)
                {
                    StartCoroutine(drawer.closing());
                }
            }

            var door1 = gameObject.GetComponent<opencloseDoor>();
            if (door1 != null)
            {
                if (door1.open)
                {
                    StartCoroutine(door1.closing());
                }
            }

            var door2 = gameObject.GetComponent<opencloseDoor1>();
            if (door2 != null)
            {
                if (door2.open)
                {
                    StartCoroutine(door2.closing());
                }
            }
        }
    }

    void CheckItemCounts()
    {
        // Batteries
        if (numBatteries < minBatteryCount)
        {
            SpawnBattery();
        }

        if (numPills < minPillCount)
        {
            SpawnPill();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckItemCounts();
    }
}
