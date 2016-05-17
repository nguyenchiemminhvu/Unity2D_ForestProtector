using UnityEngine;
using System.Collections;

public class BossHpBar : MonoBehaviour {

    Boss1 boss;
    float startScale;
	// Use this for initialization
	void Start () {
        boss = GameObject.Find("BatBoss").GetComponent<Boss1>();
        startScale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(startScale * ((float) boss.hp / 15), transform.localScale.y, transform.localScale.z);
	}
}
