using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishedScene : MonoBehaviour {

    public void backToMainMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
