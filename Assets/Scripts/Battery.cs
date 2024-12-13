using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Item
{
    public Tracker tracker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseItem()
    {
        itemSpawner.numBatteries -= 1;

        print("Using Battery");
        tracker.EnableCell();
        tracker.batteryLevel = 100f;
        tracker.InsertBattery();
        Destroy(this.gameObject);
    }
}
