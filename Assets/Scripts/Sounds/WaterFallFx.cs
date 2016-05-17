using UnityEngine;
using System.Collections;

public class WaterFallFx : MonoBehaviour {

    AudioSource sound;
    GameObject player;
    GameObject sceneSize;
    float distance;
    float rate;
    float maxDistance;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        sceneSize = GameObject.Find("SceneSize");
        sound = GetComponent<AudioSource>();
        maxDistance = Mathf.Abs(sceneSize.transform.position.x - transform.position.x);
    }
	
	// Update is called once per frame
	void Update () {
        distance = Mathf.Abs(transform.position.x - player.transform.position.x);
        // volume min: 0.3 max: 0.7
        sound.volume = 0.3f + 0.4f *(1 - (distance / maxDistance));
	}
}
