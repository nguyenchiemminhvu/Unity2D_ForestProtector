using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {

    enum SpiderDirection
    {
        LEFT = -1,
        RIGHT = 1
    };

    public int direction;
    public float speed;
    
    [HideInInspector]
    public bool isArrived;

    public int damageToPlayer()
    {
        return 1;
    }

	void Start () {
        isArrived = false;
        direction = (int)SpiderDirection.RIGHT;
        speed = 1.5f;

        StartCoroutine(MainScene.instance.spiderMoving(this));
	}
	
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy_boundary") 
        {
            isArrived = true;
            Invoke("moving", 0.5f);
        }
    }

    public void reverseDirection()
    {
        direction *= -1;
    }

    void moving()
    {
        StartCoroutine(MainScene.instance.spiderMoving(this));
    }

}
