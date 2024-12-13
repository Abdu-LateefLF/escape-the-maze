using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SojaExiles;

public class InteractionHandler : MonoBehaviour
{
    public float interactDistance = 3.6f;
    public float checkInteractableDistance = 5f;

    public Transform interactionCamera;
    public Button interactButton;
    public Image selectIndicator;
    public Image tooFarIndicator;
    public InstructionsManager instrucManager;

    [Header("Cursor Image")]
    public Sprite ableToInteractImage;
    public Sprite lockedImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit rayHit;

        Vector3 endPos = (interactionCamera.forward * interactDistance) + interactionCamera.position;

        Debug.DrawLine(interactionCamera.position, endPos, Color.red);
        if (Physics.Linecast(interactionCamera.position, endPos, out rayHit))
        {
            if (rayHit.collider.gameObject.layer == 6)
            {
                interactButton.interactable = true;
                selectIndicator.gameObject.SetActive(true);
                selectIndicator.sprite = ableToInteractImage;

                // Check if we need to show interact instructions
                instrucManager.CheckShowInteractInstructions();

                if (rayHit.collider.gameObject.GetComponent<Switch>() != null)
                {
                    Switch lever = rayHit.collider.gameObject.GetComponent<Switch>();
                    if (lever.flippedOn == true)
                    {
                        interactButton.interactable = false;
                        selectIndicator.gameObject.SetActive(false);

                        // Stop showing interact instructions
                        instrucManager.TempStopInteractInstructions();
                    }
                }
                // Exit Gates
                if (rayHit.collider.gameObject.GetComponent<ExitDoor>() != null)
                {
                    ExitDoor exitDoor = rayHit.collider.gameObject.GetComponent<ExitDoor>();
                    if (exitDoor.numSwitchesOn >= exitDoor.requiredAmountOfSwitches)
                    {
                        selectIndicator.sprite = ableToInteractImage;
                    }
                    else
                    {
                        selectIndicator.sprite = lockedImage;
                    }
                }
            }
            else
            {
                interactButton.interactable = false;
                selectIndicator.gameObject.SetActive(false);

                // Stop showing interact instructions
                instrucManager.TempStopInteractInstructions();
            }
        }
        else
        {
            interactButton.interactable = false;
            selectIndicator.gameObject.SetActive(false);

            // Stop showing interact instructions
            instrucManager.TempStopInteractInstructions();
        }

        // Far but interactable
        RaycastHit farHit;
        Vector3 farHitEndPos = (interactionCamera.forward * checkInteractableDistance) + interactionCamera.position;

        Debug.DrawLine(interactionCamera.position, farHitEndPos, Color.red);
        if (Physics.Linecast(interactionCamera.position, farHitEndPos, out farHit))
        {
            if (farHit.collider.gameObject.layer == 6 && farHit.distance > interactDistance)
            {
                tooFarIndicator.gameObject.SetActive(true);

                if (rayHit.collider != null)
                {
                    if (rayHit.collider.gameObject.GetComponent<Switch>() != null)
                    {
                        Switch lever = rayHit.collider.gameObject.GetComponent<Switch>();
                        if (lever.flippedOn == true)
                        {
                            tooFarIndicator.gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                tooFarIndicator.gameObject.SetActive(false);
            }
        }
        else
        {
            tooFarIndicator.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        RaycastHit rayHit;

        Vector3 endPos = (interactionCamera.forward * interactDistance) + interactionCamera.position;

        Debug.DrawLine(interactionCamera.position, endPos, Color.red);
        if (Physics.Linecast(interactionCamera.position, endPos, out rayHit))
        {
            if (rayHit.collider.gameObject.layer == 6)
            {
                if (rayHit.collider.gameObject.GetComponent<Item>() != null)
                {
                    rayHit.collider.gameObject.GetComponent<Item>().UseItem();
                    print("called use");
                }
                if (rayHit.collider.gameObject.GetComponent<Drawer_Pull_X>() != null)
                {
                    rayHit.collider.gameObject.GetComponent<Drawer_Pull_X>().Interact();
                }
                if (rayHit.collider.gameObject.GetComponent<opencloseDoor>() != null)
                {
                    rayHit.collider.gameObject.GetComponent<opencloseDoor>().Interact();
                }
                if (rayHit.collider.gameObject.GetComponent<opencloseDoor1>() != null)
                {
                    rayHit.collider.gameObject.GetComponent<opencloseDoor1>().Interact();
                }

                if (rayHit.collider.gameObject.GetComponent<Switch>() != null)
                {
                    Switch lever = rayHit.collider.gameObject.GetComponent<Switch>();
                    if (lever.flippedOn == false)
                        lever.Interact();
                }

                // Exit Door
                if (rayHit.collider.gameObject.GetComponent<ExitDoor>() != null)
                {
                    rayHit.collider.gameObject.GetComponent<ExitDoor>().Interact();
                }
            }
        }
    }
}
