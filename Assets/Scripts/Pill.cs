using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : Item
{
    public PlayerMovement player;
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
        itemSpawner.numPills -= 1;

        player.sanity = 100f;
        player.TakePill();
        player.numPillsConsumed++;
        Destroy(this.gameObject);
    }
}
