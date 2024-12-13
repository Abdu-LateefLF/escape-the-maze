using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public static bool firstTimePlaying = true;

    public GameObject lookInstrucObject;
    public GameObject moveInstrucObject;
    public GameObject runInstrucObject;
    public GameObject leftArrowInstrucObject;
    public GameObject rightArrowInstrucObject;
    public GameObject interactInstrucObject;

    public GameObject runIcon;
    public GameObject leftSwitchIcon;
    public GameObject rightSwitchIcon;
    public GameObject interactIcon;

    public GameObject player;

    public FixedTouchField touchField;
    public FloatingJoystick moveJoystick;

    public GameObject bars;

    public float lookCompleteDistance = 10f;
    public float startDelay = 5f;
    public float delayBetweenInstructions = 0.5f;

    Player playerClass;
    CharacterController cController;

    bool hasStartedInstructions = false;
    bool lookedAround = false;
    bool moved = false;
    bool hasRan = false;
    bool hasSwitchedLeftScreen = false;
    bool hasSwitchedRightScreen = false;
    bool hasInteracted = false;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("PlayedAlready"))
            firstTimePlaying = PlayerPrefs.GetInt("PlayedAlready") < 1;

        if (PlayerPrefs.HasKey("InteractedAlready"))
            hasInteracted = PlayerPrefs.GetInt("InteractedAlready") > 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (firstTimePlaying)
        {
            playerClass = player.GetComponent<Player>();
            cController = player.GetComponent<CharacterController>();

            StartCoroutine(StartWithDelay());
        }
    }

    IEnumerator StartWithDelay()
    {
        lookInstrucObject.SetActive(false);
        moveInstrucObject.SetActive(false);
        runInstrucObject.SetActive(false);
        leftArrowInstrucObject.SetActive(false);
        rightArrowInstrucObject.SetActive(false);
        interactInstrucObject.SetActive(false);

        leftSwitchIcon.SetActive(false);
        rightSwitchIcon.SetActive(false);
        interactIcon.SetActive(false);

        int numChildren = touchField.transform.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            touchField.transform.GetChild(i).gameObject.SetActive(false);
        }

        moveJoystick.gameObject.SetActive(false);
        bars.SetActive(false);

        yield return new WaitForSeconds(startDelay);

        StartInstructions();
    }

    void StartInstructions()
    {
        hasStartedInstructions = true;
        lookInstrucObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (firstTimePlaying && hasStartedInstructions)
        {
            // If they haven't looked around
            if (lookedAround == false)
            {
                if (touchField.TouchDist.sqrMagnitude > lookCompleteDistance)
                {
                    lookedAround = true;

                    StartCoroutine(StartMoveInstructions());
                }
            }
            else
            {
                Vector2 moveDirection = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);

                if (!moved && moveDirection.magnitude > 0)
                {
                    playerClass.ActivateSnake();

                    StartCoroutine(StartRunInstructions());
                }
            }
        }
    }

    IEnumerator StartMoveInstructions()
    {
        yield return new WaitForSeconds(delayBetweenInstructions);

        lookInstrucObject.SetActive(false);

        moveJoystick.gameObject.SetActive(true);
        moveInstrucObject.gameObject.SetActive(true);
    }

    IEnumerator StartRunInstructions()
    {
        moved = true;
        moveInstrucObject.SetActive(false);

        yield return new WaitForSeconds(delayBetweenInstructions);

        runIcon.SetActive(true);

        playerClass.ActivateSnake();

        bars.SetActive(true);

        if (hasRan == false)
        runInstrucObject.SetActive(true);
    }

    public void CompleteRunInstructions()
    {
        if (firstTimePlaying && !hasRan)
        {
            hasRan = true;

            runInstrucObject.SetActive(false);

            StartCoroutine(StartRightArrowInstructions());
        }
    }

    IEnumerator StartRightArrowInstructions()
    {
        yield return new WaitForSeconds(delayBetweenInstructions);

        rightSwitchIcon.SetActive(true);
        rightArrowInstrucObject.SetActive(true);
    }

    public void CompleteRightArrowInstructions()
    {
        if (firstTimePlaying && !hasSwitchedRightScreen)
        {
            hasSwitchedRightScreen = true;
            rightArrowInstrucObject.SetActive(false);

            StartCoroutine(StartLeftArrowInstructions());
        }
    }

    IEnumerator StartLeftArrowInstructions()
    {
        yield return new WaitForSeconds(delayBetweenInstructions);

        leftSwitchIcon.SetActive(true); 
        leftArrowInstrucObject.SetActive(true);
    }

    public void CompleteLeftArrowInstructions()
    {
        if (firstTimePlaying && !hasSwitchedLeftScreen)
        {
            hasSwitchedLeftScreen = true;
            leftArrowInstrucObject.SetActive(false);

            firstTimePlaying = false;

            // Save
            PlayerPrefs.SetInt("PlayedAlready", 1);

            // Enable Interact Icon
            interactIcon.SetActive(true);
        }
    }

    public void CheckShowInteractInstructions()
    {
        if (hasInteracted == false)
        {
            interactInstrucObject.SetActive(true);
        }
    }

    public void TempStopInteractInstructions()
    {
        if (hasInteracted == false)
        {
            interactInstrucObject.SetActive(false);
        }
    }

    public void CompleteInteractInstructions()
    {
        if (hasInteracted == false)
        {
            interactInstrucObject.SetActive(false);

            hasInteracted = true;

            // Save 
            PlayerPrefs.SetInt("InteractedAlready", 1);
        }
    }
}
