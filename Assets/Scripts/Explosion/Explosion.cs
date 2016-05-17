using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    enum ExplosionStatus
    {
        NOTHING = 0,
        EXPLODING = 1
    };

    public const int EXPLOSION_DAMAGE = 5;

    private GameObject parent;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip explosionSound;

	void Start () {
        animator = gameObject.GetComponent<Animator>();
        parent = gameObject.GetComponentInParent<BoxCollider2D>().gameObject;
	}

    public void activeExplosion(float seconds)
    {
        Invoke("exploding", seconds);
    }

    public void destroyObject()
    {
        Destroy(parent);
    }

    public void nothingHappened()
    {
        animator.SetInteger("explosion_state", (int)ExplosionStatus.NOTHING);
    }

    public void exploding()
    {
        createExplosionTrigger();
        animator.SetInteger("explosion_state", (int)ExplosionStatus.EXPLODING);
    }

    public void createExplosionTrigger()
    {
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();

        collider.isTrigger = true;
        collider.tag = "explosion";
        collider.radius = 1.5f;
    }

    public void playExplosionSound()
    {
        if (explosionSound != null)
            audioSource.PlayOneShot(explosionSound);
    }
}
