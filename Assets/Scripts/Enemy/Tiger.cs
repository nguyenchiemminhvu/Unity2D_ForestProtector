using UnityEngine;
using System.Collections;

public class Tiger : MonoBehaviour {

    const int SPEED = 5;
    const int DAMAGE_TO_PLAYER = 5;
    bool isRightFacing;
    
	// Use this for initialization
	void Start () {
        isRightFacing = false;
       
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x + (isRightFacing? SPEED : -SPEED) * Time.deltaTime, transform.position.y, transform.position.z);
	}
    public void ChangeFacing()
    {
        isRightFacing = !isRightFacing;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.decreaseHealth(DAMAGE_TO_PLAYER);
            ChangeFacing();
        }
    }
}
