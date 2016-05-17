using UnityEngine;
using System.Collections;

public class TestPlayerMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.D)) {
			transform.position = new Vector3 (transform.position.x + 5*Time.deltaTime, transform.position.y, transform.position.z);
		}
		else if (Input.GetKey(KeyCode.A)) {
			transform.position = new Vector3 (transform.position.x - 5*Time.deltaTime, transform.position.y, transform.position.z);
		}
		if(Input.GetKeyDown(KeyCode.W)){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5f), ForceMode2D.Impulse);
		}
	}

	public void MoveLeft(){
		transform.position = new Vector3 (transform.position.x - 5*Time.deltaTime, transform.position.y, transform.position.z);
	}

	public void MoveRight(){
		transform.position = new Vector3 (transform.position.x + 5*Time.deltaTime, transform.position.y, transform.position.z);
	}
	public void Jump(){
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5f), ForceMode2D.Impulse);
	}
}
