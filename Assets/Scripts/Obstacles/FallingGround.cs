using UnityEngine;
using System.Collections;

public class FallingGround : MonoBehaviour {

    private Rigidbody2D body;

	void Start () {
        body = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
        body.freezeRotation = true;
        body.gravityScale = 0.0f;
	}
	
	void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.name == "PlayerCheckBottom")
        {
            body.gravityScale = 1.0f;
        }
	}
}
