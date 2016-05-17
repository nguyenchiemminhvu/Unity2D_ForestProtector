using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionButton : MonoBehaviour {

    public GameObject panel;

    public void touchOnInstructionButton()
    {
        panel.GetComponentInChildren<Text>().text = "";
        panel.SetActive(false);
        MainScene.instance.resumeAllAnimation();
    }
}
