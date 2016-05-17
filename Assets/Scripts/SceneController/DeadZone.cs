using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            return;
        }

        Destroy(other.gameObject);
    }
}
