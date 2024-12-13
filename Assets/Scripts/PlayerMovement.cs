using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public GameHandler gameHandler;
    public Animator fpHands;

    public FloatingJoystick moveJoystick;

    public Snake snake;
    public Camera cam;
    public GameObject flashlight;
    public Animator camAnimator;

    private float moveSpeed;
    public float walkSpeed = 12f;
    public float runSpeed = 25f;
    public float gravityDownwardSpeed = 60f;

    public bool isSprinting = false;
    public bool running = false;

    [HideInInspector]
    public float stamina;
    [HideInInspector]
    public float restTimeElasped;

    public float maxStamina = 10;
    private float staminaDecaySpeed = 50f;
    private float staminaRegenSpeed = 30f;
    public Slider staminaSlider;

    public float walkingStepLoudness = 0.8f;
    public float minTimeBetweenStepWhileWalking = 1f;
    public float runningStepLoudness = 1.2f;
    public float minTimeBetweenStepWhileRunning = 0.4f;
    public float stepDelay = 0.4f;
    public AudioSource footStepAudioSource;
    public AudioClip[] footsteps;

    // Stats
    [HideInInspector]
    public int numPillsConsumed = 0;
    [HideInInspector]
    public int numRatsStartled = 0;

    bool isResting = false;

    [HideInInspector]
    public float sanity;

    public float maxSanity = 10;
    public float sanityEffectThreshold = 60f;
    public float sanityPulseZoomIn = 50f;
    public float beatSpeed = 1.3f;
    public float flashlightBlinkSpeed = 0.25f;
    public float timeBetweenHeartbeat = 4f;
    private float sanityLossSpeed = 50f;
    public Slider sanitySlider;
    public AudioSource takePillSound;
    public AudioSource heartbeatSound;

    bool isSane = false;

    public float restTime = 12;
    public Image staminaBarColor;
    public Color staminaColor;
    public Color restColor;

    private float lastTimeSinceStep;
    private float timeElapsedSinceHeartbeat = 0;

    [HideInInspector]
    public float initCamFov;

    // Modes
    [Header("Modes/Easy")]
    public float EMstaminaDecaySpeed = 20f;
    public float EMstaminaRegenSpeed = 10f;
    public float EMsanityLossSpeed = 4f;
    [Header("Modes/Normal")]
    public float NMstaminaDecaySpeed = 25f;
    public float NMstaminaRegenSpeed = 10f;
    public float NMsanityLossSpeed = 5f;
    [Header("Modes/Hard")]
    public float HMstaminaDecaySpeed = 30f;
    public float HMstaminaRegenSpeed = 5f;
    public float HMsanityLossSpeed = 8f;
    [Header("Modes/Very Hard")]
    public float VHMstaminaDecaySpeed = 35f;
    public float VHMstaminaRegenSpeed = 2f;
    public float VHMsanityLossSpeed = 10f;
    public float VHMrestTime = 16.5f;
    [Header("Modes/Extreme")]
    public float XMstaminaDecaySpeed = 35f;
    public float XMstaminaRegenSpeed = 2f;
    public float XMsanityLossSpeed = 10f;
    public float XMrestTime = 16.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Update according to mode
        switch (MainMenu.mode)
        {
            case 0:
                staminaDecaySpeed = EMstaminaDecaySpeed;
                staminaRegenSpeed = EMstaminaRegenSpeed;
                sanityLossSpeed = EMsanityLossSpeed;
                break;
            case 1:
                staminaDecaySpeed = NMstaminaDecaySpeed;
                staminaRegenSpeed = NMstaminaRegenSpeed;
                sanityLossSpeed = NMsanityLossSpeed;
                break;
            case 2:
                staminaDecaySpeed = HMstaminaDecaySpeed;
                staminaRegenSpeed = HMstaminaRegenSpeed;
                sanityLossSpeed = HMsanityLossSpeed;
                break;
            case 3:
                staminaDecaySpeed = VHMstaminaDecaySpeed;
                staminaRegenSpeed = VHMstaminaRegenSpeed;
                sanityLossSpeed = VHMsanityLossSpeed;
                restTime = VHMrestTime;
                break;
            case 4:
                staminaDecaySpeed = XMstaminaDecaySpeed;
                staminaRegenSpeed = XMstaminaRegenSpeed;
                sanityLossSpeed = XMsanityLossSpeed;
                restTime = XMrestTime;
                break;
        }

        moveSpeed = walkSpeed;
        initCamFov = cam.fieldOfView;

        stamina = maxStamina;
        staminaSlider.maxValue = maxStamina;

        sanity = maxSanity;
        sanitySlider.maxValue = maxSanity;
    }

    // Update is called once per frame
    void Update()
    {
        float x = moveJoystick.Horizontal;
        float z = moveJoystick.Vertical;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        UpdateSanity();

        // Update Stamina
        UpdateStamina();

        controller.Move(Vector3.down * gravityDownwardSpeed * Time.deltaTime);

        lastTimeSinceStep += Time.deltaTime;
    }

    void UpdateSanity()
    {
        sanity -= sanityLossSpeed * Time.deltaTime;

        // Update effects
        if (sanity < sanityEffectThreshold)
        {
            timeElapsedSinceHeartbeat += Time.deltaTime;

            int dangerLevel = 1;
            if (sanity < 40)
                dangerLevel = 2;
            if (sanity < 20)
                dangerLevel = 3;
            if (sanity < 1)
                dangerLevel = 4;

            if (timeElapsedSinceHeartbeat >= (timeBetweenHeartbeat / dangerLevel))
            {
                // Play Heart Beat
                heartbeatSound.Play();
                StartCoroutine(Pulse());
                timeElapsedSinceHeartbeat = 0;

                if (Random.Range(1,3) == 1)
                StartCoroutine(FlashlightBlink());
            }
        }

        if (sanity >= maxSanity)
        {
            sanitySlider.gameObject.SetActive(false);
        }
        else
        {
            sanitySlider.gameObject.SetActive(true);
        }

        // If the player is insane
        if (sanity <= 0)
        {
            sanity = 0;
            isSane = false;

            StopSprint();

            stamina = 0;
            staminaSlider.gameObject.SetActive(true);
            isResting = true;
            restTimeElasped = restTime - 0.1f;

            // Make snake chase player
            if (snake.state != "kill")
            {
                snake.knowPlayerLocation = true;
                snake.state = "chase";
            }
        }
        else
        {
            isSane = true;

            snake.knowPlayerLocation = false;
        }

        sanitySlider.value = sanity;
    }

    public void TakePill()
    {
        takePillSound.Play();
    }

    IEnumerator Pulse()
    {
        float t = 0;
        while (cam.fieldOfView > sanityPulseZoomIn)
        {
            t += Time.deltaTime * beatSpeed;

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sanityPulseZoomIn, t);

            yield return new WaitForEndOfFrame();
        }

        t = 0;
        while (cam.fieldOfView < initCamFov)
        {
            t += Time.deltaTime * beatSpeed;

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, initCamFov, t);

            yield return new WaitForEndOfFrame();
        }

        flashlight.SetActive(true);

    }

    IEnumerator FlashlightBlink()
    {
        flashlight.SetActive(false);

        yield return new WaitForSeconds(flashlightBlinkSpeed);

        flashlight.SetActive(true);

        yield return new WaitForSeconds(flashlightBlinkSpeed);

        flashlight.SetActive(false);

        yield return new WaitForSeconds(flashlightBlinkSpeed);

        flashlight.SetActive(true);
    }

    void UpdateStamina()
    {
        running = isSprinting && (controller.velocity.magnitude > 2.5f);

        if (isResting == false)
        {
            if (running)
            {
                stamina -= staminaDecaySpeed * Time.deltaTime;
                staminaSlider.gameObject.SetActive(true);

                if (stamina <= 0)
                {
                    stamina = 0;
                    StopSprint();

                    restTimeElasped = 0;
                    isResting = true;
                }
            }

            else
            {
                stamina += staminaRegenSpeed * Time.deltaTime;

                if (stamina >= maxStamina)
                {
                    stamina = maxStamina;
                    staminaSlider.gameObject.SetActive(false);
                }
            }
        }

        staminaSlider.value = stamina;
        fpHands.SetFloat("Speed", controller.velocity.magnitude);

        if (isResting)
        {
            if (isSane)
            restTimeElasped += Time.deltaTime;

            staminaBarColor.color = restColor;
            staminaSlider.maxValue = restTime;
            staminaSlider.value = restTimeElasped;

            if (restTimeElasped >= restTime)
            {
                restTimeElasped = 0;
                staminaBarColor.color = staminaColor;
                staminaSlider.maxValue = maxStamina;

                isResting = false;
            }
        }
    }

    public void ToggleSprint()
    {
        if (isSprinting)
            StopSprint();
        else
            Sprint();
    }

    public void Sprint()
    {
        if (isResting == false && isSane)
        {
            isSprinting = true;
            moveSpeed = runSpeed;
        }
    }

    public void StopSprint()
    {
        isSprinting = false;
        moveSpeed = walkSpeed;
    }

    public void Footstep()
    {
        StartCoroutine(FinishStep());
    }

    IEnumerator FinishStep()
    {
        bool valid = false;

        float pitch = Random.Range(0.8f, 1.1f);
        int clipRandom = Random.Range(0, footsteps.Length - 1);

        footStepAudioSource.clip = footsteps[clipRandom];
        footStepAudioSource.pitch = pitch;

        // Play Camera Bob
        camAnimator.SetTrigger("Footstep");

        yield return new WaitForSeconds(stepDelay);

        if (isSprinting)
        {
            print("Footstep");

            footStepAudioSource.volume = runningStepLoudness;

            if (lastTimeSinceStep > minTimeBetweenStepWhileRunning)
                valid = true;
        }

        else
        {
            footStepAudioSource.volume = walkingStepLoudness;

            if (lastTimeSinceStep > minTimeBetweenStepWhileWalking)
                valid = true;
        }

        if (valid)
        {
            footStepAudioSource.Play();
            lastTimeSinceStep = 0;
        }
    }

    public void NoticeStartledRat()
    {
        numRatsStartled++;
        gameHandler.CheckRatAchievements();
    }
}
