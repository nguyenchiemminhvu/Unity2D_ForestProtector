using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CanvasButtonUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressing;
    public Player player;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Start()
    {
        isPressing = false;
    }

    void Update()
    {
        if (isPressing)
        {
            if (player.interaction)
            {
                if (player.canClimb)
                {
                    if (!player.isClimbing)
                    {
                        player.body.isKinematic = true;
                        player.isClimbing = true;
                    }
                    else
                    {
                        player.transform.Translate(new Vector3(0.0f, Time.deltaTime * Player.CLIMBING_SPEED, 0.0f));
                    }
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressing = false;
    }
}
