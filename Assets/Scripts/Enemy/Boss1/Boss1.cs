using UnityEngine;
using System.Collections;
using System;

public class Boss1 : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    bool isRightFacing;
    [Range(0, 15)]
    public int hp;
    public enum bossState
    {
        moving = 1,
        attack,
        die
    }
    int moveSpeed;
    public bossState state;
    float startPosY;
    public GameObject keyPrefabs;

    // Use this for initialization
    void Start()
    {
        hp = 15;
        moveSpeed = -5;
        isRightFacing = false;
        state = bossState.moving;
        player = GameObject.Find("Player").GetComponent<Player>();
        startPosY = transform.position.y;
        //Debug.Log(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        if (hp > 0)
        {
            if (state == bossState.moving || state == bossState.attack)
            {
                Moving();
            }
            else { }
        }
        else
        {
            if (state != bossState.die)
            {
                state = bossState.die;
                Die();
            }

        }
    }
    void Moving()
    {
        if (transform.position.y < startPosY - 0.3)
        {
            transform.position = new Vector3(transform.position.x + moveSpeed / 2 * Time.deltaTime, transform.position.y + 1 * Time.deltaTime, transform.position.z);
        }
        else if (transform.position.y > startPosY + 0.3)
        {
            transform.position = new Vector3(transform.position.x + moveSpeed / 2 * Time.deltaTime, transform.position.y - 1 * Time.deltaTime, transform.position.z);
        }
        else
        {
            state = bossState.moving;
            transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);

        }
    }
    public void Attack()
    {
        if (state != bossState.attack)
        {
            Vector2 dir = (player.transform.position - transform.position) / 1.8f;
            GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
        }
        state = bossState.attack;
    }
    void ChangeFacing()
    {
        isRightFacing = !isRightFacing;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        moveSpeed *= -1;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "ground")
        {
            if (state == bossState.die)
            {
                InsKey();
            }
            else ChangeFacing();
        }
        if (other.gameObject.name == "Player")
        {
            player.decreaseHealth(5);
        }
    }
    public void Hurt()
    {
        hp--;
    }
    void Die()
    {
        Score.addScore(500);
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        GetComponent<Rigidbody2D>().gravityScale = 2;
    }
    void InsKey()
    {
        Instantiate(keyPrefabs, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
