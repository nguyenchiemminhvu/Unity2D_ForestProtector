using UnityEngine;
using System.Collections;

public class LandMine : MonoBehaviour {

    private Explosion explosion;

	void Start () {
        explosion = gameObject.GetComponentInChildren<Explosion>();
        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), GameObject.Find("Player").GetComponentInChildren<BoxCollider2D>());
	}
	
	void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "explosion")
        {
            exploding();
        }
	}

    public void exploding()
    {
        explosion.activeExplosion(0.5f);
    }
}
