using UnityEngine;
using System.Collections;

public class Turtle : MonoBehaviour {

    enum TurtleStatus
    {
        TURTLE_SLEEPING = 0,
        TURTLE_WAKING_UP = 1,
        TURTLE_SWIMMING = 2
    };

    enum TurtleDirection
    {
        LEFT = -1,
        RIGHT = 1
    };

    public Player player;
    private Animator animator;
    [Range(-1, 1)] public int direction;
    public float speed;
    [HideInInspector] public bool isArrived;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        isArrived = false;
        direction = direction >= 0 ? (int)TurtleDirection.RIGHT : (int)TurtleDirection.LEFT;
        flip();
        speed = 1.0f;
        animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerCheckBottom")
        {
            if (this.isSleeping())
            {
                if(player.mapItems.numberOfTurtleFood > 0)    
                {
                    player.mapItems.numberOfTurtleFood--;
                    StartCoroutine(MainScene.instance.turtleCarryPlayer(this));
                }
            }
        }

        if (other.gameObject.tag == "supporter_boundary")
        {
            isArrived = true;
        }
    }

    public int getTurtleState()
    {
        return animator.GetInteger("turtle_state");
    }

    public void sleep()
    {
        animator.SetInteger("turtle_state", (int)TurtleStatus.TURTLE_SLEEPING);
    }

    public bool isSleeping()
    {
        return animator.GetInteger("turtle_state") == (int)TurtleStatus.TURTLE_SLEEPING;
    }

    public void wakeUp()
    {
        animator.SetInteger("turtle_state", (int) TurtleStatus.TURTLE_WAKING_UP);
    }

    public bool isWakingUp()
    {
        return animator.GetInteger("turtle_state") == (int)TurtleStatus.TURTLE_WAKING_UP;
    }

    public void swimming()
    {
        animator.SetInteger("turtle_state", (int) TurtleStatus.TURTLE_SWIMMING);
    }

    public bool isSwimming()
    {
        return animator.GetInteger("turtlte_state") == (int)TurtleStatus.TURTLE_SWIMMING;
    }

    public void reverseDirection()
    {
        direction *= -1;
    }

    public void flip()
    {
        gameObject.transform.localScale = new Vector3(direction, 1.0f, 1.0f);
    }
}
