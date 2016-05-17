using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {

    public const float START_DURATION = 1.0f;
    public const float REPEAT_DURATION = 2.0f;

    public GameObject leftBullet;
    public GameObject rightBullet;
    public Transform shootTransform;

    [Range(-1, 1)]
    public int direction;

    void Start()
    {
        if (direction == 0) direction = 1;
       // gameObject.transform.localScale = new Vector3(direction, 1, 1);
        InvokeRepeating("shoot", START_DURATION, REPEAT_DURATION);
    }

    void Update()
    {

    }

    void shoot()
    {
        if (direction == -1)
        {
            Instantiate(leftBullet, shootTransform.position, shootTransform.rotation);
        }
        else if (direction == 1)
        {
            Instantiate(rightBullet, shootTransform.position, shootTransform.rotation);
        }
    }
}
