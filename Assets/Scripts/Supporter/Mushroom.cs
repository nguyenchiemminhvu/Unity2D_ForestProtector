using UnityEngine;
using System.Collections;

public class Mushroom : MonoBehaviour {

    [HideInInspector] public Animator animator;
    [HideInInspector] public Player player;

    public const float MUSHROOM_RELOAD_DURATION = 1.0f;
    private bool isReady;

    public float jumpHeight;

	// Use this for initialization
	void Start () {
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();

        isReady = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerCheckBottom")
        {
            player.standing();
            if (isReady)
            {
                elasticState();
                StartCoroutine(mushroomReloading());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerCheckBottom")
        {
            normalizeState();
        }
    }

    public void normalizeState()
    {
        animator.SetInteger("mushroom_state", 0);
    }

    public void elasticState()
    {
        animator.SetInteger("mushroom_state", 1);
    }

    public void forcePlayer()
    {
        normalizeState();
        player.jumpOnMushroom(jumpHeight);
    }

    IEnumerator mushroomReloading()
    {
        isReady = false;
        yield return new WaitForSeconds(MUSHROOM_RELOAD_DURATION);
        isReady = true;
    }
}
