using UnityEngine;
using System.Collections;

public class TouchesController : MonoBehaviour
{
    public Camera GUICam;
    public Player player;

    public GameObject ButtonFire;
    public GameObject ButtonJump;
    public GameObject ButtonUp;
    public GameObject ButtonDown;
    public GameObject ButtonLeft;
    public GameObject ButtonRight;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        // get all Button
        ButtonFire  = GameObject.Find("ButtonFire");
        ButtonJump  = GameObject.Find("ButtonJump");
        ButtonUp    = GameObject.Find("ButtonUp");
        ButtonDown  = GameObject.Find("ButtonDown");
        ButtonLeft  = GameObject.Find("ButtonLeft");
        ButtonRight = GameObject.Find("ButtonRight");

        #if UNITY_STANDALONE_WIN
            ButtonFire.transform.position = new Vector3(0.0f, 0.0f, -1000.0f);
            ButtonJump.transform.position = new Vector3(0.0f, 0.0f, -1000.0f);
            ButtonLeft.transform.position = new Vector3(0.0f, 0.0f, -1000.0f);
            ButtonRight.transform.position = new Vector3(0.0f, 0.0f, -1000.0f);
            ButtonUp.transform.position = new Vector3(0.0f, 0.0f, -1000.0f);
            ButtonDown.transform.position = new Vector3(0.0f, 0.0f, -1000.0f);
        #endif
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                RaycastHit2D hit = Physics2D.Raycast(GUICam.ScreenToWorldPoint(Input.touches[i].position), Vector2.zero);

                if (Input.touches[i].phase == TouchPhase.Began)
                {
                    switch (hit.collider.name)
                    {
                        case "ButtonJump":
                            handleButtonJump();
                            break;

                        case "ButtonFire":
                            handleButtonFire();
                            break;
                        default:
                            break;
                    }
                }
                if (Input.touches[i].phase == TouchPhase.Ended)
                {
                    releaseAnimation(hit.collider.name);
                    switch (hit.collider.name)
                    {
                        case "ButtonLeft":
                        case "ButtonRight":
                            releaseButtonMovement();
                            break;
                    }
                }
                if (Input.touches[i].phase == TouchPhase.Stationary)
                {
                    stationaryAnimation(hit.collider.name);
                    switch (hit.collider.name)
                    {
                        case "ButtonLeft":
                            handleButtonLeft();
                            break;

                        case "ButtonRight":
                            handleButtonRight();
                            break;

                        case "ButtonUp":
                            handleButtonUp();
                            break;

                        case "ButtonDown":
                            handleButtonDown();
                            break;
                    }
                }
            }
        }
    }

    void handleButtonFire()
    {
        if (player.interaction)
            player.fireActivated();
    }

    void handleButtonJump()
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
                    if (player.doubleJumpOpened && player.canDoubleJump && player.fallingVelocity > - 5.0f)
                    {
                        player.canDoubleJump = false;
                        player.jump();
                    }
                }
            }
        }
    }

    void handleButtonMovement(string name)
    {
        switch (name)
        {
            case "ButtonLeft":
                handleButtonLeft();
                break;

            case "ButtonRight":
                handleButtonRight();
                break;

            case "ButtonUp":
                handleButtonUp();
                break;

            case "ButtonDown":
                handleButtonDown();
                break;
        }
    }

    void handleButtonLeft()
    {
        if (player.interaction)
        {
            player.playerHorizontalMovement -= 0.05f;
            player.playerHorizontalMovement = Mathf.Clamp(player.playerHorizontalMovement, -1.0f, 1.0f);
            player.setDirection(Player.PlayerDirection.LEFT);
            player.flip();
        }
    }

    void handleButtonRight()
    {
        if (player.interaction)
        {
            player.playerHorizontalMovement += 0.05f;
            player.playerHorizontalMovement = Mathf.Clamp(player.playerHorizontalMovement, -1.0f, 1.0f);
            player.setDirection(Player.PlayerDirection.RIGHT);
            player.flip();
        }
    }

    void handleButtonUp()
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

    void handleButtonDown()
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

    void releaseButtonMovement()
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

    /*------------------------------------------------------------------------*/
    // Button animation
    private IEnumerator beganAnimation(string buttonName)
    {
        switch (buttonName)
        {
            case "ButtonFire":
                ButtonFire.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                yield return new WaitForEndOfFrame();
                ButtonFire.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonJump":
                ButtonJump.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                yield return new WaitForEndOfFrame();
                ButtonJump.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonLeft":
                ButtonLeft.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                yield return new WaitForEndOfFrame();
                ButtonLeft.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                break;

            case "ButtonRight":
                ButtonRight.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                yield return new WaitForEndOfFrame();
                ButtonRight.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonUp":
                ButtonUp.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                yield return new WaitForEndOfFrame();
                ButtonUp.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonDown":
                ButtonDown.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                yield return new WaitForEndOfFrame();
                ButtonDown.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;
        }
    }

    private void stationaryAnimation(string buttonName)
    {
        switch (buttonName)
        {
            case "ButtonFire":
                ButtonFire.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                break;

            case "ButtonJump":
                ButtonJump.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                break;

            case "ButtonLeft":
                ButtonLeft.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                break;

            case "ButtonRight":
                ButtonRight.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                break;

            case "ButtonUp":
                ButtonUp.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                break;

            case "ButtonDown":
                ButtonDown.transform.localScale = new Vector3(3.7f, 3.7f, 1.0f);
                break;
        }
    }

    private void releaseAnimation(string buttonName)
    {
        switch (buttonName)
        {
            case "ButtonFire":
                ButtonFire.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonJump":
                ButtonJump.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonLeft":
                ButtonLeft.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonRight":
                ButtonRight.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonUp":
                ButtonUp.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;

            case "ButtonDown":
                ButtonDown.transform.localScale = new Vector3(3.5f, 3.5f, 1.0f);
                break;
        }
    }
}
