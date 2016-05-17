using UnityEngine;
using System.Collections;

public class InstructionTrigger : MonoBehaviour {

    public string text;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            MainScene.instance.giveInstruction(text);
            Destroy(gameObject);
        }
    }
}
