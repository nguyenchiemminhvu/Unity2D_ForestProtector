using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CanvasButtonRight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
                player.playerHorizontalMovement += 0.05f;
                player.playerHorizontalMovement = Mathf.Clamp(player.playerHorizontalMovement, -1.0f, 1.0f);
                
                player.setDirection(Player.PlayerDirection.RIGHT);
                player.flip();
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
        player.playerHorizontalMovement = 0.0f;
    }
}
