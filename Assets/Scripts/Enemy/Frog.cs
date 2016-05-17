using UnityEngine;
using System.Collections;

public class Frog : MonoBehaviour {

    public const float yIntensity = 5.0f;
    public const float xIntensity = 1.0f;
    public const float REST_DURATION = 3.0f;
    public const int FROG_DAMAGE = 3;
    public const float INNER_ACTIVATED_RANGE = 7.0f;
    public const float OUTER_ACTIVATED_RANGE = 14.0f;

    enum JumpDirection
    {
        LEFT = -1,
        RIGHT = 1
    };

    private Player player;
    private Rigidbody2D body;
    private JumpDirection direction;
    public bool isActivating;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

	void Start () 
    {
        isActivating = false;
        body = gameObject.GetComponent<Rigidbody2D>();
        lookAtPlayer();
	}

    void Update()
    {
        if (isActivating)
        {
            if (playerDistance() >= OUTER_ACTIVATED_RANGE)
            {
                deactivated();
            }
        }
        else
        {
            if(playerDistance() <= INNER_ACTIVATED_RANGE)
            {
                activated();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isActivating)
                player.decreaseHealth(Frog.FROG_DAMAGE);
            else
                activated();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isActivating)
                player.decreaseHealth(Frog.FROG_DAMAGE);
        }
    }

    public void activated()
    {
        InvokeRepeating("followingPlayer", 0.0f, REST_DURATION);
        isActivating = true;
    }

    public void deactivated()
    {
        CancelInvoke("followingPlayer");
        isActivating = false;
    }

    void followingPlayer()
    {
        lookAtPlayer();
        jump();
    }

    float playerDistance()
    {
        return (transform.position - player.transform.position).magnitude;
    }

    void jump()
    {
        body.AddForce(new Vector2(xIntensity * (int)direction, yIntensity), ForceMode2D.Impulse);
    }

    void lookAtPlayer()
    {
        if (gameObject.transform.position.x < player.transform.position.x)
        {
            direction = JumpDirection.RIGHT;
        }
        else
        {
            direction = JumpDirection.LEFT;
        }

        gameObject.transform.localScale = new Vector3(-(int)direction, 1, 1);
    }
}
