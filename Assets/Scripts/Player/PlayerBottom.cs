using UnityEngine;
using System.Collections;

public class PlayerBottom : MonoBehaviour
{

    private const float HALF_OF_JUMP_INTENSITY = Player.JUMP_INTENSITY / 2;
    private const int MULTIPLE_FALLING_VELOCITY_PERMITED = 2;

    [HideInInspector]
    public Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (checkEnterInto(other))
        {
            int multiple = (int)Mathf.Abs(player.fallingVelocity / HALF_OF_JUMP_INTENSITY);
            player.decreaseHealth(multiple - MULTIPLE_FALLING_VELOCITY_PERMITED);
        }
    }

    bool checkEnterInto(Collider2D other)
    {
        if (other.gameObject.tag == "ground"
                 || other.gameObject.tag == "supporter"
                 || other.gameObject.tag == "obstacles")
        {
            return true;
        }

        return false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (checkStayOn(other))
        {
            player.isJumping = false;

            //standing or running
            if (player.playerHorizontalMovement == 0)
                player.standing();
            else
                player.running();
        }
    }

    private bool checkStayOn(Collider2D other)
    {
        if (
            other.gameObject.tag == "ground"
         || other.gameObject.tag == "supporter"
         || other.gameObject.tag == "obstacles"
            )
        {
            return true;
        }

        return false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (checkExitFrom(other))
        {
            player.isJumping = true;
            player.jumping();
        }

    }

    private bool checkExitFrom(Collider2D other)
    {
        if (
            other.gameObject.tag == "ground"
         || other.gameObject.tag == "supporter"
         || other.gameObject.tag == "obstacles"
            )
        {
            return true;
        }

        return false;
    }
}
