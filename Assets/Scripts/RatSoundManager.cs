using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSoundManager : MonoBehaviour
{
    public AudioSource squealNoise;
    public AudioSource footstepSound;

    public void RunNoise()
    {
        if (squealNoise.isPlaying == false)
        {
            squealNoise.pitch = Random.Range(0.7f, 2.5f);
            squealNoise.Play();
        }
    }

    public void FootStep()
    {
        if (footstepSound.isPlaying == false)
        footstepSound.Play();   
    }
}
