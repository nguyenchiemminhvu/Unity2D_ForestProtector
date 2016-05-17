using UnityEngine;
using System.Collections;

public class WarningTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            MainScene.instance.showWarning();
            Destroy(gameObject);
        }
    }
}
