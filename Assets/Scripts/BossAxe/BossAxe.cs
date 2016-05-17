using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossAxe : MonoBehaviour {

    public const float JUMP_INTENSITY = 20.0f;
    public const float RELOAD_AXE_DURATION = 2.0f;
    public const float RELOAD_LANDMINE_DURATION = 2.0f;
    public const float SPAWN_BEAR_TRAP_DURATION = 10.0f;
    public const float PAIN_DURATION = 1.0f;
    public const float TIME_OF_REST = 1.5f;
    public const int AXE_DAMAGE = 5;

    enum BossActionType
    {
        IDLE,
        ATTACK
    };

    enum BossStatus
    {
        IDLING,
        JUMPING_TO_PLAYER,
        THROWING_AXE,
        DYING
    };

    /*---------------------------------------------------*/
    // boss component
    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer renderer;
    /*---------------------------------------------------*/

    public Player target;
    private int direction;

    /*---------------------------------------------------*/
    // status
    private BossStatus state;
    private BossActionType currentAction;
    delegate void BossAction();
    BossAction actions;

    [HideInInspector]
    public bool isActivated;
    [HideInInspector]
    public bool isVulnerable;

    private bool isAttacking;
    private bool isFirstAttack;
    /*---------------------------------------------------*/

    private Health health;
    private bool isPaining;

    /*---------------------------------------------------*/
    // skill prefab
    public GameObject tempest;
    public Transform tempestTransform;
    public GameObject leftAxe;
    public GameObject rightAxe;
    public GameObject landMine;
    public GameObject bearTrap;
    public Transform throwAxeTransform;
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // audio
    private AudioSource audioSource;

    public AudioClip throwAxeSound;
    public AudioClip jumpSound;
    public AudioClip tempestSound;
    /*---------------------------------------------------*/

	void Start () 
    {
        initComponents();
        target = GameObject.Find("Player").GetComponent<Player>();
        health = new Health(20, 20);

        deactivated();
        
        becameInvulnerable();
        isPaining = false;
        
        currentAction = BossActionType.IDLE;
        state = BossStatus.IDLING;
        isAttacking = false;
        isFirstAttack = true;

        flip();
    }

    /*---------------------------------------------------*/
    // Boss's operation
    public IEnumerator doNothing()
    {
        becameVulnerable();
        yield return new WaitForSeconds(TIME_OF_REST);
        becameInvulnerable();
        chooseHowToAttack();
        StartCoroutine(attack());
    }

    public void chooseHowToAttack()
    {
        if(!isFirstAttack)
        {
            int rand = Random.Range(0, 3);
            switch (rand)   
            {
            case 0:
                actions = jumpToPlayer;
                break;
            case 1:
                actions = throwAxe;
                break;
            case 2:
                actions = setLandMine;
                break;
            }
        }
        else
        {
            actions = jumpToPlayer;
            isFirstAttack = false;
        }
    }

    public IEnumerator attack()
    {
        actions();
        yield return new WaitUntil(() => isAttacking == false);
        StartCoroutine(doNothing());
    }
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // Collision and trigger detecting
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "ground")
        {
            body.velocity = new Vector2(0.0f, 0.0f);
            makeTempest();
            idling();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "ground")
        {
            jumping();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == target.tag)
        {
            if (!isActivated)
            {
                activated();
            }
            else
            {
                target.decreaseHealth(target.getMaxHealth() / 2);
            }
        }

        if (other.gameObject.tag == "player_banana")
        {
            if (isVulnerable)
                decreaseHealth(Player.BANANA_DAMAGE);
        }

        if (other.gameObject.tag == "player_big_banana")
        {
            if (isVulnerable)
                decreaseHealth(Player.BIG_BANANA_DAMAGE);
        }
    }
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // Activation
    public void activated()
    {
        isActivated = true;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject obj = gameObject.transform.GetChild(i).gameObject;
            if (obj.name == "BossTrigger")
                Destroy(obj);
        }

        StartCoroutine(doNothing());
        autoSpawnBearTrap();
    }

    public void deactivated()
    {
        isActivated = false;
        state = BossStatus.DYING;
    }
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // Boss's life
    public void becameVulnerable()
    {
        isVulnerable = true;
    }

    public void becameInvulnerable()
    {
        isVulnerable = false;
    }

    public void decreaseHealth(int amount)
    {
        if (!isPaining)
        {
            StartCoroutine(onPain());
            health.decreaseHealth(amount);
            if (health.isDead())
            {
                StopAllCoroutines();
                hasDeath();
            }
        }
    }

    public void hasDeath()
    {
        StartCoroutine(dying());
    }

    IEnumerator dying()
    {
        becameInvulnerable();
        deactivated();

        body.gravityScale = 0.0f;
        Destroy(gameObject.GetComponent<BoxCollider2D>());

        while (renderer.material.color.a > 0)
        {
            yield return new WaitForEndOfFrame();
            renderer.material.color = new Color(
                                                Random.Range(0.0f, 1.0f),
                                                Random.Range(0.0f, 1.0f),
                                                Random.Range(0.0f, 1.0f),
                                                renderer.material.color.a - 0.005f
                                                );
        }

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("finished");
    }
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // Skills
    public void jumpToPlayer()
    {
        isAttacking = true;
        flip();
        body.AddForce(new Vector2(targetDistance() / (JUMP_INTENSITY / 15) * direction, JUMP_INTENSITY), ForceMode2D.Impulse);
        playJumpSound();
    }

    public void makeTempest()
    {
        Instantiate(tempest, tempestTransform.position, tempestTransform.rotation);
        playTempestSound();
        isAttacking = false;
    }

    public void throwAxe()
    {
        isAttacking = true;
        flip();
        if (direction == -1)
            Instantiate(leftAxe, throwAxeTransform.position, throwAxeTransform.rotation);
        else
            Instantiate(rightAxe, throwAxeTransform.position, throwAxeTransform.rotation);

        playThrowAxeSound();
        StartCoroutine(reloadingAxe());
    }

    public void setLandMine()
    {
        isAttacking = true;
        flip();

        Instantiate(landMine, 
                    new Vector3(
                            target.transform.position.x,
                            target.transform.position.y + 3.0f,
                            landMine.transform.position.z
                               ), 
                    landMine.transform.rotation
                   );

        StartCoroutine(reloadingLandMine());
    }

    public void setBearTrap()
    {
        Instantiate(bearTrap,
                    new Vector3(
                            gameObject.transform.position.x + direction * 2,
                            gameObject.transform.position.y,
                            bearTrap.transform.position.z
                               ),
                    bearTrap.transform.rotation
                   );
    }
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // Animations
    public void idling()
    {
        animator.SetInteger("state", (int)BossStatus.IDLING);
        state = BossStatus.IDLING;
    }

    public void jumping()
    {
        animator.SetInteger("state", (int)BossStatus.JUMPING_TO_PLAYER);
        state = BossStatus.JUMPING_TO_PLAYER;
    }

    public void throwing()
    {
        animator.SetInteger("state", (int)BossStatus.THROWING_AXE);
        state = BossStatus.THROWING_AXE;
    }
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // Sound effect
    private void playThrowAxeSound()
    {
        if (throwAxeSound != null)
        {
            audioSource.PlayOneShot(throwAxeSound);
        }
    }

    private void playJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void playTempestSound()
    {
        if(tempestSound != null)
        {
            audioSource.PlayOneShot(tempestSound);
        }
    }
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // Initialize components
    private void initComponents()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    /*---------------------------------------------------*/

    /*---------------------------------------------------*/
    // Others
    private void flip()
    {
        calculateDirection();
        gameObject.transform.localScale = new Vector3(-direction, 1, 1);
    }

    private void calculateDirection()
    {
        if (gameObject.transform.position.x >= target.transform.position.x)
            direction = -1;
        else
            direction = 1;
    }

    private float targetDistance()
    {
        return (gameObject.transform.position - target.transform.position).magnitude;
    }

    private void autoSpawnBearTrap()
    {
        InvokeRepeating("setBearTrap", 0.0f, SPAWN_BEAR_TRAP_DURATION);
    }

    IEnumerator reloadingAxe()
    {
        yield return new WaitForSeconds(RELOAD_AXE_DURATION);
        isAttacking = false;
    }

    IEnumerator reloadingLandMine()
    {
        yield return new WaitForSeconds(RELOAD_LANDMINE_DURATION);
        isAttacking = false;
    }

    IEnumerator onPain()
    {
        isPaining = true;
        renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        yield return new WaitForSeconds(PAIN_DURATION);
        renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        isPaining = false;
    }
    /*---------------------------------------------------*/
}
