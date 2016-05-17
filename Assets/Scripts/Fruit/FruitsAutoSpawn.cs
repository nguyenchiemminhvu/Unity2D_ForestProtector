using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitsAutoSpawn : MonoBehaviour {

    public const float SPAWN_DURATION = 10.0f;

    public List<Sprite> fruitSprites;
    public Player player;
    public SpriteRenderer renderer;
    private bool wasEaten;

	void Start () 
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = fruitSprites[Random.Range(0, fruitSprites.Count)];
        wasEaten = false;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!wasEaten)
            {
                player.increaseHealth(1);
                player.increaseEnergy(1);
                player.playFruitSound();
                StartCoroutine(autoSpawn());
            }
        }
    }

    IEnumerator autoSpawn()
    {
        renderer.sprite = null;
        wasEaten = true;
        yield return new WaitForSeconds(SPAWN_DURATION);
        wasEaten = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = fruitSprites[Random.Range(0, fruitSprites.Count)];
    }
}
