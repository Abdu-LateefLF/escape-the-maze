using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    public Transform player;
    public GameObject monster;

    public Text distanceText;
    public Text statusText;

    public Color passiveColor;
    public Color activeColor;

    public AudioSource pulseAudio;

    public void Pulse()
    {
        float distance = Vector3.Distance(player.position, monster.transform.position);
        distanceText.text = "[Distance " + Mathf.RoundToInt(distance).ToString() + "]";

        // Get Snake Status
        string status = "PASSIVE";
        statusText.color = passiveColor;

        Snake snake = monster.GetComponent<Snake>();
        
        if (snake.active == true)
        {
            status = "ACTIVE";
            statusText.color = activeColor;
        }

        statusText.text = status;
    }

    public void PlayPulseSound()
    {
        pulseAudio.Play();
    }
}

