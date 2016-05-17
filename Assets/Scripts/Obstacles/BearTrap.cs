using UnityEngine;
using System.Collections;

public class BearTrap : MonoBehaviour {

    private const int BEAR_TRAP_DAMAGE = 2;

    enum BearTrapStatus
    {
        INACTIVATED = 0,
        ACTIVATED = 1
    };

    private Player player;
    private Animator animator;

    public bool isActivated;

    void Start()
    {
        isActivated = false;
        player = GameObject.Find("Player").GetComponent<Player>();
        animator = gameObject.GetComponent<Animator>();
        
        Physics2D.IgnoreCollision(gameObject.GetComponent<CircleCollider2D>(), player.GetComponentInChildren<BoxCollider2D>());
        GameObject bossAxe = GameObject.Find("MotherFucker");
        if (bossAxe)
        {
            Physics2D.IgnoreCollision(bossAxe.GetComponentInChildren<BoxCollider2D>(), gameObject.GetComponent<CircleCollider2D>());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isActivated)
            {
                activated();
            }
        }
    }

    public void activated()
    {
        isActivated = true;
        animator.SetInteger("state", (int)BearTrapStatus.ACTIVATED);
    }

    public void damageToPlayer()
    {
        player.decreaseHealth(BEAR_TRAP_DAMAGE);
        player.StartCoroutine(player.beTrapped());
    }

    public void destroyThisShit()
    {
        Destroy(gameObject);
    }
}
