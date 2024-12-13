using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnakeMesh : MonoBehaviour
{
    public GameObject skin;
    public GameObject eyes;
    public GameObject teeth;
    public GameObject tongue;

    // Settings
    public float SteerSpeed = 180;
    public int Gap = 10;
    public float distanceBeforeAvoiding = 5f;

    // References
    public GameObject BodyPrefab;
    public GameObject AnchorPrefab;
    public List<GameObject> bodyParts = new List<GameObject>();

    [Header("Gap Auto Adjust Settings")]
    public int maxSpeed = 8;
    public int minGap = 10;
    public int maxGap = 10;

    public float positionUpdateInterval = 0.1f; // Adjust this interval as needed
    private float timeSinceLastUpdate = 0.0f;

    // Lists
    [HideInInspector]
    public List<GameObject> points = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    private Snake snakeCont;
    private NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        snakeCont = GetComponent<Snake>();
        nav = GetComponent<NavMeshAgent>();

        for (int i = 0; i < bodyParts.Count; i++)
        {
            GrowSnake();

            GameObject anchor = Instantiate(AnchorPrefab);
            anchor.transform.position = bodyParts[i].transform.position;
            bodyParts[i].transform.SetParent(anchor.transform);
            bodyParts[i].transform.localPosition = new Vector3(0f, -1.333333f, 0f);

            anchor.transform.SetParent(points[i].transform);
            anchor.transform.localPosition = new Vector3(0f, 0f, 0f);
            anchor.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

            if (i == 0)
            {
                anchor.transform.localRotation = Quaternion.Euler(-90f, -90f, 0);
                anchor.transform.localPosition = new Vector3(0f, -1.5f, 0f);
            }
        }
       //StartCoroutine(StartBlockingBackTracking());
    }

    IEnumerator StartBlockingBackTracking()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 1; i < bodyParts.Count; i++)
        {
            points[i].GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate desired gap based on the snake's speed
        float desiredGap = Mathf.Lerp(maxGap, minGap, nav.speed / maxSpeed);

        // Update Gap based on desiredGap
        Gap = Mathf.RoundToInt(desiredGap);

        // New
        timeSinceLastUpdate += Time.deltaTime;

        // Update position history at a fixed interval
        if (timeSinceLastUpdate >= positionUpdateInterval)
        {
            PositionsHistory.Insert(0, transform.position);
            timeSinceLastUpdate = 0.0f;

            // New

            // Store position history
            // PositionsHistory.Insert(0, transform.position);

            // Move body parts
            int index = 0;
            foreach (var body in points)
            {
                Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];

                // Move body towards the point along the snakes path
                Vector3 moveDirection = point - body.transform.position;

                //float desiredSpeed = nav.velocity.magnitude;

                body.transform.position += moveDirection * nav.speed * Time.deltaTime;

                // Rotate body towards the point along the snakes path
                body.transform.LookAt(point);

                index++;
            }
        }          
    }

    private void GrowSnake()
    {
        // Instantiate body instance and
        // add it to the list
        GameObject body = Instantiate(BodyPrefab);
        points.Add(body);
    }

    private void OnDrawGizmos()
    {
        if (nav == null)
            return;

        Gizmos.color = Color.red;

        if (nav.path != null && nav.path.corners.Length > 1)
        {
            Vector3[] pathCorners = nav.path.corners;

            for (int i = 1; i < pathCorners.Length; i++)
            {
                Gizmos.DrawLine(pathCorners[i - 1], pathCorners[i]);
            }
        }
    }
}