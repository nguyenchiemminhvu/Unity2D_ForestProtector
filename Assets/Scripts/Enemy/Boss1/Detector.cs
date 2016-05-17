using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("player enter trigger");
            GetComponentInParent<Boss1>().Attack();
        }
    }
}
