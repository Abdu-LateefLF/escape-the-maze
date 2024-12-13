using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private Animator animator;

    [HideInInspector]
    public SwitchSpawner switchSpawner;

    public GameObject exitDoor;
    public bool flippedOn = false;
    public AudioSource clickSound;
    public AudioSource beepSound;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (flippedOn == false)
        {
            flippedOn = true;

            animator.Play("FlipOn");

            exitDoor.GetComponent<ExitDoor>().numSwitchesOn++;

            switchSpawner.objectiveSystem.UpdateObjective();

            // Check for quips
            ExitDoor exit = exitDoor.GetComponent<ExitDoor>();

            exit.miniMap.MarkSwitchLocation(this.transform);

            if (exit.numSwitchesOn == 1)
            {
                // first switch
                exit.quipManager.ClearSubtitleQueue();
                exit.quipManager.AddSubtitleToQueue("That seems like it did something", 3.7f);
            }

            if (exit.numSwitchesOn == exit.requiredAmountOfSwitches)
            {
                // last switch
                exit.quipManager.ClearSubtitleQueue();
                exit.quipManager.AddSubtitleToQueue("That should be the last one", 3.2f);
                exit.quipManager.AddSubtitleToQueue("I need to find the exit", 3.7f);
            }
        }
    }

    public void OnLeverReachBottom()
    {
        clickSound.Play();
    }

    public void Beep()
    {
        beepSound.Play();
    }
}
