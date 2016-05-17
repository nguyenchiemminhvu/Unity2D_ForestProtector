using UnityEngine;
using System.Collections;

public class TigerFront : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "ground")
        GetComponentInParent<Tiger>().ChangeFacing();
       // transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
    }
}
