using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    public int minFallSpeed = 3;
    public int maxFallSpeed = 5;

    public int minRotationSpeed = 1;
    public int maxRotationSpeed = 4;

    public int minLifeSpan = 2;
    public int maxLifeSpan = 8;

    int fallSpeed;
    int rotationSpeed;
    float lifeSpan;
    bool rotationDirection;

    // Start is called before the first frame update
    void Start()
    {
        fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        lifeSpan = Random.Range(minLifeSpan, maxLifeSpan);

        rotationDirection = Random.value < 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        int direction = rotationDirection ? 1 : -1;  

        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * fallSpeed, transform.position.z);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + Time.deltaTime * rotationSpeed * direction * 0.5f,
            transform.rotation.eulerAngles.z + Time.deltaTime * rotationSpeed * direction);

        lifeSpan -= Time.deltaTime;

        if (lifeSpan <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
