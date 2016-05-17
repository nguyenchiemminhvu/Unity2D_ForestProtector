using UnityEngine;
using System.Collections;

public class fruits : MonoBehaviour {

    public Sprite[] fruitSprites;
    public Player player;

    void Start () {
        GetComponent<SpriteRenderer>().sprite = fruitSprites[Random.Range(0, fruitSprites.Length)];
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.increaseEnergy(1);
            player.playFruitSound();
            Score.addScore(5);
            GameObject.Destroy(gameObject);
        }
    }
}
