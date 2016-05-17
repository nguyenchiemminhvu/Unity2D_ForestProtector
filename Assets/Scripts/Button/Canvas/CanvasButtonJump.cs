using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CanvasButtonJump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
                if (player.canDoubleJump)
                    player.jump();
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
