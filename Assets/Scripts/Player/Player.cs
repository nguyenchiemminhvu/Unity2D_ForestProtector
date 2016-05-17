using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public const int ORIGINAL_LIFE = 3;
    public const int MAX_LIFE = 5;
    public const int MAX_HEALTH = 10;
    public const int MAX_ENERGY = 10;
    public const float PAIN_DURATION = 1.0f;
    public const float RELOAD_DURATION = 1.0f;
    public const float RESTRICT_DURATION = 3.0f;
    public const float JUMP_INTENSITY = 10.0f;
    public const float JUMP_ON_LADDER_INTENSITY = JUMP_INTENSITY;
    public const float THROW_INTENSITY = 15.0f;
    public const float ORIGINAL_SPEED = 6.0f;
    public const float CLIMBING_SPEED = 2.0f;
    public const float CAMERA_DEPTH_POSITION = -10.0f;
    const float DEFAULT_GRAVITY = 2f;

    public const int LEVEL_OPEN_DOUBLEJUMP = 0;
    public const int LEVEL_OPEN_SHOOT = 0;
    public const int LEVEL_OPEN_CLIMB = 0;

    public Text keyDisplay;
    
    public enum PlayerStatus
    {
        PLAYER_STANDING = 0,
        PLAYER_RUNNING = 1,
        PLAYER_JUMPING = 2,
        PLAYER_THROWING = 3,
        PLAYER_CLIMBING = 4,
        PLAYER_DYING = 5
    };
    private PlayerStatus previousStatus;

    public enum PlayerDirection
    {
        LEFT = -1,
        RIGHT = 1
    }

    /*-------------------------------*/
    [HideInInspector]
    public bool interaction;
    /*-------------------------------*/

    /*-------------------------------*/
    // Player parts
    public GameObject camera;
    public GameObject buttonFire;
    /*-------------------------------*/

    /*-------------------------------*/
    // Player components
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D body;
    public Animator animator;
    /*-------------------------------*/

    /*-------------------------------*/
    // Player life
    private int numberOfLife;
    private Health health;

    [HideInInspector] 
    public bool isPaining;
    /*-------------------------------*/

    /*-------------------------------*/
    // Player attributes
    [HideInInspector]
    [Range(0, 10)]
    public float speed;

    [HideInInspector]
    public float playerHorizontalMovement;

    [HideInInspector]
    [Range(0, 100)]
    public int energy;

    [HideInInspector] 
    public float fallingVelocity;

    [Range(-1, 1)]
    private int playerDirection;
    /*-------------------------------*/

    /*-------------------------------*/
    // Player basic skill
    [HideInInspector]
    public bool doubleJumpOpened;

    [HideInInspector]
    public bool canDoubleJump;

    [HideInInspector]
    public bool isJumping;

    [HideInInspector]
    public bool climbOpened;

    [HideInInspector]
    public bool canClimb;

    [HideInInspector]
    public bool isClimbing;

    [HideInInspector]
    public bool shootOpened;

    [HideInInspector]
    public bool canShoot;

    private bool isReloading;

    public GameObject leftBanana;
    public GameObject rightBanana;
    public GameObject leftBigBanana;
    public GameObject rightBigBanana;
    public Transform bananaTransform;

    // damage of skills
    public const int BANANA_DAMAGE = 1;
    public const int BIG_BANANA_DAMAGE = 10;
    /*-------------------------------*/

    /*-------------------------------*/
    // Player - Map (Collaborative)
    [HideInInspector] 
    public int currentLevel;
    
    [HideInInspector] 
    public Vector3 originalPosition;

    [HideInInspector]
    public MapItems mapItems;

    [HideInInspector]
    public GameObject stayingOnObject;
    /*-------------------------------*/

    /*-------------------------------*/
    // Audio
    private AudioSource audioSource;

    public AudioClip throwSound;
    public AudioClip powerThrowSound;
    public AudioClip jumpSound;
    public AudioClip painSound;
    public AudioClip deadSound;
    public AudioClip teleportSound;
    public AudioClip fruitSound;
    public AudioClip finishedLevelSound;
    /*-------------------------------*/

    void Awake()
    {
        initAudio();
    }

    void Start()
    {
        initPlayerLife();
        initPlayerAttributes();
        initPlayerParts();
        initPlayerComponents();
        initPlayerStatus();
        initMapCollarborative();
        initPlayerBasicSkills(SceneManager.GetActiveScene().buildIndex);

        intergrateInteractive();
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("climb_trigger"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponentInChildren<BoxCollider2D>(), obj.GetComponent<BoxCollider2D>());
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("frog"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponentInChildren<BoxCollider2D>(), obj.GetComponent<BoxCollider2D>());
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("boss_axe"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponentInChildren<BoxCollider2D>(), obj.GetComponent<BoxCollider2D>());
        }
    }

    void Update()
    {
        fallingVelocity = body.velocity.y;
        Debug.Log(fallingVelocity);
        #if UNITY_STANDALONE

        //handle keyboard event in here

        #endif

        horizontalMoving(Time.deltaTime);
        shiftCamera();
    }
    /*-------------------------------*/

    /*-------------------------------*/
    void OnCollisionEnter2D(Collision2D other)
    {

    }

    void OnCollisionStay2D(Collision2D other)
    {

    }

    void OnCollisionExit2D(Collision2D other)
    {

    }
    /*-------------------------------*/

    /*-------------------------------*/
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "explosion")
        {
            decreaseHealth(Explosion.EXPLOSION_DAMAGE);
        }

        if (other.gameObject.name == "NextLevelGate" && mapItems.keys == 0)
        {
            if (PlayerPrefs.HasKey("curScore"))
            {
                PlayerPrefs.SetInt("curScore", Score.score);
                PlayerPrefs.Save();
            }
            Debug.Log(currentLevel + " - " + mapItems.keys);
            SceneManager.LoadScene("Level-" + (currentLevel + 1));
        }

        if (other.gameObject.tag == "key")
        {
            
            mapItems.keys--;
            keyDisplay.text = "Remain keys: " + mapItems.keys;
            other.gameObject.GetComponent<Keys>().OnGetKey();
        }

        if(other.gameObject.tag == "dead_zone")
        {
            hasDeath();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "climb_trigger")
        {
            stayingOnObject = other.gameObject;
            if (!isClimbing)
            {
                canClimb = true;
            }
        }

        if (other.gameObject.tag == "entrance_door")
        {
            EntranceDoor door = other.gameObject.GetComponent<EntranceDoor>();
            if(!door.closed)
            {
                stayingOnObject = door.gameObject;
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "climb_trigger")
        {
            canClimb = false;
            body.gravityScale = DEFAULT_GRAVITY;
            isClimbing = false;
            isJumping = true;
            resetStayingOnObject();
        }
        else if (other.gameObject.tag == "entrance_door")
        {
            resetStayingOnObject();
        }
    }
    /*-------------------------------*/

    /*-------------------------------------------------*/
    // Effecting on player life
    public int getHealth()
    {
        return health.health;
    }

    public int getMaxHealth()
    {
        return MAX_HEALTH;
    }

    public void increaseHealth(int amount)
    {
        health.increaseHealth(amount);
    }

    public void decreaseHealth(int amount)
    {
        if (!isPaining && amount > 0)
        {
            playPainSound();
            StartCoroutine(onPain());
            health.decreaseHealth(amount);
            if (health.isDead())
            {
                removeInteractive();
                hasDeath();
            }
        }
    }

    public IEnumerator onPain()
    {
        body.gravityScale = DEFAULT_GRAVITY;
        body.velocity = new Vector2(0.0f, 0.0f);
        body.AddForce(new Vector2(-playerDirection, 0), ForceMode2D.Impulse);
        isPaining = true;
        spriteRenderer.material.color = new Color(spriteRenderer.material.color.r, spriteRenderer.material.color.g, spriteRenderer.material.color.b, 0.5f);

        yield return new WaitForSeconds(PAIN_DURATION);

        spriteRenderer.material.color = new Color(spriteRenderer.material.color.r, spriteRenderer.material.color.g, spriteRenderer.material.color.b, 1.0f);
        isPaining = false;
    }

    public void hasDeath()
    {
        if (numberOfLife > 0)
        {
            numberOfLife--;
            StartCoroutine(resetPlayer());
        }
        else
        {
            PlayerPrefs.SetInt("curScore", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene("gameover");
        }
    }
    /*-------------------------------------------------*/

    /*-------------------------------------------------*/
    // Player movement
    public void setDirection(PlayerDirection direction)
    {
        switch (direction)
        {
            case PlayerDirection.LEFT:
                playerDirection = -1;
                break;

            case PlayerDirection.RIGHT:
                playerDirection = 1;
                break;
        }
    }

    public void horizontalMoving(float deltaTime)
    {
        gameObject.transform.Translate(new Vector3(deltaTime * speed * playerHorizontalMovement, 0.0f, 0.0f));
    }

    public void moveLeft()
    {
        playerHorizontalMovement -= 0.05f;
        playerHorizontalMovement = Mathf.Clamp(playerHorizontalMovement, -1.0f, 1.0f);
        setDirection(Player.PlayerDirection.LEFT);
        flip();
    }

    public void stopMovingLeftRight()
    {
        playerHorizontalMovement = 0.0f;
    }

    public void moveRight()
    {
        playerHorizontalMovement += 0.05f;
        playerHorizontalMovement = Mathf.Clamp(playerHorizontalMovement, -1.0f, 1.0f);
        setDirection(Player.PlayerDirection.RIGHT);
        flip();
    }

    public void climbUp()
    {
        if (climbOpened && canClimb)
        {
            if (!isClimbing)
            {
                body.gravityScale = 0f;
                body.velocity = new Vector2(0, 0);
                isClimbing = true;
                isJumping = false;
                climbing();
            }
            else
            {
                transform.Translate(new Vector3(0.0f, Time.deltaTime * Player.CLIMBING_SPEED, 0.0f));
            }
        }
    }

    public void climbDown()
    {
        if (body.gravityScale == 0.0f)
        {
            isClimbing = true;
            transform.Translate(new Vector3(0.0f, Time.deltaTime * Player.CLIMBING_SPEED * -1, 0.0f));
        }
    }
    /*-------------------------------------------------*/

    /*-------------------------------------------------*/
    // using basis skill
    public void jump()
    {
        playJumpSound();
        isJumping = true;
        body.velocity = new Vector2(0, 0);
        body.gravityScale = DEFAULT_GRAVITY;
        body.AddForce(new Vector2(0, JUMP_INTENSITY), ForceMode2D.Impulse);
    }

    public void jumpOnLadder()
    {
        canDoubleJump = false;
        isJumping = true;
        isClimbing = false;
        body.velocity = new Vector2(0, 0);
        body.gravityScale = DEFAULT_GRAVITY;
        body.AddForce(new Vector2(0, JUMP_ON_LADDER_INTENSITY), ForceMode2D.Impulse);
        jumping();
    }

    public void jumpOnMushroom(float height)
    {
        body.velocity = new Vector2(0.0f, 0.0f);
        body.gravityScale = DEFAULT_GRAVITY;
        Debug.Log(height);
        body.AddForce(new Vector2(0, height), ForceMode2D.Impulse);
        isJumping = true;
        jumping();
    }

    public void fireActivated()
    {
        if(!isReloading)
        {
            throwing();
        }
    }

    public void fire()
    {
        if (isFullEnergy())
        {
            throwBigBanana();
        }
        else
        {
            throwBanana();
        }
        
        StartCoroutine(reloading());
    }

    private void throwBanana()
    {
        playThrowSound();
        if (playerDirection == (int)PlayerDirection.LEFT)
        {
            Instantiate(leftBanana, bananaTransform.position, bananaTransform.rotation);
        }
        else if (playerDirection == (int)PlayerDirection.RIGHT)
        {
            Instantiate(rightBanana, bananaTransform.position, bananaTransform.rotation);
        }
    }

    private void throwBigBanana()
    {
        consumeEnergy(MAX_ENERGY);

        if (playerDirection == (int)PlayerDirection.LEFT)
        {
            Instantiate(leftBigBanana, bananaTransform.position, bananaTransform.rotation);
        }
        else if (playerDirection == (int)PlayerDirection.RIGHT)
        {
            Instantiate(rightBigBanana, bananaTransform.position, bananaTransform.rotation);
        }
    }

    public IEnumerator reloading()
    {
        isReloading = true;
        yield return new WaitForSeconds(RELOAD_DURATION);
        isReloading = false;
    }

    public bool isFullEnergy()
    {
        return energy == MAX_ENERGY;
    }

    public void increaseEnergy(int amount)
    {
        energy += amount;
        if (energy > MAX_ENERGY)
            energy = MAX_ENERGY;

        changeButtonFire(isFullEnergy());
    }

    public void consumeEnergy(int amount)
    {
        if (energy >= amount)
            energy -= amount;

        changeButtonFire(isFullEnergy());
    }

    public void changeButtonFire(bool isFullEnergy)
    {
        if (isFullEnergy)
        {
            buttonFire.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        }
        else
        {
            buttonFire.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
    /*-------------------------------------------------*/

    /*-------------------------------------------------*/
    // Player animation
    public int getPlayerState()
    {
        return animator.GetInteger("player_state");
    }

    public void returnToPreviousState()
    {
        animator.SetInteger("player_state", (int)previousStatus);
    }

    public void standing()
    {
        isJumping = false;
        previousStatus = PlayerStatus.PLAYER_STANDING;
        animator.SetInteger("player_state", (int)PlayerStatus.PLAYER_STANDING);
    }

    public bool isStanding()
    {
        return getPlayerState() == (int)PlayerStatus.PLAYER_STANDING;
    }

    public void running()
    {
        previousStatus = PlayerStatus.PLAYER_RUNNING;
        animator.SetInteger("player_state", (int)PlayerStatus.PLAYER_RUNNING);
    }

    public bool isRunning()
    {
        return getPlayerState() == (int)PlayerStatus.PLAYER_RUNNING;
    }

    public void jumping()
    {
        previousStatus = PlayerStatus.PLAYER_JUMPING;
        animator.SetInteger("player_state", (int)PlayerStatus.PLAYER_JUMPING);
    }

    public void throwing()
    {
        animator.SetInteger("player_state", (int)PlayerStatus.PLAYER_THROWING);
    }

    public bool isThrowing()
    {
        return getPlayerState() == (int)PlayerStatus.PLAYER_THROWING;
    }

    public void climbing()
    {
        previousStatus = PlayerStatus.PLAYER_CLIMBING;
        animator.SetInteger("player_state", (int)PlayerStatus.PLAYER_CLIMBING);
    }

    public void flip()
    {
        gameObject.transform.localScale = new Vector3(playerDirection, 1, 1);
    }
    /*-------------------------------------------------*/

    public void intergrateInteractive()
    {
        interaction = true;
    }

    public void removeInteractive()
    {
        interaction = false;
    }

    public void openJumpSkill()
    {
        canDoubleJump = true;
    }

    public void closeJumpSkill()
    {
        canDoubleJump = false;
    }

    public void openClimbSkill()
    {
        canClimb = true;
    }

    public void closeClimbSkill()
    {
        canClimb = false;
    }

    public void openShootSkill()
    {
        canShoot = true;
    }

    public void closeShootSkill()
    {
        canShoot = false;
    }

    public IEnumerator restrictSpeed()
    {
        speed = 1.0f;
        yield return new WaitForSeconds(RESTRICT_DURATION);
        speed = ORIGINAL_SPEED;
    }

    /*---------------------------------------------------*/
    // Initialize for player
    public void initPlayerLife()
    {
        numberOfLife = ORIGINAL_LIFE;
        health = new Health(MAX_HEALTH, MAX_HEALTH);
        isPaining = false;
    }

    public void initPlayerAttributes()
    {
        energy = 0;
        speed = ORIGINAL_SPEED;
        fallingVelocity = 0;
        playerDirection = (int)PlayerDirection.RIGHT;
        playerHorizontalMovement = 0.0f;
    }

    public void initPlayerParts()
    {
        camera = GameObject.Find("Main Camera");
        buttonFire = GameObject.Find("ButtonFire");
    }

    public void initPlayerComponents()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    public void initPlayerStatus()
    {
        previousStatus = PlayerStatus.PLAYER_STANDING;
        standing();
        stayingOnObject = gameObject;
    }

    public void initPlayerBasicSkills(int level)
    {
        bananaTransform = GameObject.Find("ThrowPosition").transform;

        isJumping = false;
        canDoubleJump = false;

        canClimb = false;
        isClimbing = false;

        canShoot = true;
        isReloading = false;

        //when the player can get double jump skill
        if (level >= LEVEL_OPEN_DOUBLEJUMP)
            doubleJumpOpened = true;
        else
            doubleJumpOpened = false;

        //when the player can open climb skill
        if (level >= LEVEL_OPEN_CLIMB)
            climbOpened = true;
        else
            climbOpened = false;

        //when the player can open shoot skill
        if (level >= LEVEL_OPEN_SHOOT)
            shootOpened = true;
        else
            shootOpened = false;
    }

    public void initMapCollarborative()
    {
        originalPosition = gameObject.transform.position;

        if (SceneManager.GetActiveScene().name != "level demo")
            currentLevel = int.Parse(SceneManager.GetActiveScene().name.Substring(6));
        else
            currentLevel = 1;

        mapItems = new MapItems(MainScene.instance.GetCurrentLevelKeys(currentLevel));
        keyDisplay.text = "Remain keys: " + mapItems.keys;
    }
    /*-----------------------------------------------------------*/

    /*-----------------------------------------------------------*/
    // Audio
    private void initAudio()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void playThrowSound()
    {
        if (throwSound != null)
            audioSource.PlayOneShot(throwSound);
    }

    public void playPowerThrowSound()
    {
        if (powerThrowSound != null)
            audioSource.PlayOneShot(powerThrowSound);
    }

    public void playJumpSound()
    {
        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
    }

    public void playPainSound()
    {
        if (painSound != null)
            audioSource.PlayOneShot(painSound);
    }

    public void playDeadSound()
    {
        if (deadSound != null)
            audioSource.PlayOneShot(deadSound);
    }

    public void playTeleportSound()
    {
        if (teleportSound != null)
            audioSource.PlayOneShot(teleportSound);
    }

    public void playFruitSound()
    {
        if (fruitSound != null)
            audioSource.PlayOneShot(fruitSound);
    }

    public void playFinishedLevelSound()
    {
        if (finishedLevelSound != null)
            audioSource.PlayOneShot(finishedLevelSound);
    }
    /*-----------------------------------------------------------*/

    /*-----------------------------------------------------------*/
    // Others
    public void shiftCamera()
    {
        camera.transform.position = new Vector3
            (
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                CAMERA_DEPTH_POSITION
            );
    }

    public void resetStayingOnObject()
    {
        stayingOnObject = gameObject;
    }


    public IEnumerator beTrapped()
    {
        speed = ORIGINAL_SPEED / 2;
        closeJumpSkill();
        yield return new WaitForSeconds(RESTRICT_DURATION);
        openJumpSkill();
        speed = ORIGINAL_SPEED;
    }

    IEnumerator resetPlayer()
    {
        interaction = false;
        isPaining = true;
        float currentTime = Time.realtimeSinceStartup;

        MainScene.instance.stopAllAnimation();
        while (Time.realtimeSinceStartup < currentTime + 1.0f)
        {
            yield return null;
        }
        MainScene.instance.resumeAllAnimation();
        
        health.health = health.maxHealth;
        energy = 0;
        speed = ORIGINAL_SPEED;
        fallingVelocity = 0;
        playerDirection = (int)PlayerDirection.RIGHT;
        playerHorizontalMovement = 0.0f;
        body.velocity = new Vector2(0.0f, 0.0f);
        previousStatus = PlayerStatus.PLAYER_STANDING;
        standing();
        stayingOnObject = gameObject;
        isJumping = false;
        canDoubleJump = false;
        canClimb = false;
        isClimbing = false;
        canShoot = true;
        isReloading = false;

        interaction = true;
        gameObject.transform.position = new Vector3(
                                                originalPosition.x,
                                                originalPosition.y,
                                                originalPosition.z
                                                   );

        isPaining = false;
        interaction = true;
    }
    /*-----------------------------------------------------------*/
}