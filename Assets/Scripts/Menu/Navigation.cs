using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Navigation : MonoBehaviour {

    
    public GameObject pausePanel;
    public GameObject aboutPanel;
    public void StartClick()
    {
        PlayerPrefs.SetInt("curScore", 0);
        PlayerPrefs.Save();
        Score.score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("Level-1");
    }
    public void OptionsClick()
    {

    }
    public void AboutClick()
    {
        aboutPanel.SetActive(true);
    }
    public void QuitClick()
    {
        Application.Quit();
    }
    public void PMResumeClick()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void PMMenuClick()
    {
        SceneManager.LoadScene("menu");
    }
    public void PauseClick()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    public void APBackClick()
    {
        aboutPanel.SetActive(false);
    }
}
