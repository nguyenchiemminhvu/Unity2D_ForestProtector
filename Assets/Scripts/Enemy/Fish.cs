using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

    public const int DAMAGE_TO_PLAYER = 1;
    public const float SLEEP_DURATION = 1.0f;

    private Rigidbody2D body;
    private Player target;

    [Range (5.0f, 15.0f)] public float jumpIntensity;

    void Awake()
    {
        jumpIntensity = 10.0f;
    }
	// Use this for initialization
	void Start () {
        body = gameObject.GetComponent<Rigidbody2D>();
        body.gravityScale = 0.0f;
        Invoke("resetGravity", Random.Range(0.0f, 2.0f));
	}

    void resetGravity()
    {
        body.gravityScale = 1.0f;
    }

    void Update()
    {
        if(body.velocity.y == 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else if (body.velocity.y > 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy_boundary")
        {
            StartCoroutine(active());
        }

        if (other.gameObject.tag == "Player")
        {
            target = GameObject.Find("Player").GetComponent<Player>();
            target.decreaseHealth(DAMAGE_TO_PLAYER);
            //Debug.Log(target.getHealth());
        }
    }

    public IEnumerator active()
    {
        body.velocity = new Vector2(0, 0);
        body.gravityScale = 0.0f;

        yield return new WaitForSeconds(SLEEP_DURATION);

        body.gravityScale = 1.0f;
        body.AddForce(new Vector2(0, jumpIntensity), ForceMode2D.Impulse);
    }
}
