using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCamera;
    public GameObject deathCam;
    public Transform eyes;

    public Animator headAnimator;

    private Vector3 camPos;
    private Quaternion camRot;

    [HideInInspector]
    public NavMeshAgent nav;

    private SnakeMesh snakeBody;

    public string state = "idle";

    public float loseDistance = 10f;

    private float patrolSpeed = 2f;
    private float chaseSpeed = 4f;

    [Header("Modes/Easy")]
    public float EMpatrolSpeed = 3f;
    public float EMchaseSpeed = 4f;
    [Header("Modes/Normal")]
    public float NMpatrolSpeed = 4f;
    public float NMchaseSpeed = 5f;
    [Header("Modes/Hard")]
    public float HMpatrolSpeed = 5f;
    public float HMchaseSpeed = 7f;
    [Header("Modes/Very Hard")]
    public float VHMpatrolSpeed = 6f;
    public float VHMchaseSpeed = 7f;
    [Header("Modes/Extreme")]
    public float XMpatrolSpeed = 10f;
    public float XMchaseSpeed = 12f;

    public AudioSource hissSound;
    public AudioSource killSound;

    [Header("Kill")]
    public float killDistance = 4f;
    public float zoomInSpeed = 3f;
    public GameObject gameOverCanvas;
    public GameObject mainCanvas;

    [Header("States")]
    public float patrolRadius = 200f;
    public float searchRadius1 = 10f;
    public float searchRadius2 = 30f;

    // Stats
    [HideInInspector]
    public int numTimesSpottedPlayer = 0;

    public bool onlyChasePlayer = false;

    private bool alive = true;
    public bool highAlert = false;
    public bool active = false;
    public bool knowPlayerLocation = false;

    private int alertness = 10;

    // Start is called before the first frame update
    void Start()
    {
        // Update according to mode
        switch (MainMenu.mode)
        {
            case 0:
                patrolSpeed = EMpatrolSpeed;
                chaseSpeed = EMchaseSpeed;
                break;
            case 1:
                patrolSpeed = NMpatrolSpeed;
                chaseSpeed = NMchaseSpeed;
                break;
            case 2:
                patrolSpeed = HMpatrolSpeed;
                chaseSpeed = HMchaseSpeed;
                break;
            case 3:
                patrolSpeed = VHMpatrolSpeed;
                chaseSpeed = VHMchaseSpeed;
                break;
            case 4:
                patrolSpeed = XMpatrolSpeed;
                chaseSpeed = XMchaseSpeed;
                break;
        }

        nav = GetComponent<NavMeshAgent>();
        nav.speed = patrolSpeed;

        snakeBody = gameObject.GetComponent<SnakeMesh>();

        deathCam.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public bool CheckSight()
    {
        if (alive)
        {
            Debug.DrawLine(transform.position, player.transform.position, Color.green);

            RaycastHit rayHit;
            if (Physics.Linecast(transform.position, player.transform.position, out rayHit))
            {
                if (rayHit.collider.gameObject.name == "Player")
                {
                    if (state != "kill")
                    {
                        if (state != "chase" && hissSound.isPlaying == false)
                        {
                            hissSound.Play();
                        }

                        state = "chase";

                        numTimesSpottedPlayer++; // Update Stats
                    }

                    return true;
                }
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (state == "idle")
                IdleState();

            if (state == "walk")
                WalkState();

            if (state == "chase")
                ChaseState();

            if (state == "hunt")
                HuntState();

            if (state == "kill")
                KillState();
        }
    }

    void IdleState()
    {
        // pick a random place to walk
        Vector3 randomPos = Random.insideUnitSphere * patrolRadius;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position + randomPos, out navHit, patrolRadius, NavMesh.AllAreas);
        nav.speed = patrolSpeed;

        active = false;

        nav.SetDestination(navHit.position);

        headAnimator.SetBool("Sees Player", false);
        state = "walk";
    }

    void WalkState()
    {
        if (nav.remainingDistance <= nav.stoppingDistance + 4.0f && !nav.pathPending)
        {
            if (highAlert)
                state = "hunt";

            else
                state = "idle";
        }
    }

    void ChaseState()
    {
        nav.SetDestination(player.transform.position);
        nav.speed = chaseSpeed;
        headAnimator.SetBool("Sees Player", true);

        highAlert = true;
        active = true;
        alertness = 10;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (GetPathRemainingDistance() > loseDistance && !knowPlayerLocation && !CheckSight())
        {
            headAnimator.SetBool("Sees Player", false);
            state = "hunt";
        }

        else if (distance < killDistance)
        {
            deathCam.gameObject.SetActive(true);

            camPos = deathCam.transform.position;
            camRot = deathCam.transform.rotation;

            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Player>().alive = false;

            snakeBody.enabled = false;

            deathCam.transform.position = playerCamera.transform.position;
            deathCam.transform.rotation = playerCamera.transform.rotation;

            playerCamera.SetActive(false);
        
            StartCoroutine(Reset());

            headAnimator.Play("Armature|Kill Player");
            deathCam.gameObject.GetComponent<Animator>().Play("DeathCamera");

            killSound.Play();

            for (int i = 0; i < snakeBody.points.Count; i++)
            {
                if (snakeBody.points[i].GetComponent<AudioSource>() != null)
                snakeBody.points[i].GetComponent<AudioSource>().enabled = false;
            }

            state = "kill";
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

    void HuntState()
    {
        if (alertness > 0)
        {
            // pick a random place to near the player
            float radius = (alertness > 5) ? searchRadius1 : searchRadius2;

            Vector3 randomPos = Random.insideUnitSphere * radius;

            NavMeshHit navHit;
            NavMesh.SamplePosition(player.transform.position + randomPos, out navHit, radius, NavMesh.AllAreas);
            nav.SetDestination(navHit.position);
            active = true;

            nav.speed = chaseSpeed;

            state = "walk";
        }

        else
        {
            highAlert = false;
            nav.speed = patrolSpeed;
            state = "idle";
        }

        alertness -= 5;
    }

    void KillState()
    {
        deathCam.transform.position = Vector3.Slerp(deathCam.transform.position, camPos, zoomInSpeed * Time.deltaTime);
        deathCam.transform.rotation = Quaternion.Slerp(deathCam.transform.rotation, camRot, zoomInSpeed * Time.deltaTime);

        nav.SetDestination(transform.position);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1.25f);

        gameOverCanvas.SetActive(true);
        mainCanvas.SetActive(false);

    }
}
