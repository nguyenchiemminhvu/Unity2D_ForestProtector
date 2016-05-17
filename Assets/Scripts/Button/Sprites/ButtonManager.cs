using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public Camera mainCamera;
    [HideInInspector] public Player player;
    [HideInInspector] public bool isPressing;

    public GameObject ButtonFire;
    public GameObject ButtonJump;
    public GameObject ButtonUp;
    public GameObject ButtonDown;
    public GameObject ButtonLeft;
    public GameObject ButtonRight;

    void Awake()
    {
    }

	void Start () 
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        isPressing = false;

        // get all Button
        ButtonFire  = GameObject.Find("ButtonFire");
        ButtonJump  = GameObject.Find("ButtonJump");
        ButtonUp    = GameObject.Find("ButtonUp");
        ButtonDown  = GameObject.Find("ButtonDown");
        ButtonLeft  = GameObject.Find("ButtonLeft");
        ButtonRight = GameObject.Find("ButtonRight");
	}
	
	void Update () 
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.touches[i].position), Vector2.zero);

                if (Input.touches[i].phase == TouchPhase.Began)
                {
                    switch (hit.collider.name)
                    {
                        case "ButtonJump":
                            handleButtonJump();
                            StartCoroutine(beganAnimation("ButtonJump"));
                            break;

                        case "ButtonFire":
                            handleButtonFire();
                            StartCoroutine(beganAnimation("ButtonFire"));
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
            }
        }
        
        /*
        ////////////////////////////////////////////////////////////
        // I use this code for check the Event on Unity editor
        // It will be deleted after figure out the climbing problem
        if (isPressing)
        {
            if (player.interaction)
            {
                if (gameObject.name == "ButtonFire")
                    handleButtonFire();

                handleButtonMovement(gameObject.name);

                stationaryAnimation(gameObject.name);
            }
        }
        */
	}

    /*
    // This code need to delete after figure out the climbing problem
    void OnMouseDown()
    {
        isPressing = true;

        if (gameObject.name == "ButtonJump")
        {
            handleButtonJump();
            StartCoroutine(beganAnimation("ButtonJump"));
        }
    }

    void OnMouseUp()
    {
        isPressing = false;

        if (gameObject.name == "ButtonLeft" || gameObject.name == "ButtonRight")
            releaseButtonMovement();

        releaseAnimation(gameObject.name);
    }
    */
    
    void handleButtonFire()
    {
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
                    if (player.doubleJumpOpened && player.canDoubleJump)
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
        player.playerHorizontalMovement -= 0.05f;
        player.playerHorizontalMovement = Mathf.Clamp(player.playerHorizontalMovement, -1.0f, 1.0f);
        player.setDirection(Player.PlayerDirection.LEFT);
        player.flip();
    }

    void handleButtonRight()
    {
        player.playerHorizontalMovement += 0.05f;
        player.playerHorizontalMovement = Mathf.Clamp(player.playerHorizontalMovement, -1.0f, 1.0f);
        player.setDirection(Player.PlayerDirection.RIGHT);
        player.flip();
    }

    void handleButtonUp()
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
            isPressing = false;
            StartCoroutine(teleportEffect());
        }
    }

    void handleButtonDown()
    {
        if (player.body.gravityScale == 0.0f)
        {
            player.isClimbing = true;
            player.transform.Translate(new Vector3(0.0f, Time.deltaTime * Player.CLIMBING_SPEED * -1, 0.0f));
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
    /*------------------------------------------------------------------------*/
}
