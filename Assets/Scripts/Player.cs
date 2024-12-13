using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public bool alive = true;

    public PlayerMovement playerMovement;
    public PlayerCamera playerCamera;
    public Tracker tracker;
    public Animator cameraAnimator;
    public CharacterQuipManager quipManager;

    public GameObject snake;

    public float wakeUpTime = 5.5f;

    [Header("Camera Shake")]
    public float snakeDistanceThreshold = 4f;

    // Start is called before the first frame update
    void Start()
    {
        // Show Starting quip 
        quipManager.AddSubtitleToQueue("", 0.8f);
        quipManager.AddSubtitleToQueue("How did I end up here?", 3.5f);
        quipManager.AddSubtitleToQueue("I need to leave.. now", 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (alive)
        {
            if (other.gameObject.name == "eyes")
            {
                other.transform.parent.GetComponent<Snake>().CheckSight();
            }

            if (other.gameObject.tag == "Snake Body Segment")
            {
                playerMovement.camAnimator.SetBool("Near Snake", true);

                float blend = Vector3.Distance(transform.position, other.gameObject.transform.position) / snakeDistanceThreshold;
                playerMovement.camAnimator.SetFloat("ShakeBlend", blend);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Snake Body Segment")
        {
            playerMovement.camAnimator.SetBool("Near Snake", false);
        }
    }

    public IEnumerator WakeUp()
    {
        cameraAnimator.SetBool("Waking Up", true);
        cameraAnimator.Play("WakeUp");

        // Save Player Rotation
        Quaternion rotation = transform.rotation;

        playerMovement.sanitySlider.gameObject.SetActive(false);
        playerMovement.staminaSlider.gameObject.SetActive(false);
        playerMovement.enabled = false;
        playerCamera.enabled = false;
        tracker.enabled = false;

        snake.GetComponent<Snake>().enabled = false;
        snake.GetComponent<SnakeMesh>().enabled = false;
        snake.GetComponent<SnakeMesh>().enabled = false;
        snake.GetComponent<NavMeshAgent>().enabled = false;

        StartCoroutine(PrepSkin());

        yield return new WaitForSeconds(wakeUpTime);

        playerMovement.enabled = true;
        playerMovement.sanitySlider.gameObject.SetActive(true);
        playerCamera.enabled = true;
        tracker.enabled = true;

        cameraAnimator.SetBool("Waking Up", false);

        transform.rotation = rotation;

        if (InstructionsManager.firstTimePlaying == false)
        {
            ActivateSnake();
        }
    }
    public void ActivateSnake()
    {
        snake.GetComponent<Snake>().enabled = true;
        snake.GetComponent<SnakeMesh>().enabled = true;
        snake.GetComponent<NavMeshAgent>().enabled = true;
    }

    IEnumerator PrepSkin()
    {
        SnakeMesh snakeMesh = snake.GetComponent<SnakeMesh>();

        snakeMesh.skin.SetActive(false);
        snakeMesh.eyes.SetActive(false);
        snakeMesh.tongue.SetActive(false);
        snakeMesh.teeth.SetActive(false);

        yield return new WaitForSeconds(wakeUpTime + 3f);

        snakeMesh.skin.SetActive(true);
        snakeMesh.eyes.SetActive(true);
        snakeMesh.tongue.SetActive(true);
        snakeMesh.teeth.SetActive(true);
    }
}
