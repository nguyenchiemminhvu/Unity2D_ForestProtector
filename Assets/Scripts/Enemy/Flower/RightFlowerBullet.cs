using UnityEngine;
using System.Collections;

public class RightFlowerBullet : MonoBehaviour {

    private const float LAUNCH_INTENSITY = 10.0f;
    public const int DAMAGE_TO_PLAYER = 2;

    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(LAUNCH_INTENSITY, 2), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "EnemyFlower")
            return;

        if (other.gameObject.name == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.decreaseHealth(DAMAGE_TO_PLAYER);
        }
        if (other.gameObject.tag == "ground") Destroy(gameObject);
    }
}
