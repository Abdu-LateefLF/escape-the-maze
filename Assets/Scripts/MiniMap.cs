using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Tracker tracker;
    public RectTransform playerMinimapAnchor;
    public RectTransform snakeMinimapAnchor;
    public RectTransform batteryAnchor;
    public RectTransform minimapPoint1;
    public RectTransform minimapPoint2;
    public GameObject screen;
    public GameObject switchAnchorPrefab;

    public Transform PlayerPos;
    public Transform snakePos;
    public Transform worldPoint1;
    public Transform worldPoint2;

    public float adjustmentFactor = 1f;

    public AudioSource pingAudio;

    float minimapRatio;

    bool exposedBatteryLoc = false;

    // Start is called before the first frame update
    void Start()
    {
        CalculateMapRatio();
    }

    // Update is called once per frame
    void Update()
    {
        Track(playerMinimapAnchor, PlayerPos);
        Track(snakeMinimapAnchor, snakePos);

        if (tracker.enabled && tracker.batteryLevel <= 50 && !exposedBatteryLoc)
        {
            exposedBatteryLoc = true;
            ExposeBatteryLoc();
        }

        if (tracker.batteryLevel > 50)
        {
            HideBatteryLoc();
        }
    }

    void ExposeBatteryLoc()
    {
        GameObject[] batteries = GameObject.FindGameObjectsWithTag("Battery");

        int rand = Random.Range(0, batteries.Length - 1);

        batteryAnchor.gameObject.SetActive(true);

        Track(batteryAnchor, batteries[rand].transform);
    }
    
    void HideBatteryLoc()
    {
        batteryAnchor.gameObject.SetActive(false);
        exposedBatteryLoc = false;
    }

    public void MarkSwitchLocation(Transform switchTransform)
    {
        GameObject switchAnchor = Instantiate(switchAnchorPrefab);
        switchAnchor.transform.SetParent(screen.transform);
        switchAnchor.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        switchAnchor.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 0f);
        switchAnchor.GetComponent<RectTransform>().localScale = new Vector3(0.003f, 0.0025f, 0.0025f);

        Track(switchAnchor.GetComponent<RectTransform>(), switchTransform);
    }

    void Track(RectTransform minimapAnchor, Transform pos)
    {
        minimapAnchor.anchoredPosition = minimapPoint1.anchoredPosition +
            new Vector2((pos.position.x - worldPoint1.position.x) * minimapRatio * adjustmentFactor, (pos.position.z - worldPoint1.position.z) * minimapRatio * adjustmentFactor);
    }

    void CalculateMapRatio()
    {
        // distance world ignoring Y axis
        Vector2 distanceWorldVector = worldPoint1.position - worldPoint2.position;
        distanceWorldVector.y = 0f;
        float distanceWorld = distanceWorldVector.magnitude;

        // distance minimap
        float distanceMinimap = Mathf.Sqrt(
            Mathf.Pow((minimapPoint1.anchoredPosition.x - minimapPoint2.anchoredPosition.x), 2) +
            Mathf.Pow((minimapPoint1.anchoredPosition.y - minimapPoint2.anchoredPosition.y), 2));

        minimapRatio = distanceMinimap / distanceWorld;
    }

    public void Ping()
    {
        pingAudio.Play();
    }

}
