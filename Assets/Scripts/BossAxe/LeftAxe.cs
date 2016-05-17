using UnityEngine;
using System.Collections;

public class LeftAxe : MonoBehaviour {

    private const float flightVelocityScale = 15.0f;

    private Player target;
    private Rigidbody2D body;

	void Start () 
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject)
        {
            target = playerObject.GetComponent<Player>();
        }

        fly();
        StartCoroutine(autoDestroy());
	}
	
	void Update () 
    {
        gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 10.0f);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == target.tag)
        {
            target.decreaseHealth(BossAxe.AXE_DAMAGE);
        }

        if (other.gameObject.tag == "ground" || other.gameObject.tag == "dead_zone")
        {
            destroyThisShit();
        }
    }

    private void fly()
    {
        Vector3 direction = playerDirection() * flightVelocityScale;
        body.AddForce(new Vector2(direction.x, direction.y), ForceMode2D.Impulse);
    }

    private Vector3 playerDirection()
    {
        return (target.transform.position - gameObject.transform.position).normalized;
    }

    public void destroyThisShit()
    {
        Destroy(gameObject);
    }

    IEnumerator autoDestroy()
    {
        yield return new WaitForSeconds(5.0f);
        destroyThisShit();
    }
}
