using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public SceneFader fader;
    public GameObject helpManual;

    // Start is called before the first frame update
    void Start()
    {
        //씬 시작 페이드인 효과
        fader.FadeIn(1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HowToPlay()
    {
        helpManual.SetActive(true);
    }

    public void ManualExit()
    {
        helpManual.SetActive(false);
    }

    public void Menu()
    {
        fader.FadeTo("MainMenu");
    }

}
