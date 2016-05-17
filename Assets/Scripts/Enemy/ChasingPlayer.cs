using UnityEngine;
using System.Collections;

public class ChasingPlayer : MonoBehaviour {

    public Transform target;
    public const float moveSpeed = 4;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forward = (target.position - gameObject.transform.position).normalized;
        transform.position += forward * moveSpeed * Time.deltaTime;
	}
}
