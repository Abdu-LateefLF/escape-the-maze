using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tracker : MonoBehaviour
{
    public GameHandler gameHandler;

    public float batteryLevel = 100f;
    public float switchNearbyDistance = 20f;

    private float batteryLossSpeed = 2.5f;

    [Header("Modes/Easy")]
    public float EMbatteryLossSpeed = 2.5f;
    [Header("Modes/Normal")]
    public float NMbatteryLossSpeed = 4f;
    [Header("Modes/Hard")]
    public float HMbatteryLossSpeed = 6f;
    [Header("Modes/Very Hard")]
    public float VHMbatteryLossSpeed = 8.5f;
    [Header("Modes/Extreme")]
    public float XMbatteryLossSpeed = 8.5f;

    public Animator animator;
    public AudioSource batteryInsertSound;

    [Header("Sceen States")]
    public GameObject activeScreen;
    public GameObject screen1;
    public GameObject screen2;
    public GameObject inActiveScreen;
    public GameObject switchNearbyOverlay;

    [Header("Bars")]
    public Image bar1;
    public Image bar2;
    public Image bar3;
    public Image bar4;

    // Stats
    [HideInInspector]
    public int numBatteriesUsed = 0;

    bool onScreen1 = true;
    public bool lowBattery = false;
    public bool isSwitchNearby = false;

    private GameObject[] switches;

    // Start is called before the first frame update
    void Start()
    {
        batteryLevel = 100f;

        // Update according to mode
        switch (MainMenu.mode)
        {
            case 0:
                batteryLossSpeed = EMbatteryLossSpeed;
                break;
            case 1:
                batteryLossSpeed = NMbatteryLossSpeed;
                break;
            case 2:
                batteryLossSpeed = HMbatteryLossSpeed;
                break;
            case 3:
                batteryLossSpeed = VHMbatteryLossSpeed;
                break;
            case 4:
                batteryLossSpeed = XMbatteryLossSpeed;
                break;
        }

        switches = GameObject.FindGameObjectsWithTag("Switch");
        switchNearbyOverlay.SetActive(false);

        // Check if a switch is nearby
        StartCoroutine(StartCheckSwitchNearby());
    }

    // Update is called once per frame
    void Update()
    {
        batteryLevel -= batteryLossSpeed * Time.deltaTime;

        bar4.gameObject.SetActive(batteryLevel >= 75);     
        bar3.gameObject.SetActive(batteryLevel >= 50);     
        bar2.gameObject.SetActive(batteryLevel >= 25);    
        bar1.gameObject.SetActive(batteryLevel > 0);    

        if (batteryLevel <= 0)
        {
            batteryLevel = 0;
            DisableCell();
        }
    }

    IEnumerator StartCheckSwitchNearby()
    {
        yield return new WaitForSeconds(0.5f);

        CheckSwitchNearby();
    }

    void CheckSwitchNearby()
    {
        bool temp = false;

        for (int i = 0; i < switches.Length; i++)
        {
            if (Vector3.Distance(transform.position, switches[i].transform.position) < switchNearbyDistance && switches[i].gameObject.GetComponent<Switch>().flippedOn == false)
            {
                temp = true; break;
            }
        }

        isSwitchNearby = temp;
        switchNearbyOverlay.SetActive(isSwitchNearby);

        // Check if a switch is nearby
        StartCoroutine(StartCheckSwitchNearby());
    }

    public void DisableCell()
    {
        activeScreen.SetActive(false);  
        inActiveScreen.SetActive(true);

        lowBattery = true;
    }

    public void EnableCell()
    {
        activeScreen.SetActive(true);
        inActiveScreen.SetActive(false);

        batteryLevel = 100;

        lowBattery = false;
    }

    public void SwitchToScreen1()
    {
        if (onScreen1 == false && !lowBattery)
        {
            animator.Play("Button1_001");
        }
    }

    public void SwitchToScreen2()
    {
        if (onScreen1 == true && !lowBattery)
        {
            animator.Play("Button2");
        }
    }

    public void ShowScreen1()
    {
        screen1.SetActive(true);
        screen2.SetActive(false);

        onScreen1 = true;
    }

    public void ShowScreen2()
    {
        screen1.SetActive(false);
        screen2.SetActive(true);

        onScreen1 = false;
    }

    public void InsertBattery()
    {
        batteryInsertSound.Play();

        numBatteriesUsed++;
        gameHandler.CheckBatteryUsedCount();
    }
}
