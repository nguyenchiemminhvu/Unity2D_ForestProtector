using UnityEngine;
using System.Collections;

public class BigLeftBanana : MonoBehaviour {

    private const float rotationScale = 1.0f;
    private const float speed = -10.0f;

    private float previousPosX;
    private float currentPosX;
    private float perimeter;

    void Start()
    {
        StartCoroutine(destroyBanana());

        // rotate the banana
        previousPosX = currentPosX = gameObject.transform.position.x;
        perimeter = (gameObject.GetComponent<CircleCollider2D>().radius * 2 * Mathf.PI) * rotationScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Score.addScore(20);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "boss")
        {
            other.GetComponent<Boss1>().Hurt();
        }
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(transform.position.x + Time.deltaTime * speed, transform.position.y, transform.position.z);

        previousPosX = currentPosX;
        currentPosX = gameObject.transform.position.x;

        //Rotate banana by time
        gameObject.transform.Rotate(new Vector3(0, 0, 1), gameObject.transform.rotation.z - 360 * (currentPosX - previousPosX) / perimeter);
    }

    IEnumerator destroyBanana()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
