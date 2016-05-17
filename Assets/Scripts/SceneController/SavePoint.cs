using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour {

    public Player player;

    public Sprite non_activated;
    public Sprite activated;

    private SpriteRenderer renderer;
    private bool isActivated;

	void Start () 
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<Player>();
        isActivated = false;
	}
	
	void Update () 
    {
        if (isActivated)
        {
            renderer.sprite = activated;
        }
        else
        {
            renderer.sprite = non_activated;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!isActivated)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("save_point"))
                {
                    obj.GetComponent<SavePoint>().isActivated = false;
                }

                player.originalPosition = gameObject.transform.position;
                isActivated = true;
            }
        }
    }
}
