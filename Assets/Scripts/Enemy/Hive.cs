using UnityEngine;
using System.Collections;

public class Hive : MonoBehaviour {

    public GameObject bee;

    private bool isShooted;
    private float hiveAngle;
    private float[] angles = { 20, -20, -20, 20 };

    void Start()
    {
        isShooted = false;
        hiveAngle = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player_banana" || other.gameObject.tag == "player_big_banana" || other.gameObject.tag == "Player")
        {
            if (!isShooted)
            {
                StartCoroutine(oscillate());
                Instantiate(bee, gameObject.transform.position, gameObject.transform.rotation);
            }
        }
    }

    IEnumerator oscillate()
    {
        isShooted = true;

        for (int i = 0; i < 20; i++)
        {
            hiveAngle += angles[i % 4];
            gameObject.transform.Rotate(new Vector3(0, 0, 1), hiveAngle);
            yield return new WaitForEndOfFrame();
        }

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isShooted = false;
    }
}
