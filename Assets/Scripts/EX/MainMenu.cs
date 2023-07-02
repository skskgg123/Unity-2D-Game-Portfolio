using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Animator backgroundAnim;

    public int menuNum = 0;
    public TextMeshProUGUI buttonText;
    public SceneFader fader;
    public GameObject buttons;
    public Animator checkKey;
    public AudioClip[] clip;
    public AudioClip bgClip;
    public GameObject sceneMenu;

    private void Start()
    {
        SoundManager.instance.BackgroundSound(bgClip);

        //저장기록 초기화하고 내보내기
        //PlayerPrefs.DeleteAll();
    }

    private void Update()
    {

        if (menuNum < 0)
        {
            menuNum = 0;
        }
        else if(menuNum >3 )
        {
            menuNum = 3;
        }

        

        switch (menuNum)
        {
            case 0:
                buttonText.text = "NEW GAME";
                ResetButton();
                buttons.transform.GetChild(0).gameObject.SetActive(true);
                
                break;
            case 1:
                buttonText.text = "SCENE";
                ResetButton();
                buttons.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 2:
                buttonText.text = "OPTION";
                ResetButton();
                buttons.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 3:
                buttonText.text = "QUIT";
                ResetButton();
                buttons.transform.GetChild(3).gameObject.SetActive(true);
                break;

        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            checkKey.SetBool("isCheck", false);
        }

    }

    public void ResetButton()
    {
        for (int i = 0; i < buttons.transform.childCount; i++)
        {
            buttons.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void NewGame(string sceneName)
    {
        fader.FadeTo(sceneName);
        SoundManager.instance.SFXPlay("001_Hover_01", clip[1]);
    }

    public void Scene(string sceneName)
    {
        sceneMenu.SetActive(true);
        SoundManager.instance.SFXPlay("001_Hover_01", clip[1]);

    }

    public void Option()
    {
        checkKey.SetBool("isCheck", true);
        SoundManager.instance.SFXPlay("001_Hover_01", clip[1]);
    }

    public void QuitGame()
    {
        checkKey.SetBool("isCheck", true);
        Application.Quit();
        SoundManager.instance.SFXPlay("001_Hover_01", clip[1]);
    }

    public void ArrowR()
    {
        menuNum++;
        backgroundAnim.SetInteger("OptionMove", menuNum);

        SoundManager.instance.SFXPlay("001_Hover_01", clip[0]);
    }

    public void ArrowL()
    {

        menuNum--;
        backgroundAnim.SetInteger("OptionMove", menuNum);

        SoundManager.instance.SFXPlay("001_Hover_01", clip[0]);
    }

    public void MainMenuButton()
    {
        sceneMenu.SetActive(false);
        SoundManager.instance.SFXPlay("001_Hover_01", clip[1]);
    }

    public void OptionClose()
    {
        checkKey.SetBool("isCheck", false);
        SoundManager.instance.SFXPlay("001_Hover_01", clip[1]);
    }

}
