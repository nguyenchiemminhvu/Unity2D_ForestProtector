using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

    public Text scoreText;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score:" + Score.score;
	}
}
