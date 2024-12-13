using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSnakeHiss : MonoBehaviour
{
    public AudioSource hissSound;

    public float minTimeToPlay = 25f;
    public float maxTimeToPlay = 60f;

    public float minPitch = 0.5f;
    public float maxPitch = 1.7f;

    public float minVolume = 0.5f;
    public float maxVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        PlaySnakeSound();
    }

    void PlaySnakeSound()
    {
        StartCoroutine(Hiss());
    }

    IEnumerator Hiss()
    {
        float waitTime = Random.Range(minTimeToPlay, maxTimeToPlay);

        yield return new WaitForSeconds(waitTime);

        float pitch = Random.Range(minPitch, maxPitch);
        float volume = Random.Range(minVolume, maxVolume);

        hissSound.pitch = pitch;
        hissSound.volume = volume;
        hissSound.Play();

        // Loop
        PlaySnakeSound();
    }
}
