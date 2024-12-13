using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaButton : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public Image image;
    public Sprite runSprite;
    public Sprite walkSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isSprinting)
        {
            image.sprite = walkSprite;
        }
        else
        {
            image.sprite = runSprite;
        }
    }
}
