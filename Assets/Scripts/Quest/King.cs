using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class King : DrawDialog
{
    public GameObject go;
    public GameObject questReady;
    public GameObject answerButton;
    private Button dialogButton;
    public Text noText;
    public Text noText_2;
    public Text yesText;

    private bool isAnswer = false;
    private bool isLastTalk = false;
    public bool isEnd = false;

    public AudioClip[] clip;

    private void Awake()
    {
        dialogButton = nextButton.GetComponent<Button>();
        isEnd = false;
    }

    public void Quest()
    {
        if(isEnd)
        {
            return;
        }

        isAnswer = true;
        go.SetActive(true);
        DialogSelectEvent(1);
        questReady.SetActive(false);
        SoundManager.instance.SFXPlay("001_Hover_01", clip[0]);


    }

    public override void EndDialog()
    {
        InitDialog();

        nextButton.SetActive(false);
        background.SetActive(false);

        if (characterState != null)
            EndingDialog();


        //다음 대화가 있는지 체크
        if (nextDialog > -1)
        {
            DialogSelectEvent(nextDialog);
        }
        else
        {
            isEnd = true;
        }

    }

    public override void NextButton()
    {
        if (isButton)
        {
            DrawSentece();
        }
        else
        {
            if (isAnswer)
            {
                answerButton.SetActive(true);


                dialogButton.interactable = false;
            }
            else
            {
                DrawNext();
            }

        }

    }

    public void Yes()
    {
        isAnswer = false;
        answerButton.SetActive(false);
        DialogSelectEvent(2);
        dialogButton.interactable = true;
        SoundManager.instance.SFXPlay("001_Hover_01", clip[2]);
    }

    public void No()
    {

        answerButton.SetActive(true);
        DialogSelectEvent(3);
        dialogButton.interactable = false;
        noText_2.gameObject.SetActive(true);
        noText.enabled = false;
        SoundManager.instance.SFXPlay("001_Hover_01", clip[0]);
    }

    public void SecondNo()
    {
        DrawNext();
        isLastTalk = true;

        if (isLastTalk)
        {
            SoundManager.instance.SFXPlay("001_Hover_01", clip[1]);
            noText_2.enabled = false;
            yesText.gameObject.SetActive(true);
        }
    }

    public void NextScene()
    {
        //go.SetActive(false);
        SoundManager.instance.SFXPlay("001_Hover_01", clip[0]);
        fader.FadeTo(loadToScene);
    }

}
