using UnityEngine;
using System.Collections;

public class RockTrigger : MonoBehaviour {
	
	void OnTriggerEnter2D (Collider2D other) {
	    if(other.gameObject.tag == "Player")
        {
            Rigidbody2D rockBody = gameObject.GetComponentInParent<Rigidbody2D>();
            rockBody.gravityScale = 1.0f;
            Destroy(gameObject);
        }
	}
}
