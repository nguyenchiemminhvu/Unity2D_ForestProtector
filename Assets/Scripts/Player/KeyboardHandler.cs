using UnityEngine;
using System.Collections;

public class KeyboardHandler : MonoBehaviour {

    private Player player;
    private bool[] keys;

	void Start () {
        
        #if UNITY_ANDROID
            Destroy(gameObject);    
        #endif

        player = GameObject.Find("Player").GetComponent<Player>();
        keys = new bool[1024];
	}
	
	void Update () {
        receiveKeyDownInput();
        detectKeyUpInput();
        handleKeyEvents();
    }

    void receiveKeyDownInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            keys[(int)KeyCode.D] = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            keys[(int)KeyCode.A] = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            keys[(int)KeyCode.W] = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            keys[(int)KeyCode.S] = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            keys[(int)KeyCode.J] = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            keys[(int)KeyCode.K] = true;
        }
    }

    void detectKeyUpInput()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            keys[(int)KeyCode.D] = false;
            releaseKeyovement();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            keys[(int)KeyCode.A] = false;
            releaseKeyovement();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            keys[(int)KeyCode.W] = false;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            keys[(int)KeyCode.S] = false;
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            keys[(int)KeyCode.J] = false;
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            keys[(int)KeyCode.K] = false;
        }
    }

    void handleKeyEvents()
    {
        if(keys[(int)KeyCode.J])
        {
            handleKeyFire();
            keys[(int)KeyCode.J] = false;
        }
        if(keys[(int)KeyCode.K])
        {
            handleKeyJump();
            keys[(int)KeyCode.K] = false;
        }

        if(keys[(int)KeyCode.A])
        {
            handleKeyLeft();
        }
        if(keys[(int)KeyCode.D])
        {
            handleKeyRight();
        }
        if(keys[(int)KeyCode.W])
        {
            handleKeyUp();
        }
        if(keys[(int)KeyCode.S])
        {
            handleKeyDown();
        }
    }

    void handleKeyFire()
    {
        if (player.interaction)
            player.fireActivated();
    }

    void handleKeyJump()
    {
        if (player.interaction)
        {
            if (player.isClimbing)
            {
                player.jumpOnLadder();
            }
            else
            {
                if (!player.isJumping)
                {
                    player.jump();
                    player.canDoubleJump = true;
                }
                else
                {
                    if (player.doubleJumpOpened && player.canDoubleJump && player.fallingVelocity > -5.0f)
                    {
                        player.canDoubleJump = false;
                        player.jump();
                    }
                }
            }
        }
    }

    void handleKeyMovement(string name)
    {
        switch (name)
        {
            case "ButtonLeft":
                handleKeyLeft();
                break;

            case "ButtonRight":
                handleKeyRight();
                break;

            case "ButtonUp":
                handleKeyUp();
                break;

            case "ButtonDown":
                handleKeyDown();
                break;
        }
    }

    void handleKeyLeft()
    {
        if (player.interaction)
        {
            player.playerHorizontalMovement -= 0.05f;
            player.playerHorizontalMovement = Mathf.Clamp(player.playerHorizontalMovement, -1.0f, 1.0f);
            player.setDirection(Player.PlayerDirection.LEFT);
            player.flip();
        }
    }

    void handleKeyRight()
    {
        if (player.interaction)
        {
            player.playerHorizontalMovement += 0.05f;
            player.playerHorizontalMovement = Mathf.Clamp(player.playerHorizontalMovement, -1.0f, 1.0f);
            player.setDirection(Player.PlayerDirection.RIGHT);
            player.flip();
        }
    }

    void handleKeyUp()
    {
        if (player.interaction)
        {
            if (player.stayingOnObject.tag == "climb_trigger")
            {
                if (player.climbOpened && player.canClimb)
                {
                    if (!player.isClimbing)
                    {
                        player.body.gravityScale = 0.0f;
                        player.body.velocity = new Vector2(0, 0);
                        player.isClimbing = true;
                        player.isJumping = false;
                        player.climbing();
                    }
                    else
                    {
                        player.transform.Translate(new Vector3(0.0f, Time.deltaTime * Player.CLIMBING_SPEED, 0.0f));
                    }
                }
            }
            else if (player.stayingOnObject.tag == "entrance_door")
            {
                StartCoroutine(teleportEffect());
            }
        }
    }

    void handleKeyDown()
    {
        if (player.interaction)
        {
            if (player.body.gravityScale == 0.0f)
            {
                player.isClimbing = true;
                player.transform.Translate(new Vector3(0.0f, Time.deltaTime * Player.CLIMBING_SPEED * -1, 0.0f));
            }
        }
    }

    void releaseKeyovement()
    {
        player.playerHorizontalMovement = 0.0f;
    }

    IEnumerator teleportEffect()
    {
        player.interaction = false;
        player.isPaining = true;

        EntranceDoor door = player.stayingOnObject.GetComponent<EntranceDoor>();
        while (player.spriteRenderer.material.color.a > 0.0f)
        {
            player.spriteRenderer.material.color = new Color(
                                                        player.spriteRenderer.material.color.r,
                                                        player.spriteRenderer.material.color.g,
                                                        player.spriteRenderer.material.color.b,
                                                        player.spriteRenderer.material.color.a - 0.05f
                                                            );
            yield return new WaitForEndOfFrame();
        }

        MainScene.instance.moveObjectToPosition(player.gameObject, door.nextDoor.transform.position);
        player.spriteRenderer.material.color = new Color(
                                                        player.spriteRenderer.material.color.r,
                                                        player.spriteRenderer.material.color.g,
                                                        player.spriteRenderer.material.color.b,
                                                        1.0f
                                                        );

        player.isPaining = false;
        player.interaction = true;
    }
}
