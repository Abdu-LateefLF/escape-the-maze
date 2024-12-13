using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveSystem : MonoBehaviour
{
    public Text objectiveText;
    public Animator animator;

    public ExitDoor exitDoor;

    private void Start()
    {
    }

    public void UpdateObjective()
    {
        if (exitDoor.numSwitchesOn < exitDoor.requiredAmountOfSwitches)
        {
            objectiveText.text = exitDoor.numSwitchesOn.ToString() + "/" + exitDoor.requiredAmountOfSwitches.ToString();
        }
        else
        {
            objectiveText.text = "Find the exit door";
        }

        animator.Play("UpdateAnim");
    }
}
