using UnityEngine;
using System.Collections;

public class LeftBanana : MonoBehaviour {

    private const float rotationScale = 3.0f;

    private float previousPosX;
    private float currentPosX;
    private float perimeter;

    void Start()
    {
        previousPosX = currentPosX = gameObject.transform.position.x;
        perimeter = (gameObject.GetComponent<CircleCollider2D>().radius * 2 * Mathf.PI) * rotationScale;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-Player.THROW_INTENSITY, 1), ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "ground")
        {
            Destroy(gameObject);
        }

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
        previousPosX = currentPosX;
        currentPosX = gameObject.transform.position.x;
        gameObject.transform.Rotate(new Vector3(0, 0, 1), gameObject.transform.rotation.z - 360 * (currentPosX - previousPosX) / perimeter);
    }
}
