using UnityEngine;
using System.Collections;

public class EntranceDoor : MonoBehaviour {

    public EntranceDoor nextDoor;
    public SpriteRenderer spriteRenderer;

    public bool closed;

    public Sprite doorOpened;
    public Sprite doorClosed;

	void Start () 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update () {

        if (closed)
        {
            spriteRenderer.sprite = doorClosed;
        }
        else
        {
            spriteRenderer.sprite = doorOpened;
        }
	}

    void openTheDoor()
    {
        closed = false;
    }

    void closeTheDoor()
    {
        closed = true;
    }
}
