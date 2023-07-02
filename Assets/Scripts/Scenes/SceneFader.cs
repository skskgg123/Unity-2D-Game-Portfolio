using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;

    public float fadeInTime = 0f;

    private void Start()
    {
        
    }

    public void FadeIn(float fadeTime)
    {
        StartCoroutine(FadeInTime(fadeTime));
    }

    IEnumerator FadeInTime(float fadeTime)
    {
        img.color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(fadeTime);

        float t = 1.0f;

        while (t >= 0)
        {
            t -= Time.deltaTime;
            img.color = new Color(255, 255, 255, t);
            yield return 0;
        }

    }

    public void FadeTo(string sceneName)
    {
        StartCoroutine(FadeOutTime(sceneName));
    }

    public void FadeTo(int sceneNum)
    {
        StartCoroutine(FadeOutTime(sceneNum));
    }


    //어두워지는 것:  알파값이 0 -> 1
    IEnumerator FadeOutTime(string sceneName)
    {
        float t = 0.0f;

        while (t <= 1)
        {
            t += Time.deltaTime;
            img.color = new Color(255, 255, 255, t);
            yield return 0;
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOutTime(int sceneNum)
    {
        float t = 0.0f;

        while (t <= 1)
        {
            t += Time.deltaTime;
            img.color = new Color(255, 255, 255, t);
            yield return 0;
        }

        SceneManager.LoadScene(sceneNum);
    }

}
