using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MenuRat : MonoBehaviour
{
    public string state = "idle";

    public float runSpeed = 7f;
    public float roamSpeed = 2.5f;
    public float hoverSpeed = 3f;
    public float minIdleTime = 2f;
    public float maxIdleTime = 8f;

    public float searchRadius = 20f;

    public int chanceOfHover = 30;
    public int maxItemHovers = 3;
    public float itemCheckRadius = 30f;
    public float hoverItemRadius = 0.3f;

    bool isFleeing = false;
    bool runningFromSnake = false;
    public bool spotItem = false;

    private NavMeshAgent nav;
    public Animator animator;
    public GameObject mesh;
    public GameObject mainCamera;

    private int hoverIndex = 0;
    private Vector3 itemPosition;
    private GameObject prevItem;

    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "idle")
            IdleState();
        if (state == "spotted item")
            SpottedItem();
        if (state == "hover near item")
            HoverNearItem();
        if (state == "wait")
            WaitState();
        if (state == "walk")
            WalkState();

        // Update animation
        animator.SetFloat("Speed", nav.velocity.magnitude);
        animator.SetBool("Hovering Item", spotItem);

        mesh.SetActive(mainCamera.activeSelf);
    }

    void IdleState()
    {
        isFleeing = false;

        waitTime = Random.Range(minIdleTime, maxIdleTime);

        // Check chance of hover
        bool shouldHover = Random.Range(0, 100) <= chanceOfHover;

        // Check for item nearby
        if (shouldHover)
        {
            bool foundSomething = false;

            var hitColliders = Physics.OverlapSphere(transform.position, itemCheckRadius);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.GetComponent<Item>())
                {
                    if (hitColliders[i].gameObject != prevItem)
                    {
                        itemPosition = hitColliders[i].transform.position;
                        prevItem = hitColliders[i].gameObject;
                        state = "spotted item";

                        foundSomething = true;
                        break;
                    }
                }
            }

            if (foundSomething == false)
                state = "wait";
        }

        else
            state = "wait";
    }

    void WaitState()
    {
        waitTime -= Time.deltaTime;

        nav.SetDestination(transform.position);

        if (waitTime <= 0)
        {
            waitTime = 0;
            // pick a random place to walk
            Vector3 randomPos = Random.insideUnitSphere * searchRadius;

            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomPos, out navHit, searchRadius, NavMesh.AllAreas);
            nav.speed = roamSpeed;

            nav.SetDestination(navHit.position);

            if (spotItem)
                state = "hover near item";
            else
                state = "walk";
        }
    }

    void WalkState()
    {
        if (GetPathRemainingDistance() <= nav.stoppingDistance && !nav.pathPending)
        {
            if (spotItem)
            {
                print("Reached Distance");
                state = "wait";
            }
            else
                state = "idle";
        }
    }

    void SpottedItem()
    {
        spotItem = true;
        hoverIndex = 0;

        state = "hover near item";
    }

    void HoverNearItem()
    {
        hoverIndex++;

        if (hoverIndex < 3)
        {
            Vector3 randomPos = Random.insideUnitSphere * hoverItemRadius;
            NavMeshHit navHit;
            NavMesh.SamplePosition(itemPosition + randomPos, out navHit, hoverItemRadius, NavMesh.AllAreas);
            nav.speed = hoverSpeed;

            nav.SetDestination(navHit.position);

            waitTime = Random.Range(minIdleTime, maxIdleTime);

            print("Hovering");
            state = "walk";
        }

        else if (hoverIndex >= 3)
        {
            print("Finished Hovering");

            var randomPos = new Vector3(Random.Range(-hoverItemRadius, hoverItemRadius), 0, Random.Range(-hoverItemRadius, hoverItemRadius));

            NavMeshHit navHit;
            NavMesh.SamplePosition(itemPosition + randomPos, out navHit, hoverItemRadius, NavMesh.AllAreas);
            nav.speed = hoverSpeed;

            nav.SetDestination(navHit.position);

            spotItem = false;

            waitTime = Random.Range(minIdleTime, maxIdleTime);

            state = "idle";
        }
    }

    public float GetPathRemainingDistance()
    {
        if (nav.pathPending ||
            nav.pathStatus == NavMeshPathStatus.PathInvalid ||
            nav.path.corners.Length == 0)
            return -1f;

        float distance = 0.0f;
        for (int i = 0; i < nav.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(nav.path.corners[i], nav.path.corners[i + 1]);
        }

        return distance;
    }
}
