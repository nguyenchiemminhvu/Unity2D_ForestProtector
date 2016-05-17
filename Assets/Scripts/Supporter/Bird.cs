using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

    enum BirdStatus
    {
        STANDING = 0,
        FLYING = 1
    };

    enum BirdDirection
    {
        LEFT = -1,
        RIGHT = 1
    }

    public Animator animator;
    [Range (-1, 1)] public int direction;
    public float speed;
    [HideInInspector] public bool isArrived;

    void Start()
    {
        direction = direction >= 0 ? (int) BirdDirection.RIGHT : (int) BirdDirection.LEFT;
        flip();
        speed = 2.0f;
        isArrived = false;
        animator = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "supporter_boundary")
        {
            isArrived = true;
        }

        if (other.gameObject.name == "PlayerCheckBottom")
        {
            if (this.isStangding())
            {
                Player player = GameObject.Find("Player").GetComponent<Player>();
               if (player.mapItems.numberOfBirdFood > 0)
                {
                    player.mapItems.numberOfBirdFood--;
                    StartCoroutine(MainScene.instance.birdCarryPlayer(this));
                }
            }
        }
    }

    public int getBirdState()
    {
        return animator.GetInteger("bird_state");
    }

    public void standing()
    {
        animator.SetInteger("bird_state", (int)BirdStatus.STANDING);
    }

    public bool isStangding()
    {
        return this.getBirdState() == (int) BirdStatus.STANDING;
    }

    public void flying()
    {
        animator.SetInteger("bird_state", (int)BirdStatus.FLYING);
    }

    public bool isFlying()
    {
        return this.getBirdState() == (int) BirdStatus.FLYING;
    }

    public void reverseDirection()
    {
        direction *= -1;
    }

    public void flip()
    {
        gameObject.transform.localScale = new Vector3(direction*2, 2, 1);
    }
}
