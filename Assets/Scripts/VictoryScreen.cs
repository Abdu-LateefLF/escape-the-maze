using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public RectTransform confettiSpawner;
    public GameObject[] confettiPrefabs;
    public Text modeText;

    public float minTimeToSpawn = 1.4f;
    public int minBatch = 3;
    public int maxBatch = 9;

    float timeToSpawn = 0;
    // Start is called before the first frame update
    void Start()
    {
        switch (MainMenu.mode)
        {
            case 0:
                modeText.text = "EASY MODE";
                modeText.color = Color.green;
                break;
            case 1:
                modeText.text = "NORMAL MODE";
                modeText.color = Color.cyan;
                break;
            case 2:
                modeText.text = "HARD MODE";
                modeText.color = Color.yellow;
                break;
            case 3:
                modeText.text = "VERY HARD MODE";
                modeText.color = Color.magenta;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeToSpawn += Time.deltaTime;

        if (timeToSpawn >= minTimeToSpawn)
        {
            int batchNum = Random.Range(minBatch, maxBatch);

            for (int i = 0; i < batchNum; i++)
            {
                SpawnConfetti();
            }

            timeToSpawn = 0;    
        }
    }

    void SpawnConfetti()
    {
        int rand = Random.Range(0, confettiPrefabs.Length - 1);

        GameObject confetti = Instantiate(confettiPrefabs[rand]);
        confetti.transform.SetParent(confettiSpawner);

        float offsetX = Random.Range((Screen.width / 2 - 60) * -1, Screen.width / 2 - 60);
        confetti.transform.localPosition = new Vector3(offsetX, 0f, 0f);
    }
}
