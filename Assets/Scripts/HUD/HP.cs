using UnityEngine;
using System.Collections;

public class HP : MonoBehaviour {

    Player player;
    float ratio;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        ratio = (float)player.getHealth() / (float)player.getMaxHealth();
        transform.GetChild(1).transform.localScale =
            new Vector3(transform.GetChild(0).transform.localScale.x * ratio,
                        transform.GetChild(1).transform.localScale.y,
                        transform.GetChild(1).transform.localScale.z);

    }
}
