using UnityEngine;
using System.Collections;

public class TurtleFood : MonoBehaviour {

    private const float SPAWN_DURATION = 10.0f;

    private SpriteRenderer renderer;
    private Sprite sprite;
    private bool isReloading;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite = renderer.sprite;
        isReloading = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isReloading)
            {
                MainScene.instance.player.mapItems.numberOfTurtleFood++;
                StartCoroutine(autoSpawn());
            }
            
        }
    }

    IEnumerator autoSpawn()
    {
        isReloading = true;
        renderer.sprite = null;
        yield return new WaitForSeconds(SPAWN_DURATION);
        renderer.sprite = sprite;
        isReloading = false;
    }
}
