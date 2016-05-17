using UnityEngine;
using System.Collections;

public class Tempest : MonoBehaviour {

    private Animator animator;

	void Start () 
    {
        createTempestTrigger();
	}
	
	void Update () 
    {
	    
	}

    public void createTempestTrigger()
    {
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.tag = "explosion";
        collider.isTrigger = true;

        collider.offset = new Vector2(0.0f, 0.0f);
        collider.size = new Vector2(5.4f, 1.0f);
    }

    public void destroyThisShit()
    {
        Destroy(gameObject);
    }
}
